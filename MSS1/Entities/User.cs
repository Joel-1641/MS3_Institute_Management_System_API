using System.ComponentModel.DataAnnotations;

namespace MSS1.Entities
{
    public class User
    {
        [Key]
        public int UserId { get; set; }  // Primary Key
        public string FullName { get; set; }
        public int RoleId { get; set; }   // Foreign Key to Role

        // Navigation properties
        public Role Role { get; set; }
        public Authentication Authentication { get; set; }  // One-to-one relationship with Authentication
        public Admin Admin { get; set; }    // One-to-one relationship with Admin (only for Admins)
        public Student Student { get; set; }  // One-to-one relationship with Student (only for Students)

    }
}
