using System.ComponentModel.DataAnnotations;

namespace MSS1.Entities
{
    public class Course
    {
        [Key]
        public int CourseId { get; set; } // Primary Key

        [Required]
        public string CourseName { get; set; }

        [Required]
        public string Level { get; set; } // Beginner, Intermediate, Advanced

        [Required]
        public decimal CourseFee { get; set; }

        
        public string Description { get; set; }

        // Navigation Properties
        public ICollection<Student> Students { get; set; }
        public ICollection<StudentCourse> StudentCourses { get; set; }
    }

}
