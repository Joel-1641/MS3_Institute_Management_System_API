namespace MSS1.DTOs.ResponseDTOs
{
    public class NotificationResponseDTO
    {
        public int NotificationId { get; set; } // The ID of the notification
        public string Message { get; set; } // The content/message of the notification
        public bool IsRead { get; set; } // Whether the notification has been read by the recipient
        public DateTime CreatedAt { get; set; } // When the notification was created
        public string NotificationType { get; set; } // Type of notification: "Payment", "Enrollment", etc.
        public string Target { get; set; } // Target recipient of the notification: "Admin" or "Student"
        public string StudentName { get; set; } // The name of the student if it's a student-targeted notification
        public string StudentNIC { get; set; } // The NIC of the student (if the notification is related to a student)
    }
}
