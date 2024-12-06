using System.ComponentModel.DataAnnotations;

namespace MSS1.Entities
{
    public class Notification
    {
        [Key]
        public int NotificationId { get; set; } // Primary Key

        [Required]
        public int UserId { get; set; } // Foreign Key to User (sender or recipient)

        [Required]
        public string Message { get; set; } // Notification message content

        [Required]
        public DateTime CreatedDate { get; set; } // Date the notification was created

        public bool IsRead { get; set; } // Whether the notification has been read by the user

        // Notification Type (e.g., Payment Due, Course Update, General)
        public string Type { get; set; } // Payment, Alert, Reminder, etc.

        // Navigation Property
        public User User { get; set; } // The user who receives the notification
    }
}
