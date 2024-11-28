using System.ComponentModel.DataAnnotations;

namespace MSS1.Entities
{
    public class Course
    {
        [Key]
        public int CourseId { get; set; } // Primary Key
        public string CourseName { get; set; }
        public string Level { get; set; } // Beginner, Intermediate, Advanced
        public decimal CourseFee { get; set; }
        public string Description { get; set; }

        // Navigation Properties
        public ICollection<Student> Students { get; set; }
    }

}
