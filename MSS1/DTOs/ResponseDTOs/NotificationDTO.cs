﻿namespace MSS1.DTOs.ResponseDTOs
{
    public class NotificationDto
    {
        public int NotificationId { get; set; }
        public string Message { get; set; }
        public DateTime CreatedDate { get; set; }
        public bool IsRead { get; set; }
        public string Type { get; set; }
    }

}
