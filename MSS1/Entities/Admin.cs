using System.ComponentModel.DataAnnotations;

namespace MSS1.Entities
{
    public class Admin
    {
        [Key]
        public int AdminId { get; set; }  // Primary Key
        public int UserId { get; set; }   // Foreign Key to User
        public string AdminRoleType { get; set; } // E.g., SuperAdmin, CourseAdmin
       // public string Department { get; set; }    // Department of the Admin
        public string AdminPhoneNumber { get; set; }
        public DateTime LastLoginDate { get; set; }
       // public bool HasFinancialAccess { get; set; } // Can manage financials
       // public bool HasCourseManagementAccess { get; set; } // Can manage courses

        // Navigation property
        public User User { get; set; }

    }
}
