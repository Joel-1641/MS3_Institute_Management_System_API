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

            modelBuilder.Entity<Payment>()
                .Property(p => p.AmountPaid)
                .HasPrecision(18, 2); // Precision 18, scale 2 (example)

            modelBuilder.Entity<Student>()
                .Property(s => s.RegistrationFee)
                .HasPrecision(18, 2); // Precision 18, scale 2 (example)

            // Configure many-to-many relationship
            modelBuilder.Entity<Student>()
                .HasMany(s => s.Courses)
                .WithMany(c => c.Students)
                .UsingEntity<StudentCourse>(
                    j => j.HasOne(sc => sc.Course).WithMany().HasForeignKey(sc => sc.CourseId),
                    j => j.HasOne(sc => sc.Student).WithMany().HasForeignKey(sc => sc.StudentId),
                    j => j.HasKey(sc => new { sc.StudentId, sc.CourseId })
                );
            modelBuilder.Entity<Role>().HasData(
     new Role { RoleId = 1, RoleName = "Admin" },
     new Role { RoleId = 2, RoleName = "Student" }
 );

            modelBuilder.Entity<Authentication>()
                .HasOne(a => a.User)
                .WithOne(u => u.Authentication)
                .HasForeignKey<Authentication>(a => a.UserId);
               }

       


    }

}

