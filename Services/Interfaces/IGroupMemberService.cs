using SocialMediaServer.DTOs.Request.GroupMember;
using SocialMediaServer.DTOs.Request.MediaContent;
using SocialMediaServer.DTOs.Response;
using SocialMediaServer.Utils;

namespace SocialMediaServer.Services.Interfaces
{
    public interface IGroupMemberService
    {
        Task<PaginatedResult<GroupMemberResponseDTO>> GetAllAsync(GroupMemberQueryDTO grMemberQueryDTO);
        Task<PaginatedResult<GroupMemberResponseDTO>> GetAllByGroupIdAsync(int groupId, GroupMemberQueryDTO grMemberQueryDTO);
        Task<PaginatedResult<GroupMemberResponseDTO>> GetAllByUserIdAsync(string userId, GroupMemberQueryDTO grMemberQueryDTO);
        Task<GroupMemberResponseDTO> GetByIdAsync(int id);
        Task<GroupMemberResponseDTO> CreateAsync(GroupMemberCreateDTO grMemberCreateDTO);
        Task<GroupMemberResponseDTO> UpdateAsync(GroupMemberUpdateDTO grMemberUpdateDTO, int id);
        Task<bool> DeleteAsync(int id);
    }
}