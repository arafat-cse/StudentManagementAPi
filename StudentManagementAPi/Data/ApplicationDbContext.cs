using Microsoft.EntityFrameworkCore;
using StudentManagementAPi.Models;

namespace StudentManagementAPi.Data
{
    public class ApplicationDbContext: DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
             : base(options)
        {
        }

        public DbSet<Student> Students { get; set; }
        public DbSet<Subject> Subjects { get; set; }
        public DbSet<StudentSubject> StudentSubjects { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure relationships
            modelBuilder.Entity<StudentSubject>()
                .HasOne(ss => ss.Student)
                .WithMany(s => s.StudentSubjects)
                .HasForeignKey(ss => ss.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<StudentSubject>()
                .HasOne(ss => ss.Subject)
                .WithMany(s => s.StudentSubjects)
                .HasForeignKey(ss => ss.SubjectId)
                .OnDelete(DeleteBehavior.Cascade);

            // Seed data for subjects
            modelBuilder.Entity<Subject>().HasData(
                new Subject { SubjectId = 1, SubjectName = "Mathematics" },
                new Subject { SubjectId = 2, SubjectName = "Physics" },
                new Subject { SubjectId = 3, SubjectName = "Chemistry" },
                new Subject { SubjectId = 4, SubjectName = "Biology" },
                new Subject { SubjectId = 5, SubjectName = "Computer Science" },
                new Subject { SubjectId = 6, SubjectName = "History" },
                new Subject { SubjectId = 7, SubjectName = "Geography" },
                new Subject { SubjectId = 8, SubjectName = "English" }
            );
        }
    }   
  
}
