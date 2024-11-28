using System.ComponentModel.DataAnnotations;

namespace MSS1.Entities
{
    using System.ComponentModel.DataAnnotations;

    public class User
    {
        [Key]
        public int UserId { get; set; } // Primary Key
        public string FullName { get; set; }
        public string Email { get; set; } // Unique Email Address
        public string NICNumber { get; set; } // Unique NIC Number
        public string PasswordHash { get; set; } // Password hash
        public string PasswordSalt { get; set; } // Password salt
        public DateTime DateOfBirth { get; set; } // Date of Birth
        public string Gender { get; set; } // Male, Female, Other
        public string Address { get; set; } // Address
        public int MobileNumber { get; set; } // Mobile Number
        public int RoleId { get; set; } // Foreign Key to Role
        public string ProfilePicture { get; set; } // Profile picture URL or file path

        // Navigation Properties
        public Role Role { get; set; }
        public Student Student { get; set; }
        public Lecturer Lecturer { get; set; }

        // Navigation Property to Authentication entity
        public Authentication Authentication { get; set; } // One-to-One relationship with Authentication
    }



}
