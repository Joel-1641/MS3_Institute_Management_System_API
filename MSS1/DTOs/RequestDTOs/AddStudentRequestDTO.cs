namespace MSS1.DTOs.RequestDTOs
{
    public class AddStudentRequestDTO: AddUserRequestDTO
    {
        public decimal RegistrationFee { get; set; } // Fee for registration
        public bool IsRegistrationFeePaid { get; set; } // Flag indicating if fee is paid
    }
}
