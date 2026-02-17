using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CursosApp.Data;
using CursosApp.DTOs;
using CursosApp.Models;
using CursosApp.Services;

namespace CursosApp.Controllers;

// ── AUTH ──────────────────────────────────────────────────────────────────────
[ApiController][Route("api/auth")]
public class AuthController(AppDbContext db, IJwtService jwt) : ControllerBase
{
    [HttpPost("login")]
    public async Task<ActionResult<AuthResponse>> Login([FromBody] LoginRequest req)
    {
        var user = await db.Users.FirstOrDefaultAsync(u => u.Email == req.Email && u.IsActive);
        if (user is null || !BCrypt.Net.BCrypt.Verify(req.Password, user.PasswordHash))
            return Unauthorized(new { message = "Credenciales incorrectas" });
        user.LastLoginAt = DateTime.UtcNow;
        await db.SaveChangesAsync();
        return Ok(new AuthResponse(jwt.GenerateToken(user), user.Role, user.Name, user.Email, user.Id));
    }

    [HttpPost("register")]
    public async Task<ActionResult<AuthResponse>> Register([FromBody] RegisterRequest req)
    {
        if (await db.Users.AnyAsync(u => u.Email == req.Email))
            return Conflict(new { message = "El email ya está en uso" });
        var user = new User { Name = req.Name, Email = req.Email, PasswordHash = BCrypt.Net.BCrypt.HashPassword(req.Password) };
        db.Users.Add(user);
        await db.SaveChangesAsync();
        return Created("", new AuthResponse(jwt.GenerateToken(user), user.Role, user.Name, user.Email, user.Id));
    }

    [Authorize][HttpGet("me")]
    public async Task<ActionResult> Me()
    {
        var id   = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
        var user = await db.Users.FindAsync(id);
        if (user is null) return NotFound();
        return Ok(new { user.Id, user.Name, user.Email, user.Role, user.CreatedAt, user.LastLoginAt });
    }
}

// ── CATEGORIES ────────────────────────────────────────────────────────────────
[ApiController][Route("api/categories")]
public class CategoriesController(AppDbContext db) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult> GetAll() =>
        Ok(await db.Categories.Include(c => c.Courses)
            .Select(c => new CategoryDto(c.Id, c.Name, c.Description, c.Icon, c.IsActive, c.CreatedAt, c.Courses.Count(x => x.IsPublished)))
            .ToListAsync());

    [HttpGet("{id:int}")]
    public async Task<ActionResult> GetById(int id)
    {
        var c = await db.Categories.Include(x => x.Courses).FirstOrDefaultAsync(x => x.Id == id);
        if (c is null) return NotFound(new { message = "No encontrada" });
        return Ok(new CategoryDto(c.Id, c.Name, c.Description, c.Icon, c.IsActive, c.CreatedAt, c.Courses.Count));
    }

    [Authorize(Roles = "admin")][HttpPost]
    public async Task<ActionResult> Create([FromBody] CreateCategoryRequest req)
    {
        var c = new Category { Name = req.Name, Description = req.Description, Icon = req.Icon };
        db.Categories.Add(c); await db.SaveChangesAsync();
        return Created("", new CategoryDto(c.Id, c.Name, c.Description, c.Icon, c.IsActive, c.CreatedAt, 0));
    }

    [Authorize(Roles = "admin")][HttpPut("{id:int}")]
    public async Task<ActionResult> Update(int id, [FromBody] UpdateCategoryRequest req)
    {
        var c = await db.Categories.Include(x => x.Courses).FirstOrDefaultAsync(x => x.Id == id);
        if (c is null) return NotFound(new { message = "No encontrada" });
        c.Name = req.Name; c.Description = req.Description; c.Icon = req.Icon; c.IsActive = req.IsActive;
        await db.SaveChangesAsync();
        return Ok(new CategoryDto(c.Id, c.Name, c.Description, c.Icon, c.IsActive, c.CreatedAt, c.Courses.Count));
    }

    [Authorize(Roles = "admin")][HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var c = await db.Categories.Include(x => x.Courses).FirstOrDefaultAsync(x => x.Id == id);
        if (c is null) return NotFound(new { message = "No encontrada" });
        if (c.Courses.Any()) return Conflict(new { message = "Tiene cursos asociados" });
        db.Categories.Remove(c); await db.SaveChangesAsync();
        return NoContent();
    }
}

