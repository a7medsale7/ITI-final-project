using Microsoft.EntityFrameworkCore;
using Training_ITI.Models;
using Microsoft.EntityFrameworkCore;


namespace Training_ITI.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Course> Courses => Set<Course>();
        public DbSet<Session> Sessions => Set<Session>();
        public DbSet<User> Users => Set<User>();
        public DbSet<Grade> Grades => Set<Grade>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Unique index on Course.Name
            modelBuilder.Entity<Course>()
                .HasIndex(c => c.Name)
                .IsUnique();

            // Course ↔ Instructor(User)
            modelBuilder.Entity<Course>()
                .HasOne(c => c.Instructor)
                .WithMany(u => u.CoursesAsInstructor)
                .HasForeignKey(c => c.InstructorId)
                .OnDelete(DeleteBehavior.Restrict);

            // Session ↔ Course
            modelBuilder.Entity<Session>()
                .HasOne(s => s.Course)
                .WithMany(c => c.Sessions)
                .HasForeignKey(s => s.CourseId)
                .OnDelete(DeleteBehavior.Cascade);

            // Grade ↔ Session
            modelBuilder.Entity<Grade>()
                .HasOne(g => g.Session)
                .WithMany(s => s.Grades)
                .HasForeignKey(g => g.SessionId)
                .OnDelete(DeleteBehavior.Cascade);

            // Grade ↔ Trainee(User)
            modelBuilder.Entity<Grade>()
                .HasOne(g => g.Trainee)
                .WithMany(u => u.Grades)
                .HasForeignKey(g => g.TraineeId)
                .OnDelete(DeleteBehavior.Restrict);

            // Seed basic users
            modelBuilder.Entity<User>().HasData(
                new User { Id = 1, Name = "Admin One", Email = "admin@site.com", Role = UserRole.Admin },
                new User { Id = 2, Name = "Inst. Sara", Email = "sara@site.com", Role = UserRole.Instructor },
                new User { Id = 3, Name = "Inst. Omar", Email = "omar@site.com", Role = UserRole.Instructor },
                new User { Id = 4, Name = "Trainee Ali", Email = "ali@site.com", Role = UserRole.Trainee },
                new User { Id = 5, Name = "Trainee Mona", Email = "mona@site.com", Role = UserRole.Trainee }
            );
        }
    }
}
