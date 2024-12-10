namespace MSS1.DTOs.ResponseDTOs
{
    public class PaymentStatusResponseDTO
    {
        public string NIC { get; set; }
        public string StudentName { get; set; }
        public decimal TotalFee { get; set; }
        public decimal TotalPaid { get; set; }
        public decimal DueAmount { get; set; }
        public string PaymentStatus { get; set; }
    }
}
