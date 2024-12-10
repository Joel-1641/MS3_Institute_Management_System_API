using System.ComponentModel.DataAnnotations;

namespace MSS1.Entities
{
    using System.ComponentModel.DataAnnotations;

    public class Student
    {
        [Key]
        public int StudentId { get; set; } // Primary Key
        public int UserId { get; set; } // Foreign Key to User
        public decimal RegistrationFee { get; set; }
        public bool IsRegistrationFeePaid { get; set; } // Registration Fee Status

        // Navigation Properties
        public User User { get; set; }

        public ICollection<Payment> Payments { get; set; }
        
       // public ICollection<Course> Courses { get; set; }
        public ICollection<StudentCourse> StudentCourses { get; set; }
    }


}
