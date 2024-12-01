using Microsoft.EntityFrameworkCore;
using SocialMediaServer.Data;
using SocialMediaServer.DTOs.Request.GroupChat;
using SocialMediaServer.ExceptionHandling;
using SocialMediaServer.Models;
using SocialMediaServer.Repositories.Interfaces;
using SocialMediaServer.Utils;

namespace SocialMediaServer.Repositories.Implementations
{
    public class GroupMessengeRepository : IGroupMessengeRepository
    {
        private readonly ApplicationDBContext _dbContext;
        public GroupMessengeRepository(ApplicationDBContext context)
        {
            _dbContext = context;
        }
        public async Task<GroupMessenge> CreateAsync(GroupMessenge groupMessenge)
        {
            _dbContext.GroupMessenges.Add(groupMessenge);
            await _dbContext.SaveChangesAsync();
            return groupMessenge;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var groupMessenge = await _dbContext.GroupMessenges.FirstOrDefaultAsync(p => p.Id == id);
            if (groupMessenge == null)
            {
                return false;
            }
            _dbContext.GroupMessenges.Remove(groupMessenge);
            await _dbContext.SaveChangesAsync();
            return true;
        }

        public async Task<PaginatedResult<GroupMessenge>> GetAllByGroupIdAsync(GrMessQueryDTO grMessQueryDTO, int idGroup)
        {
            var check = await _dbContext.Groups.FirstOrDefaultAsync(p => p.Id == idGroup)
                        ?? throw new AppError("Group not found", 404);
            var filterParams = new Dictionary<string, object?>
            {
                {"GroupId", grMessQueryDTO.GroupId }
            };

            var grMessenges = _dbContext.GroupMessenges
                          .Include(c => c.Replies)
                          .Include(c => c.ReplyTo)
                          .Include(c => c.Reactions)
                          .Include(c => c.Sender)
                          .Where(x => x.GroupChatId == idGroup);
            Console.WriteLine(grMessenges.Count());
            var grMessQuery = grMessenges
                .ApplySorting(grMessQueryDTO.Sort)
                .ApplyPaginationAsync(grMessQueryDTO.Page, grMessQueryDTO.PageSize);

            return await grMessQuery;
        }

        public async Task<GroupMessenge> GetByIdAsync(int id)
        {
            var grMess = await _dbContext.GroupMessenges.FirstOrDefaultAsync(p => p.Id == id)
                ?? throw new AppError("Group messenge not found", 404);
            
            return grMess;
        }

        public async Task<bool> DeleteAllByGroupIdAsync(int groupId)
        {
            var grMessenges = await _dbContext.GroupMessenges.Where(grMess => grMess.GroupChatId == groupId).ToListAsync();
            if (grMessenges.Count == 0)
            {
                return false;
            }
            _dbContext.GroupMessenges.RemoveRange(grMessenges);
            await _dbContext.SaveChangesAsync();
            return true;
        }

        public async Task<bool> RecallAsync(int grMessId)
        {
            var groupMessenge = await _dbContext.GroupMessenges.FirstOrDefaultAsync(p => p.Id == grMessId);
            if (groupMessenge == null)
            {
                return false;
            }
            groupMessenge.isDelete = true;
            _dbContext.GroupMessenges.Update(groupMessenge);
            await _dbContext.SaveChangesAsync();
            return true;
        }
    }
}