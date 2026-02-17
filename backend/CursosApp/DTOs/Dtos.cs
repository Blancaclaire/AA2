namespace CursosApp.DTOs;

public record LoginRequest(string Email, string Password);
public record RegisterRequest(string Name, string Email, string Password, string Role = "student");
public record AuthResponse(string Token, string Role, string Name, string Email, int UserId);

public record CategoryDto(int Id, string Name, string Description, string Icon, bool IsActive, DateTime CreatedAt, int CourseCount);
public record CreateCategoryRequest(string Name, string Description, string Icon);
public record UpdateCategoryRequest(string Name, string Description, string Icon, bool IsActive);

public record CourseDto(int Id, string Title, string Description, string Instructor,
    decimal Price, int DurationHours, string Level, string ImageUrl,
    bool IsPublished, int EnrollmentCount, double Rating,
    DateTime CreatedAt, DateTime UpdatedAt, int CategoryId, string CategoryName);

public record CourseDetailDto(int Id, string Title, string Description, string Instructor,
    decimal Price, int DurationHours, string Level, string ImageUrl,
    bool IsPublished, int EnrollmentCount, double Rating,
    DateTime CreatedAt, DateTime UpdatedAt, int CategoryId, string CategoryName,
    bool IsEnrolled, int? UserProgress);

public record CreateCourseRequest(string Title, string Description, string Instructor,
    decimal Price, int DurationHours, string Level, string ImageUrl, int CategoryId);

public record UpdateCourseRequest(string Title, string Description, string Instructor,
    decimal Price, int DurationHours, string Level, string ImageUrl,
    bool IsPublished, int CategoryId);

public record PagedResult<T>(IEnumerable<T> Items, int TotalCount, int Page, int PageSize, int TotalPages);

public class CourseSearchParams
{
    public string? Query { get; set; }
    public int? CategoryId { get; set; }
    public string? Level { get; set; }
    public decimal? MinPrice { get; set; }
    public decimal? MaxPrice { get; set; }
    public DateTime? DateFrom { get; set; }
    public DateTime? DateTo { get; set; }
    public bool? IsPublished { get; set; }
    public string SortBy { get; set; } = "createdAt";
    public string SortOrder { get; set; } = "desc";
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 10;
}

public record DashboardKpis(int TotalCourses, int PublishedCourses, int TotalCategories,
    int TotalUsers, int TotalEnrollments, decimal TotalRevenue, double AverageRating, double AverageDurationHours);
public record ChartPoint(string Label, double Value);
public record DashboardResponse(DashboardKpis Kpis,
    IEnumerable<ChartPoint> EnrollmentsByCategory, IEnumerable<ChartPoint> CoursesByLevel,
    IEnumerable<ChartPoint> RevenueByMonth, IEnumerable<ChartPoint> TopCoursesByRating);

public record EnrollmentDto(int CourseId, string CourseTitle, DateTime EnrolledAt, int ProgressPercent, bool IsCompleted);
public record UpdateProgressRequest(int CourseId, int ProgressPercent);
public record UpdateUserRequest(string? Role, bool? IsActive);
