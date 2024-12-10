using System.ComponentModel.DataAnnotations;

namespace MSS1.DTOs.RequestDTOs
{
    public class RecordPaymentRequestDTO
    {
        [Required]
        public string NIC { get; set; }

        [Required]
        [Range(1, double.MaxValue, ErrorMessage = "Amount must be greater than 0.")]
        public decimal Amount { get; set; }
    }
}
