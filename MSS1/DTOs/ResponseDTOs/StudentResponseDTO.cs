namespace MSS1.DTOs.ResponseDTOs
{
    public class StudentResponseDTO: UserResponseDTO
    {
        public decimal RegistrationFee { get; set; }
        public bool IsRegistrationFeePaid { get; set; }
    }
}
