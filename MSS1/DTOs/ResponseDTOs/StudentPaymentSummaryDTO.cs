namespace MSS1.DTOs.ResponseDTOs
{
    public class StudentPaymentSummaryDTO
    {
        public int StudentId { get; set; }
        public string FullName { get; set; }
        public decimal TotalFee { get; set; }
        public decimal TotalPaid { get; set; }
        public decimal PaymentDue { get; set; }
        public string PaymentStatus { get; set; }
    }
}
