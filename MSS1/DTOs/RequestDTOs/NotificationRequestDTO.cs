namespace MSS1.DTOs.RequestDTOs
{
    public class NotificationRequestDto
    {
        public int UserId { get; set; }
        public int StudentId { get; set; }
        public string Message { get; set; }
        public string Type { get; set; }
    }

}
