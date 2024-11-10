using Microsoft.EntityFrameworkCore;
using SocialMediaServer.Data;
using SocialMediaServer.DTOs.Request.GroupMess;
using SocialMediaServer.ExceptionHandling;
using SocialMediaServer.Models;
using SocialMediaServer.Repositories.Interfaces;
using SocialMediaServer.Utils;

namespace SocialMediaServer.Repositories.Implementations
{
    public class GroupRepository : IGroupRepository
    {
        private readonly ApplicationDBContext _dbContext;

        public GroupRepository(ApplicationDBContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<GroupChat> CreateAsync(GroupChat groupChat)
        {
            _dbContext.Groups.Add(groupChat);
            await _dbContext.SaveChangesAsync();
            return groupChat;
        }

        public async Task<GroupChat> GetByIdAsync(int id)
        {
            var group = await _dbContext.Groups.FirstOrDefaultAsync(p => p.Id == id)
                ?? throw new AppError("Group not found", 404);
            
            return group;
        }

        public async Task<GroupChat> UpdateAsync(GroupChat group)
        {
            _dbContext.Groups.Update(group);
            await _dbContext.SaveChangesAsync();
            return group;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var group = await _dbContext.Groups.FirstOrDefaultAsync(p => p.Id == id);
            if (group == null)
            {
                return false;
            }
            _dbContext.Groups.Remove(group);
            await _dbContext.SaveChangesAsync();
            return true;
        }

        public async Task<GroupMessengeDTO> GetAllByGroupIdAsync(int grId, GrMessQueryDTO grMessQueryDTO)
        {
            var groupChat = await _dbContext.Groups
                .Include(g => g.Members)
                .Include(g => g.Messages)
                .FirstOrDefaultAsync(g => g.Id == grId);

            if (groupChat == null)
            {
                throw new KeyNotFoundException("Group not found.");
            }

            // Lọc danh sách các thành viên đã rời nhóm
            var leftMembers = groupChat.Members
                .Where(m => m.IsLeft)
                .Select(m => new GroupMemberDTO
                {
                    Id = m.Id,
                    UserId = m.UserId,
                    Join_at = m.Join_at,
                    Left_at = m.Left_at,
                })
                .ToList();

            // Lọc và phân trang danh sách tin nhắn
            var pagedMessages = groupChat.Messages
                .OrderBy(m => m.Sent_at) // Sắp xếp theo thời gian gửi
                .Skip((grMessQueryDTO.Page - 1) * grMessQueryDTO.PageSize) // Bỏ qua số tin nhắn tương ứng với số trang
                .Take(grMessQueryDTO.PageSize) // Lấy số tin nhắn theo PageSize
                .Select(m => new MessageDTO
                {
                    Id = m.Id,
                    Content = m.Content,
                    Media_content = m.Media_content,
                    Sent_at = m.Sent_at,
                    SenderId = m.SenderId,
                })
                .ToList();

            return new GroupMessengeDTO
            {
                GroupId = groupChat.Id,
                GroupName = groupChat.Group_name,
                Messages = pagedMessages,
                LeftMembers = leftMembers
            };
        }
    }
}