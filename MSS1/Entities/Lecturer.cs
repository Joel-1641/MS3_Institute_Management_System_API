using System.ComponentModel.DataAnnotations;

namespace MSS1.Entities
{
    using System.ComponentModel.DataAnnotations;

    public class Lecturer
    {
        [Key]
        public int LecturerId { get; set; } // Primary Key
        public int UserId { get; set; } // Foreign Key to User

        // Navigation Properties
        public User User { get; set; }
        public ICollection<LecturerCourse> Courses { get; set; } // Courses they can teach
    }


}
