using System.ComponentModel.DataAnnotations;

namespace MSS1.Entities
{
    public class Student
    {
        [Key]
        public int StudentId { get; set; }  // Primary Key
        public int UserId { get; set; }     // Foreign Key to User
        public decimal RegistrationFee { get; set; }  // Registration fee
        public string ProfilePicture { get; set; }    // Profile picture (URL or base64 string)

        // Navigation property
        public User? User { get; set; }
        public ICollection<Course>? Courses { get; set; }  // Many-to-many relationship with Courses
        public ICollection<Payment>? Payments { get; set; }  // One-to-many relationship with Payments

    }
}
