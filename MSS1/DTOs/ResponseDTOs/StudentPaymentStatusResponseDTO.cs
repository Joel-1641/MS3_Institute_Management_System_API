﻿namespace MSS1.DTOs.ResponseDTOs
{
    public class StudentPaymentStatusResponseDTO
    {
        public int StudentId { get; set; }
        public string StudentName { get; set; }
        public decimal TotalFee { get; set; }
        public decimal TotalPaid { get; set; }
        public decimal PaymentDue { get; set; }
        public string PaymentStatus { get; set; }
        public string NIC { get; set; }
    }
}
