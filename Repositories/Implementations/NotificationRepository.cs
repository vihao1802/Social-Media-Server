using Microsoft.EntityFrameworkCore;
using SocialMediaServer.Data;
using SocialMediaServer.DTOs.Request.Notification;
using SocialMediaServer.ExceptionHandling;
using SocialMediaServer.Models;
using SocialMediaServer.Repositories.Interfaces;
using SocialMediaServer.Utils;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SocialMediaServer.Repositories.Implementations
{
    public class NotificationRepository : INotificationRepository
    {
        private readonly ApplicationDBContext _dbContext;

        public NotificationRepository(ApplicationDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<PaginatedResult<Notification>> GetAllNotificationsAsync(NotificationQueryDTO notificationQueryDTO)
        {
            var filterParams = new Dictionary<string, object?>
            {
                {"Id", notificationQueryDTO.Id},
                {"GroupId", notificationQueryDTO.GroupId},
                {"Content", notificationQueryDTO.Content}
            };

            var notifications = _dbContext.Notifications;

            foreach (var notification in notifications)
            {
                notification.Group = _dbContext.Groups.FirstOrDefault(g => g.Id == notification.GroupId);
            }

            var notificationsQuery = notifications
                .ApplyIncludes(notificationQueryDTO.Includes)
                .ApplyFilters(filterParams)
                .ApplySorting(notificationQueryDTO.Sort)
                .ApplyPaginationAsync(notificationQueryDTO.Page, notificationQueryDTO.PageSize);

            return await notificationsQuery;
        }

        public async Task<Notification> GetByIdAsync(int id)
        {
            var notification = await _dbContext.Notifications.FirstOrDefaultAsync(n => n.Id == id)
                ?? throw new AppError("Notification not found", 404);

            notification.Group = await _dbContext.Groups.FirstOrDefaultAsync(g => g.Id == notification.GroupId);
            return notification;
        }

        public async Task<Notification> CreateAsync(Notification notification)
        {
            _dbContext.Notifications.Add(notification);
            await _dbContext.SaveChangesAsync();
            return notification;
        }

        public async Task<Notification> UpdateAsync(Notification notification)
        {
            _dbContext.Notifications.Update(notification);
            await _dbContext.SaveChangesAsync();
            return notification;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var notification = await _dbContext.Notifications.FirstOrDefaultAsync(n => n.Id == id);
            if (notification == null)
            {
                return false;
            }
            _dbContext.Notifications.Remove(notification);
            await _dbContext.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAllByGroupIdAsync(int groupId)
        {
            var notifications = await _dbContext.Notifications.Where(n => n.GroupId == groupId).ToListAsync();
            if (!notifications.Any())
            {
                return false;
            }
            _dbContext.Notifications.RemoveRange(notifications);
            await _dbContext.SaveChangesAsync();
            return true;
        }
    }
}
