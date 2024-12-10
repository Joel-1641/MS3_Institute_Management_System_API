namespace MSS1.DTOs.ResponseDTOs
{
    public class CumulativePaymentStatusDTO
    {
        public decimal CumulativeTotalFee { get; set; }
        public decimal CumulativeTotalPaid { get; set; }
        public decimal CumulativePaymentDue { get; set; }
    }
}
