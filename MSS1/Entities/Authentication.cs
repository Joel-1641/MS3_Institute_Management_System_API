using System.ComponentModel.DataAnnotations;

namespace MSS1.Entities
{
    public class Authentication
    {
        [Key]
        public int AuthenticationId { get; set; }   // Primary Key
        public int UserId { get; set; }              // Foreign Key to User
        public string Email { get; set; }            // Email address (Unique)
        public string HashedPassword { get; set; }   // Hashed password
        public string PasswordSalt { get; set; }     // Salt used for password hashing

        // Navigation property
        public User User { get; set; }

    }
}
