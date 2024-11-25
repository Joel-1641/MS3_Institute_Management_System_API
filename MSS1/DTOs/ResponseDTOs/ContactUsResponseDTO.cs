namespace MSS1.DTOs.ResponseDTOs
{
    public class ContactUsResponseDTO
    {
        public int ContactId { get; set; }
        public string FullName { get; set; }
        public string EmailAddress { get; set; }
        public string Message { get; set; }
        public DateTime SubmittedOn { get; set; }
    }
}
