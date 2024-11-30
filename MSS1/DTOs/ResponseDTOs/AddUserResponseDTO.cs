namespace MSS1.DTOs.ResponseDTOs
{
    public class AddUserResponseDTO
    {
        public int UserId { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string RoleName { get; set; }
        public string ProfilePicture { get; set; }

        public DateTime DateOfBirth { get; set; } // Date of Birth
    }


}
