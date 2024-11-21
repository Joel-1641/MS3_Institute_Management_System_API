namespace MSS1.DTOs.RequestDTOs
{
    public class AddUserRequestDTO
    {
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public int RoleId { get; set; }
    }
}
