namespace MSS1.DTOs.ResponseDTOs
{
    public class AllStudentsPaymentSummaryDTO
    {
        public decimal TotalFees { get; set; }
        public decimal TotalPaid { get; set; }
        public decimal TotalDue { get; set; }
    }
}
