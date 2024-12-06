using MSS1.DTOs.RequestDTOs;
using MSS1.DTOs.ResponseDTOs;

namespace MSS1.Interfaces
{
    public interface INotificationService
    {
       // Task SendNotificationAsync(int userId, string message, string type);
        Task<List<NotificationDto>> GetNotificationsForUserAsync(int userId);
        Task SendNotificationAsync(NotificationRequestDto request);
    }

}
