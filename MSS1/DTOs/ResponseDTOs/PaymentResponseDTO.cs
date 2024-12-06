namespace MSS1.DTOs.ResponseDTOs
{
    public class PaymentResponseDto
    {
        public int PaymentId { get; set; }
        public int StudentId { get; set; }
        public int CourseId { get; set; }
        public decimal RegistrationFee { get; set; }
        public DateTime PaymentDate { get; set; }
    }

}
