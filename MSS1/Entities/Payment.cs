using System.ComponentModel.DataAnnotations;

namespace MSS1.Entities
{
    public class Payment
    {
        [Key]
        public int PaymentId { get; set; }

        public int StudentId { get; set; }  // Foreign Key to Student
        public int CourseId { get; set; }  // Foreign Key to Course

        [Required]
        public decimal AmountPaid { get; set; }  // Amount paid in a single transaction
        public decimal TotalFee { get; set; }    // Total fee for the course
        public decimal DueAmount { get; set; }   // Remaining amount to be paid
        public DateTime PaymentDate { get; set; } // Date of the payment
        public string PaymentMethod { get; set; } // Full Payment or Installment
        public string PaymentStatus { get; set; } // Pending, Success, or Overdue

        // Navigation Properties
        public Student Student { get; set; }
        public Course Course { get; set; }
    }

}
