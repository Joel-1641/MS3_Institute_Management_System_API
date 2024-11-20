namespace MSS1.DTOs.ResponseDTOs
{
    public class UserResponseDTO
    {
        public int UserId { get; set; }
        public string FullName { get; set; }
        public string RoleName { get; set; }
        //public string AdminRoleType { get; set; }  // Only for Admins
        public string ProfilePicture { get; set; } // Only for Students
    }
}
