namespace MSS1.Entities
{
    public class Notification
    {
       
            public int NotificationId { get; set; }
            public int StudentId { get; set; }
            public string Message { get; set; }
            public bool IsRead { get; set; }
            public DateTime CreatedAt { get; set; }
            public string NotificationType { get; set; }  // "Enrollment", "Payment"
            public string Target { get; set; }  

        public Student  Student { get; set; }
        
       
        

    }
}