// ── COURSES ───────────────────────────────────────────────────────────────────
[ApiController][Route("api/courses")]
public class CoursesController(AppDbContext db) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<PagedResult<CourseDto>>> Search([FromQuery] CourseSearchParams p)
    {
        var role = User.FindFirst(ClaimTypes.Role)?.Value;
        var q    = db.Courses.Include(c => c.Category).AsQueryable();

        if (role != "admin" && role != "instructor") q = q.Where(c => c.IsPublished);
        if (!string.IsNullOrWhiteSpace(p.Query))
        {
            var s = p.Query.ToLower();
            q = q.Where(c => c.Title.ToLower().Contains(s) || c.Description.ToLower().Contains(s) || c.Instructor.ToLower().Contains(s));
        }
        if (p.CategoryId.HasValue) q = q.Where(c => c.CategoryId == p.CategoryId);
        if (!string.IsNullOrWhiteSpace(p.Level)) q = q.Where(c => c.Level == p.Level);
        if (p.MinPrice.HasValue) q = q.Where(c => c.Price >= p.MinPrice);
        if (p.MaxPrice.HasValue) q = q.Where(c => c.Price <= p.MaxPrice);
        if (p.DateFrom.HasValue) q = q.Where(c => c.CreatedAt >= p.DateFrom);
        if (p.DateTo.HasValue)   q = q.Where(c => c.CreatedAt <= p.DateTo);
        if (p.IsPublished.HasValue && (role == "admin" || role == "instructor"))
            q = q.Where(c => c.IsPublished == p.IsPublished);

        q = (p.SortBy.ToLower(), p.SortOrder.ToLower()) switch
        {
            ("price",       "asc") => q.OrderBy(c => c.Price),
            ("price",       _)     => q.OrderByDescending(c => c.Price),
            ("rating",      "asc") => q.OrderBy(c => c.Rating),
            ("rating",      _)     => q.OrderByDescending(c => c.Rating),
            ("enrollments", "asc") => q.OrderBy(c => c.EnrollmentCount),
            ("enrollments", _)     => q.OrderByDescending(c => c.EnrollmentCount),
            ("title",       "asc") => q.OrderBy(c => c.Title),
            ("title",       _)     => q.OrderByDescending(c => c.Title),
            (_,             "asc") => q.OrderBy(c => c.CreatedAt),
            _                      => q.OrderByDescending(c => c.CreatedAt),
        };

        var total = await q.CountAsync();
        var pages = (int)Math.Ceiling(total / (double)p.PageSize);
        var items = await q.Skip((p.Page - 1) * p.PageSize).Take(p.PageSize)
            .Select(c => new CourseDto(c.Id, c.Title, c.Description, c.Instructor, c.Price,
                c.DurationHours, c.Level, c.ImageUrl, c.IsPublished, c.EnrollmentCount, c.Rating,
                c.CreatedAt, c.UpdatedAt, c.CategoryId, c.Category.Name))
            .ToListAsync();

        return Ok(new PagedResult<CourseDto>(items, total, p.Page, p.PageSize, pages));
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult> GetById(int id)
    {
        var role   = User.FindFirst(ClaimTypes.Role)?.Value;
        var course = await db.Courses.Include(c => c.Category).FirstOrDefaultAsync(c => c.Id == id);
        if (course is null || (!course.IsPublished && role != "admin" && role != "instructor"))
            return NotFound(new { message = "No encontrado" });

        bool enrolled = false; int? progress = null;
        if (User.Identity?.IsAuthenticated == true)
        {
            var uid = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            var e   = await db.Enrollments.FirstOrDefaultAsync(x => x.UserId == uid && x.CourseId == id);
            enrolled = e is not null; progress = e?.ProgressPercent;
        }
        return Ok(new CourseDetailDto(course.Id, course.Title, course.Description, course.Instructor,
            course.Price, course.DurationHours, course.Level, course.ImageUrl, course.IsPublished,
            course.EnrollmentCount, course.Rating, course.CreatedAt, course.UpdatedAt,
            course.CategoryId, course.Category.Name, enrolled, progress));
    }

    [Authorize(Roles = "admin,instructor")][HttpPost]
    public async Task<ActionResult> Create([FromBody] CreateCourseRequest req)
    {
        if (!await db.Categories.AnyAsync(c => c.Id == req.CategoryId))
            return BadRequest(new { message = "Categoría no existe" });
        var c = new Course { Title=req.Title, Description=req.Description, Instructor=req.Instructor,
            Price=req.Price, DurationHours=req.DurationHours, Level=req.Level, ImageUrl=req.ImageUrl, CategoryId=req.CategoryId };
        db.Courses.Add(c); await db.SaveChangesAsync();
        await db.Entry(c).Reference(x => x.Category).LoadAsync();
        return Created("", ToDto(c));
    }

    [Authorize(Roles = "admin,instructor")][HttpPut("{id:int}")]
    public async Task<ActionResult> Update(int id, [FromBody] UpdateCourseRequest req)
    {
        var c = await db.Courses.Include(x => x.Category).FirstOrDefaultAsync(x => x.Id == id);
        if (c is null) return NotFound(new { message = "No encontrado" });
        if (!await db.Categories.AnyAsync(x => x.Id == req.CategoryId))
            return BadRequest(new { message = "Categoría no existe" });
        c.Title=req.Title; c.Description=req.Description; c.Instructor=req.Instructor;
        c.Price=req.Price; c.DurationHours=req.DurationHours; c.Level=req.Level;
        c.ImageUrl=req.ImageUrl; c.IsPublished=req.IsPublished; c.CategoryId=req.CategoryId;
        c.UpdatedAt=DateTime.UtcNow;
        await db.SaveChangesAsync(); await db.Entry(c).Reference(x => x.Category).LoadAsync();
        return Ok(ToDto(c));
    }

    [Authorize(Roles = "admin")][HttpPatch("{id:int}/publish")]
    public async Task<IActionResult> TogglePublish(int id)
    {
        var c = await db.Courses.FindAsync(id);
        if (c is null) return NotFound(new { message = "No encontrado" });
        c.IsPublished = !c.IsPublished; c.UpdatedAt = DateTime.UtcNow;
        await db.SaveChangesAsync();
        return Ok(new { id = c.Id, isPublished = c.IsPublished });
    }

    [Authorize(Roles = "admin")][HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var c = await db.Courses.FindAsync(id);
        if (c is null) return NotFound(new { message = "No encontrado" });
        db.Courses.Remove(c); await db.SaveChangesAsync();
        return NoContent();
    }

    [Authorize][HttpPost("{id:int}/enroll")]
    public async Task<IActionResult> Enroll(int id)
    {
        var uid = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
        var c   = await db.Courses.FindAsync(id);
        if (c is null || !c.IsPublished) return NotFound(new { message = "No encontrado" });
        if (await db.Enrollments.AnyAsync(e => e.UserId == uid && e.CourseId == id))
            return Conflict(new { message = "Ya matriculado" });
        db.Enrollments.Add(new Enrollment { UserId = uid, CourseId = id });
        c.EnrollmentCount++;
        await db.SaveChangesAsync();
        return Ok(new { message = "Matriculación exitosa" });
    }

    private static CourseDto ToDto(Course c) => new(c.Id, c.Title, c.Description, c.Instructor,
        c.Price, c.DurationHours, c.Level, c.ImageUrl, c.IsPublished, c.EnrollmentCount, c.Rating,
        c.CreatedAt, c.UpdatedAt, c.CategoryId, c.Category?.Name ?? "");
}

