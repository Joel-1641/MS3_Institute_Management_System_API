using System.ComponentModel.DataAnnotations;

namespace MSS1.Entities
{
    public class Payment
    {
        [Key]
        public int PaymentId { get; set; }
        public int StudentId { get; set; }
        public decimal AmountPaid { get; set; }
        public DateTime PaymentDate { get; set; } = DateTime.Now;

        // Navigation property
        public Student Student { get; set; }
    }
}
