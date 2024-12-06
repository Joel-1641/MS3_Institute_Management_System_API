using Microsoft.EntityFrameworkCore;
using MSS1.Database;
using MSS1.Entities;
using MSS1.Interfaces;

namespace MSS1.Repository
{
    public class NotificationRepository : INotificationRepository
    {
        private readonly ITDbContext _context;

        public NotificationRepository(ITDbContext context)
        {
            _context = context;
        }

        public async Task AddNotificationAsync(Notification notification)
        {
            await _context.Notifications.AddAsync(notification);
            await _context.SaveChangesAsync();
        }

        public async Task<List<Notification>> GetNotificationsByUserIdAsync(int userId)
        {
            return _context.Notifications
                .Where(n => n.UserId == userId)
                .OrderByDescending(n => n.CreatedDate)
                .ToList();
        }



        public async Task MarkAsReadAsync(int notificationId)
        {
            var notification = await _context.Notifications.FindAsync(notificationId);
            if (notification != null)
            {
                notification.IsRead = true;
                _context.Notifications.Update(notification);
                await _context.SaveChangesAsync();
            }
        }
    }

}