// ── DASHBOARD ─────────────────────────────────────────────────────────────────
[ApiController][Route("api/dashboard")]
[Authorize(Roles = "admin,instructor")]
public class DashboardController(AppDbContext db) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult> Get([FromQuery] DateTime? dateFrom, [FromQuery] DateTime? dateTo)
    {
        var from = dateFrom ?? DateTime.UtcNow.AddMonths(-12);
        var to   = dateTo   ?? DateTime.UtcNow;

        var kpis = new DashboardKpis(
            await db.Courses.CountAsync(),
            await db.Courses.CountAsync(c => c.IsPublished),
            await db.Categories.CountAsync(c => c.IsActive),
            await db.Users.CountAsync(u => u.IsActive),
            await db.Enrollments.CountAsync(),
            await db.Courses.Where(c => c.IsPublished).SumAsync(c => c.Price * c.EnrollmentCount),
            Math.Round(await db.Courses.Where(c => c.IsPublished && c.Rating > 0).AverageAsync(c => (double?)c.Rating) ?? 0, 2),
            Math.Round(await db.Courses.Where(c => c.IsPublished).AverageAsync(c => (double?)c.DurationHours) ?? 0, 1)
        );

        var byCategory = await db.Courses.Include(c => c.Category).Where(c => c.IsPublished)
            .GroupBy(c => c.Category.Name)
            .Select(g => new ChartPoint(g.Key, g.Sum(c => (double)c.EnrollmentCount))).ToListAsync();

        var byLevel = await db.Courses.Where(c => c.IsPublished)
            .GroupBy(c => c.Level)
            .Select(g => new ChartPoint(g.Key, g.Count())).ToListAsync();

        var byMonth = await db.Enrollments.Include(e => e.Course)
            .Where(e => e.EnrolledAt >= from && e.EnrolledAt <= to)
            .GroupBy(e => new { e.EnrolledAt.Year, e.EnrolledAt.Month })
            .Select(g => new { Label = g.Key.Year + "-" + g.Key.Month.ToString().PadLeft(2,'0'), Value = g.Sum(e => (double)e.Course.Price) })
            .OrderBy(x => x.Label).Select(x => new ChartPoint(x.Label, x.Value)).ToListAsync();

        var topRating = await db.Courses.Where(c => c.IsPublished && c.Rating > 0)
            .OrderByDescending(c => c.Rating).Take(5)
            .Select(c => new ChartPoint(c.Title, c.Rating)).ToListAsync();

        return Ok(new DashboardResponse(kpis, byCategory, byLevel, byMonth, topRating));
    }
}

