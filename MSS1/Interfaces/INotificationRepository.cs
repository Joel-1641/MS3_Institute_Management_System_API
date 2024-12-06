using MSS1.Entities;

namespace MSS1.Interfaces
{
    public interface INotificationRepository
    {
        Task AddNotificationAsync(Notification notification);
        Task<List<Notification>> GetNotificationsByUserIdAsync(int userId);
        Task MarkAsReadAsync(int notificationId);
    }

}
