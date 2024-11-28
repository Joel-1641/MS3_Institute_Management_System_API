namespace MSS1.DTOs.RequestDTOs
{
    public class AddUserRequestDTO
    {
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string NICNumber { get; set; }
        public int RoleId { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Gender { get; set; }
        public string Address { get; set; }
        public int MobileNumber { get; set; }
        public string ProfilePicture { get; set; } // Optional: Profile Picture
    }
}
