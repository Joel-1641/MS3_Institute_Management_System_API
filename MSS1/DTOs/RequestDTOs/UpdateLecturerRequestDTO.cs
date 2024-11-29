namespace MSS1.DTOs.RequestDTOs
{
    public class UpdateLecturerRequestDTO
    {
        public string FullName { get; set; }
        public string Email { get; set; }
        public string NICNumber { get; set; }
        public string Gender { get; set; }
        public string Address { get; set; }
        public int MobileNumber { get; set; }
        public string ProfilePicture { get; set; }
        public List<string> Courses { get; set; }
    }
}
