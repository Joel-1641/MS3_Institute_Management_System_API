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
        public string Level { get; set; } // Beginner, Intermediate

        [Required]
        public decimal CourseFee { get; set; }
        public string CourseDuration { get; set; }
        
        public string Description { get; set; }
        public string CourseImg {  get; set; }

        public DateTime CourseStartDate { get; set; }= DateTime.Now;
        public DateTime CourseEndDate { get; set; }
        // Navigation Properties
        public ICollection<Student> Students { get; set; }
        public ICollection<StudentCourse> StudentCourses { get; set; }
    }

}
