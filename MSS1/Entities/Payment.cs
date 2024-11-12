using System.ComponentModel.DataAnnotations;

namespace MSS1.Entities
{
    public class Payment
    {
        [Key]
        public int PaymentId { get; set; }  // Primary Key
        public int StudentId { get; set; }  // Foreign Key to Student
        public decimal AmountPaid { get; set; }
        public string PaymentMethod { get; set; }  // e.g., Installments or Full Payment
        public string PaymentStatus { get; set; }  // e.g., Pending, Completed, Failed
        public DateTime PaymentDate { get; set; }

        // Navigation property
        public Student? Student { get; set; }

    }
}
