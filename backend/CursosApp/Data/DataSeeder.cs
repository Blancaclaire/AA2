using CursosApp.Models;
using Microsoft.EntityFrameworkCore;

namespace CursosApp.Data;

public static class DataSeeder
{
    public static async Task SeedUsersAsync(AppDbContext db)
    {
        if (await db.Users.AnyAsync()) return;

        var hash = BCrypt.Net.BCrypt.HashPassword("Admin1234!");
        var now  = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc);

        db.Users.AddRange(
            new User { Name = "Admin Principal", Email = "admin@cursosapp.com",      PasswordHash = hash, Role = "admin",      IsActive = true, CreatedAt = now },
            new User { Name = "Instructor Demo", Email = "instructor@cursosapp.com", PasswordHash = hash, Role = "instructor", IsActive = true, CreatedAt = now },
            new User { Name = "Alumno Demo",     Email = "student@cursosapp.com",    PasswordHash = hash, Role = "student",    IsActive = true, CreatedAt = now }
        );
        await db.SaveChangesAsync();

        // Seed enrollments after users exist
        var student = await db.Users.FirstAsync(u => u.Role == "student");
        db.Enrollments.AddRange(
            new Enrollment { UserId = student.Id, CourseId = 1, EnrolledAt = new DateTime(2024,3,1,0,0,0,DateTimeKind.Utc),  ProgressPercent = 75,  IsCompleted = false },
            new Enrollment { UserId = student.Id, CourseId = 4, EnrolledAt = new DateTime(2024,4,10,0,0,0,DateTimeKind.Utc), ProgressPercent = 100, IsCompleted = true, CompletedAt = new DateTime(2024,5,15,0,0,0,DateTimeKind.Utc) },
            new Enrollment { UserId = student.Id, CourseId = 9, EnrolledAt = new DateTime(2024,9,15,0,0,0,DateTimeKind.Utc), ProgressPercent = 30,  IsCompleted = false }
        );
        await db.SaveChangesAsync();
    }
}
