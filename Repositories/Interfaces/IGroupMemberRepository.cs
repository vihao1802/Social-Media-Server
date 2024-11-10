using SocialMediaServer.Models;

namespace SocialMediaServer.Repositories.Interfaces
{
    public interface IGroupMemberRepository
    {
        Task<GroupMember> CreateAsync(GroupMember groupMember);
        Task<GroupMember> UpdateAsync(GroupMember GroupMember);
        Task<bool> DeleteAsync(int id);
        Task<GroupMember> GetByIdAsync(int id);
        Task<GroupMember> GetByUserIdAndGroupId(int gr_id, string user_id);
    }
}