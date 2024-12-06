namespace MSS1.DTOs.RequestDTOs
{
    public class UpdateStudentRequestDTO
    {
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public long MobileNumber { get; set; }
        public string Gender { get; set; }
        public decimal RegistrationFee { get; set; }
        public bool IsRegistrationFeePaid { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string NICNumber { get; set; }
    }
}
