namespace MSS1.DTOs.RequestDTOs
{
    public class PaymentDTO
    {
        public int PaymentId { get; set; }
        public decimal AmountPaid { get; set; }
        public string PaymentMethod { get; set; }
        public DateTime PaymentDate { get; set; }
        public string PaymentStatus { get; set; }
    }
}
