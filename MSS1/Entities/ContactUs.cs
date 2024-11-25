using System.ComponentModel.DataAnnotations;

namespace MSS1.Entities
{
    public class ContactUs
    {
        [Key]
        public int ContactId { get; set; } // Primary Key

        [Required]
        [MaxLength(100)]
        public string FullName { get; set; } // Full Name of the user

        [Required]
        [EmailAddress]
        public string EmailAddress { get; set; } // Email Address

        [Required]
        [MaxLength(500)]
        public string Message { get; set; } // Message content

        public DateTime SubmittedOn { get; set; } = DateTime.UtcNow;
    }
}