// ── USERS ─────────────────────────────────────────────────────────────────────
[ApiController][Route("api/users")]
public class UsersController(AppDbContext db) : ControllerBase
{
    [Authorize(Roles = "admin")][HttpGet]
    public async Task<ActionResult> GetAll() =>
        Ok(await db.Users.Select(u => new { u.Id, u.Name, u.Email, u.Role, u.IsActive, u.CreatedAt, u.LastLoginAt,
            EnrollmentCount = u.Enrollments.Count }).ToListAsync());

    [Authorize(Roles = "admin")][HttpPost]
    public async Task<ActionResult> Create([FromBody] RegisterRequest req)
    {
        if (await db.Users.AnyAsync(u => u.Email == req.Email))
            return Conflict(new { message = "Email en uso" });
        var roles = new[] { "student", "instructor", "admin" };
        var user  = new User { Name=req.Name, Email=req.Email,
            PasswordHash=BCrypt.Net.BCrypt.HashPassword(req.Password),
            Role=roles.Contains(req.Role) ? req.Role : "student" };
        db.Users.Add(user); await db.SaveChangesAsync();
        return Created("", new { user.Id, user.Name, user.Email, user.Role });
    }

    [Authorize(Roles = "admin")][HttpPatch("{id:int}")]
    public async Task<ActionResult> Update(int id, [FromBody] UpdateUserRequest req)
    {
        var user = await db.Users.FindAsync(id);
        if (user is null) return NotFound(new { message = "No encontrado" });
        var roles = new[] { "student", "instructor", "admin" };
        if (!string.IsNullOrEmpty(req.Role) && roles.Contains(req.Role)) user.Role = req.Role;
        if (req.IsActive.HasValue) user.IsActive = req.IsActive.Value;
        await db.SaveChangesAsync();
        return Ok(new { user.Id, user.Name, user.Email, user.Role, user.IsActive });
    }

    [Authorize(Roles = "admin")][HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var user = await db.Users.FindAsync(id);
        if (user is null) return NotFound(new { message = "No encontrado" });
        db.Users.Remove(user); await db.SaveChangesAsync();
        return NoContent();
    }

    [Authorize][HttpGet("me/enrollments")]
    public async Task<ActionResult> MyEnrollments()
    {
        var uid = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
        return Ok(await db.Enrollments.Include(e => e.Course).Where(e => e.UserId == uid)
            .Select(e => new EnrollmentDto(e.CourseId, e.Course.Title, e.EnrolledAt, e.ProgressPercent, e.IsCompleted))
            .ToListAsync());
    }

    [Authorize][HttpPost("me/progress")]
    public async Task<IActionResult> UpdateProgress([FromBody] UpdateProgressRequest req)
    {
        var uid = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
        var e   = await db.Enrollments.FirstOrDefaultAsync(x => x.UserId == uid && x.CourseId == req.CourseId);
        if (e is null) return NotFound(new { message = "No matriculado" });
        e.ProgressPercent = Math.Clamp(req.ProgressPercent, 0, 100);
        if (e.ProgressPercent == 100 && !e.IsCompleted) { e.IsCompleted = true; e.CompletedAt = DateTime.UtcNow; }
        await db.SaveChangesAsync();
        return Ok(new { e.ProgressPercent, e.IsCompleted });
    }
}
