using System.ComponentModel.DataAnnotations;

namespace MSS1.Entities
{
    public class Role
    {
        [Key]
        public int RoleId { get; set; }   // Primary Key
        public string RoleName { get; set; }   // Role Name (Admin, Student)

        // Navigation property
        public ICollection<User>? Users { get; set; }

    }
}
