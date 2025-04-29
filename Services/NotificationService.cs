using ProjectPRN222.Models;
using Microsoft.EntityFrameworkCore;
namespace ProjectPRN222.Services
{
    public class NotificationService : INotificationService
    {
        private readonly Prn222projectContext _context;

        public NotificationService(Prn222projectContext context)
        {
            _context = context;
        }

        public async Task CreateNotification(Notification notification)
        {
            notification.CreateAt = DateTime.Now;
            _context.Notifications.Add(notification);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateNotification(Notification notification)
        {
            var existingNotification = await _context.Notifications.FindAsync(notification.Id);
            if (existingNotification != null)
            {
                existingNotification.Title = notification.Title;
                existingNotification.SendTo = notification.SendTo;
                await _context.SaveChangesAsync();
            }
        }

        public async Task DeleteNotification(int notificationId)
        {
            var notification = await _context.Notifications.FindAsync(notificationId);
            if (notification != null)
            {
                _context.Notifications.Remove(notification);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<Notification> GetNotificationByIdAsync(int id)  
        {
            return await _context.Notifications.FindAsync(id);
        }

        public async Task<List<Notification>> GetAllNotificationsAsync()  
        {
            return await _context.Notifications.ToListAsync();
        }
    }

}
