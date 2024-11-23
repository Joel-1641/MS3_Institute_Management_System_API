namespace MSS1.DTOs.RequestDTOs
{
    public class AddPaymentRequestDTO
    {
        public int StudentId { get; set; }
        public decimal Amount { get; set; }
        public string PaymentMethod { get; set; }
        public bool IsForRegistrationFee { get; set; }
    }
}
