using System.ComponentModel.DataAnnotations;

namespace MSS1.Entities
{
    public class LecturerCourse
    {
        [Key]
        public int LecturerCourseId { get; set; } // Primary Key
        public int LecturerId { get; set; } // Foreign Key to Lecturer
        public string CourseName { get; set; } // Course Name

        // Navigation Property
        public Lecturer Lecturer { get; set; }
    }

}
