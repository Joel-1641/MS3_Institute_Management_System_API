namespace MSS1.DTOs.ResponseDTOs
{
    public class PaymentDetailsDTO
    {
        public decimal TotalFee { get; set; }
        public decimal TotalPaid { get; set; }
        public decimal PaymentDue { get; set; }
        public string PaymentStatus { get; set; }
    }
}
