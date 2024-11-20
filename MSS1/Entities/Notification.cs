using System.ComponentModel.DataAnnotations;

namespace MSS1.Entities
{
    public class Notification
    {
        [Key]
        public int NotificationId { get; set; }  // Primary Key
        public int UserId { get; set; }          // Foreign Key to User
        public string Title { get; set; }
        public string Message { get; set; }
        public DateTime DateCreated { get; set; }

        // Navigation property
        public User User { get; set; }

    }
}
