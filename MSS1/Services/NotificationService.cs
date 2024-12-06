using MSS1.DTOs.RequestDTOs;
using MSS1.DTOs.ResponseDTOs;
using MSS1.Entities;
using MSS1.Interfaces;

namespace MSS1.Services
{
    public class NotificationService : INotificationService
    {
        private readonly INotificationRepository _notificationRepository;

        public NotificationService(INotificationRepository notificationRepository)
        {
            _notificationRepository = notificationRepository;
        }

        public async Task SendNotificationAsync(NotificationRequestDto request)
        {
            var notification = new Notification
            {
                UserId = request.StudentId,
                Message = request.Message,
                CreatedDate = DateTime.Now,
                IsRead = false,
                Type = request.Type
            };

            await _notificationRepository.AddNotificationAsync(notification);
        }


        public async Task<List<NotificationDto>> GetNotificationsForUserAsync(int userId)
        {
            var notifications = await _notificationRepository.GetNotificationsByUserIdAsync(userId);

            return notifications.Select(n => new NotificationDto
            {
                NotificationId = n.NotificationId,
                Message = n.Message,
                CreatedDate = n.CreatedDate,
                IsRead = n.IsRead,
                Type = n.Type
            }).ToList();
        }
    }

}
