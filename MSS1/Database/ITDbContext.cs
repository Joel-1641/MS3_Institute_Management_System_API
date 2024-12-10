using Microsoft.EntityFrameworkCore;
using MSS1.Entities;
using System.Configuration;

namespace MSS1.Database
{
    public class ITDbContext:DbContext
    {
        private readonly IConfiguration _configuration;
        public ITDbContext(DbContextOptions<ITDbContext>options, IConfiguration configuration) :base(options)
        {
            _configuration = configuration;

        }
        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Authentication> Authentications { get; set; }
        public DbSet<Admin> Admins { get; set; }
        public DbSet<Student> Students { get; set; }
        public DbSet<Course> Courses { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<Notification> Notifications { get; set; }
        public DbSet<ContactUs> ContactUs { get; set; }
        public DbSet<Lecturer> Lecturers { get; set; }
        public DbSet<LecturerCourse> LecturerCourses { get; set; }
        public DbSet<StudentCourse> StudentCourses { get; set; }
        //public DbSet<Report> Reports { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var connectionString = _configuration.GetConnectionString("ITDBConnection");
            optionsBuilder.UseSqlServer(connectionString);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Set precision and scale for decimal properties
            modelBuilder.Entity<Course>()
                .Property(c => c.CourseFee)
                .HasPrecision(18, 2); // Precision 18, scale 2 (example)

            //modelBuilder.Entity<Payment>()
            //    .Property(p => p.AmountPaid)
            //    .HasPrecision(18, 2); // Precision 18, scale 2 (example)

            modelBuilder.Entity<Student>()
                .Property(s => s.RegistrationFee)
                .HasPrecision(18, 2); // Precision 18, scale 2 (example)

            // Configure many-to-many relationship
            //modelBuilder.Entity<Student>()
            //    .HasMany(s => s.Courses)
            //    .WithMany(c => c.Students)
            //    .UsingEntity<StudentCourse>(
            //        j => j.HasOne(sc => sc.Course).WithMany().HasForeignKey(sc => sc.CourseId),
            //        j => j.HasOne(sc => sc.Student).WithMany().HasForeignKey(sc => sc.StudentId),
            //        j => j.HasKey(sc => new { sc.StudentId, sc.CourseId })
            //    );
            modelBuilder.Entity<Role>().HasData(
        new Role { RoleId = 1, RoleName = "Admin" },
        new Role { RoleId = 2, RoleName = "Student" },
        new Role { RoleId = 3, RoleName = "Lecturer" }
    );

            modelBuilder.Entity<Authentication>()
                .HasOne(a => a.User)
                .WithOne(u => u.Authentication)
                .HasForeignKey<Authentication>(a => a.UserId);
            // Unique NICNumber for Users
            // Unique constraints
            modelBuilder.Entity<User>().HasIndex(u => u.Email).IsUnique();
            modelBuilder.Entity<User>().HasIndex(u => u.NICNumber).IsUnique();

            modelBuilder.Entity<Lecturer>()
              .HasOne(l => l.User) // A Lecturer has one User
              .WithOne(u => u.Lecturer) // A User has one Lecturer
              .HasForeignKey<Lecturer>(l => l.UserId) // Foreign key is UserId in Lecturer
              .OnDelete(DeleteBehavior.Cascade); // Enable cascade deletion

            modelBuilder.Entity<StudentCourse>()
            .HasKey(sc => new { sc.StudentId, sc.CourseId });

            // Configure relationships
            modelBuilder.Entity<StudentCourse>()
                .HasOne(sc => sc.Student)
                .WithMany(s => s.StudentCourses)
                .HasForeignKey(sc => sc.StudentId);

            modelBuilder.Entity<StudentCourse>()
                .HasOne(sc => sc.Course)
                .WithMany(c => c.StudentCourses)
                .HasForeignKey(sc => sc.CourseId);

            modelBuilder.Entity<Payment>().HasKey(p => p.PaymentId);
            modelBuilder.Entity<Payment>()
                .HasOne(p => p.Student)
                .WithMany(s => s.Payments) // Add this navigation property to `Student`
                .HasForeignKey(p => p.StudentId);

        }






    }

}

