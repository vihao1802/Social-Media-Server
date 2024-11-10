using SocialMediaServer.DTOs.Request.GroupMember;
using SocialMediaServer.Models;
using SocialMediaServer.Utils;

namespace SocialMediaServer.Repositories.Interfaces
{
    public interface IGroupMemberRepository
    {
        Task<GroupMember> GetByIdAsync(int id);
        Task<GroupMember> UpdateAsync(GroupMember grMember);
        Task<PaginatedResult<GroupMember>> GetByUserIdAsync(string userId, GroupMemberQueryDTO grMemberQueryDTO);
        Task<PaginatedResult<GroupMember>> GetAllByGroupIdAsync(int groupId, GroupMemberQueryDTO grMemberQueryDTO);
        Task<PaginatedResult<GroupMember>> GetAllAsync(GroupMemberQueryDTO grMemberQueryDTO);
        Task<bool> DeleteAsync(int id);
        Task<GroupMember> CreateAsync(GroupMember grMember);
        
    }
}