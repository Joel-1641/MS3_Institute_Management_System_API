namespace MSS1.DTOs.ResponseDTOs
{
    public class UserResponseDTO
    {
        public int UserId { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string RoleName { get; set; }
        public string ProfilePicture { get; set; } // Profile Picture URL
        public string NICNumber { get; set; } // National Identity Card Number
        public string Gender { get; set; } // Male, Female, Other
        public string Address { get; set; } // Address
        public long MobileNumber { get; set; } // Mobile Number
    }

    
}
