namespace MSS1.DTOs.ResponseDTOs
{
    public class UserResponseDTO
    {
        public int UserId { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string RoleName { get; set; }
        public string ProfilePicture { get; set; } // Include Profile Picture in the response
        public object AdditionalData { get; set; } // Include Additional Data based on role (Student or Lecturer)
    }
}
