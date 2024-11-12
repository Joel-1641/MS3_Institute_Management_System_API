using MSS1.Entities;

namespace MSS1
{
    public class Course
    {
        public int CourseId { get; set; }  // Primary Key
        public string CourseName { get; set; }
        public string Level { get; set; }  // E.g., Beginner, Intermediate, Advanced
        public decimal CourseFee { get; set; }
        public string Description { get; set; }

        // Navigation property
        public ICollection<Student>? Students { get; set; }  // Many-to-many relationship with Students

    }
}
