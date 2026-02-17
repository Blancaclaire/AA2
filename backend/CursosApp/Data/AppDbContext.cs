using Microsoft.EntityFrameworkCore;
using CursosApp.Models;

namespace CursosApp.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<User>       Users       => Set<User>();
    public DbSet<Category>   Categories  => Set<Category>();
    public DbSet<Course>     Courses     => Set<Course>();
    public DbSet<Enrollment> Enrollments => Set<Enrollment>();

    protected override void OnModelCreating(ModelBuilder m)
    {
        // Map to the exact table/column names created by init.sql
        m.Entity<Category>(e => {
            e.ToTable("Categories");
            e.HasKey(x => x.Id);
        });

        m.Entity<User>(e => {
            e.ToTable("Users");
            e.HasKey(x => x.Id);
            e.HasIndex(x => x.Email).IsUnique();
        });

        m.Entity<Course>(e => {
            e.ToTable("Courses");
            e.HasKey(x => x.Id);
            e.Property(x => x.Price).HasPrecision(10, 2);
            e.HasOne(x => x.Category)
             .WithMany(x => x.Courses)
             .HasForeignKey(x => x.CategoryId)
             .OnDelete(DeleteBehavior.Restrict);
        });

        m.Entity<Enrollment>(e => {
            e.ToTable("Enrollments");
            e.HasKey(x => x.Id);
            e.HasIndex(x => new { x.UserId, x.CourseId }).IsUnique();
            e.HasOne(x => x.User).WithMany(x => x.Enrollments).HasForeignKey(x => x.UserId);
            e.HasOne(x => x.Course).WithMany(x => x.Enrollments).HasForeignKey(x => x.CourseId);
        });
    }
}
