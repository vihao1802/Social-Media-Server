using SocialMediaServer.DTOs.Request.GroupChat;
using SocialMediaServer.DTOs.Request.GroupMess;
using SocialMediaServer.DTOs.Response;

namespace SocialMediaServer.Services.Interfaces
{
    public interface IGroupService
    {
        Task<GroupResponseDTO> GetByIdAsync(int id);
        Task<GroupResponseDTO> CreateAsync(GroupCreateDTO groupCreateDTO);
        Task<GroupResponseDTO> UpdateAsync(GroupUpdateDTO grUpdateDTO, int id, IFormFile? mediaFile);
        Task<bool> EndGroup(int id);
        Task<GroupMessengeDTO> GetAllByGroupIdAsync(int grId, GrMessQueryDTO grMessQueryDTO);
    }
}