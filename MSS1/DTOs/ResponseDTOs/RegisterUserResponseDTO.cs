namespace MSS1.DTOs.ResponseDTOs
{
    public class RegisterUserResponseDTO
    {
        public int UserId { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string RoleName { get; set; }
        public string Message { get; set; }
    }
}
