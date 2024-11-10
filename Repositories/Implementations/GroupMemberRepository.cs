using Microsoft.EntityFrameworkCore;
using SocialMediaServer.Data;
using SocialMediaServer.ExceptionHandling;
using SocialMediaServer.Models;
using SocialMediaServer.Repositories.Interfaces;

namespace SocialMediaServer.Repositories.Implementations
{
    public class GroupMemberRepository : IGroupMemberRepository
    {
        private readonly ApplicationDBContext _dbContext;

        public GroupMemberRepository(ApplicationDBContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<GroupMember> CreateAsync(GroupMember groupMember)
        {
            _dbContext.GroupMembers.Add(groupMember);
            await _dbContext.SaveChangesAsync();
            return groupMember;
        }
        public async Task<GroupMember> UpdateAsync(GroupMember grMember)
        {
            _dbContext.GroupMembers.Update(grMember);
            await _dbContext.SaveChangesAsync();
            return grMember;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var groupMember = await _dbContext.GroupMembers.FirstOrDefaultAsync(p => p.Id == id);
            if (groupMember == null)
            {
                return false;
            }
            _dbContext.GroupMembers.Remove(groupMember);
            await _dbContext.SaveChangesAsync();
            return true;
        }

        public async Task<GroupMember> GetByIdAsync(int id)
        {
            var group = await _dbContext.GroupMembers.FirstOrDefaultAsync(p => p.Id == id)
                ?? throw new AppError("Group Member not found", 404);
            
            return group;
        }
        public async Task<GroupMember> GetByUserIdAndGroupId(int gr_id, string user_id)
        {
            return await _dbContext.GroupMembers
                .FirstOrDefaultAsync(gm => gm.GroupChatId == gr_id && gm.UserId == user_id);
        }
    }
}