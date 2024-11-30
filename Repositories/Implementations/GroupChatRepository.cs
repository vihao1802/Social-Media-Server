using Microsoft.EntityFrameworkCore;
using SocialMediaServer.Data;
using SocialMediaServer.DTOs.Request.GroupChat;
using SocialMediaServer.ExceptionHandling;
using SocialMediaServer.Models;
using SocialMediaServer.Repositories.Interfaces;
using SocialMediaServer.Utils;

namespace SocialMediaServer.Repositories.Implementations
{
    public class GroupChatRepository : IGroupChatRepository
    {
        private readonly ApplicationDBContext _dbContext;

        public GroupChatRepository(ApplicationDBContext context)
        {
            _dbContext = context;
        }

        public async Task<GroupChat> CreateAsync(GroupChatCreateDTO groupChatDto)
        {
            var groupChat = new GroupChat
            {
                Group_name = groupChatDto.name,
                Group_avt = groupChatDto.avatar,
                AdminId = groupChatDto.AdminId,
                Created_at = DateTime.Now
            };
            
            _dbContext.Groups.Add(groupChat);
            await _dbContext.SaveChangesAsync();
            return groupChat;
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

        public async Task<PaginatedResult<GroupChat>> GetAllAsync(GroupChatQueryDTO grChatQueryDTO)
        {
            var filterParams = new Dictionary<string, object?>
            {
                {"Id", grChatQueryDTO.Id},
                {"Group_name", grChatQueryDTO.Group_name},
                {"Group_avt", grChatQueryDTO.Group_avt},
                {"Created_at", grChatQueryDTO.Created_at}
            };

            var grChats = _dbContext.Groups
                            .Where(g => g.isDelete == false);
            var grChatsQuery = grChats
                .ApplyIncludes(grChatQueryDTO.Includes)
                .ApplyFilters(filterParams)
                .ApplySorting(grChatQueryDTO.Sort)
                .ApplyPaginationAsync(grChatQueryDTO.Page, grChatQueryDTO.PageSize);

            return await grChatsQuery;
        }

        public async Task<GroupChat> GetByIdAsync(int id)
        {
            var grChat = await _dbContext.Groups.Include(g => g.Members).Include(g => g.Messages)
                           .FirstOrDefaultAsync(p => p.Id == id)
                        ?? throw new AppError("Group Chat not found", 404);
            return grChat;
        }

        public async Task<GroupChat> UpdateAsync(GroupChat groupChat)
        {
            _dbContext.Groups.Update(groupChat);
            await _dbContext.SaveChangesAsync();
            return groupChat;
        }

        public async Task<List<GroupChat>> SearchByNameAsync(string searchString)
        {
            return await _dbContext.Groups
                .Where(gr => gr.Group_name.Contains(searchString))
                .ToListAsync();
        }

        public async Task<PaginatedResult<GroupChat>> GetAllByUserAsync(string userId)
        {
            
            var groupChats = _dbContext.Groups
                .Include(g => g.Members)
                .Where(g => g.isDelete == false && g.Members.Any(m => m.UserId == userId)) // Only include non-deleted groups where the user is a  member
                .Include(g => g.Messages.OrderByDescending(m => m.Sent_at).Take(1)).ThenInclude(m => m.Sender)  
                .ApplyPaginationAsync(1, 10); // Only get the latest message of each group chat

            return await groupChats;
        }
    }
}
