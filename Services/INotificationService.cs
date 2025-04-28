using ProjectPRN222.Models;

namespace ProjectPRN222.Services
{
    public interface INotificationService
    {
        Task CreateNotification(Notification notification);
        Task UpdateNotification(Notification notification);
        Task DeleteNotification(int notificationId);
        Task<Notification> GetNotificationByIdAsync(int id);  
        Task<List<Notification>> GetAllNotificationsAsync();
    }

}
