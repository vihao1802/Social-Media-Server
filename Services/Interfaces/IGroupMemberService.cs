using SocialMediaServer.DTOs.Request.GroupChat;
using SocialMediaServer.DTOs.Request.GroupMember;
using SocialMediaServer.DTOs.Response;

namespace SocialMediaServer.Services.Interfaces
{
    public interface IGroupMemberService
    {
        Task<MemberResponseDTO> GetByIdAsync(int id);
        Task<MemberResponseDTO> CreateAsync(int gr_id, string user_id);
        Task<MemberResponseDTO> UpdateAsync(GroupMemberUpdateDTO grMemberUpdateDTO, int id);
        Task<bool> DeleteAsync(int member_id);
    }
}