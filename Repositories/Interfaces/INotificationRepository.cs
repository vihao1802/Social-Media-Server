using SocialMediaServer.DTOs.Request.Notification;
using SocialMediaServer.Models;
using SocialMediaServer.Utils;
using System.Threading.Tasks;

namespace SocialMediaServer.Repositories.Interfaces
{
    public interface INotificationRepository
    {
        Task<PaginatedResult<Notification>> GetAllNotificationsAsync(NotificationQueryDTO notificationQueryDTO);
        Task<Notification> GetByIdAsync(int id);
        Task<Notification> CreateAsync(Notification notification);
        Task<Notification> UpdateAsync(Notification notification);
        Task<bool> DeleteAsync(int id);
        Task<bool> DeleteAllByGroupIdAsync(int groupId);
        
    }
}
