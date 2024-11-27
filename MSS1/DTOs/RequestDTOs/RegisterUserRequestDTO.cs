using System.ComponentModel.DataAnnotations;

namespace MSS1.DTOs.RequestDTOs
{
    public class RegisterUserRequestDTO
        {
        [Required(ErrorMessage = "Email is required.")]
        [RegularExpression(@"^[^@\s]+@[^@\s]+\.[^@\s]+$", ErrorMessage = "Invalid email format.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password is required.")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Full Name is required.")]
        public string FullName { get; set; }

        [Required(ErrorMessage = "Role ID is required.")]
        public int RoleId { get; set; }
    }
    }