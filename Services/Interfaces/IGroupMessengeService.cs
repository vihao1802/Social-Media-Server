using DTOs.Response;
using SocialMediaServer.DTOs.Request.GroupChat;
using SocialMediaServer.DTOs.Request.GroupMess;
using SocialMediaServer.Utils;

namespace SocialMediaServer.Services.Interfaces
{
    public interface IGroupMessengeService
    {
        Task<PaginatedResult<GrMessResponseDTO>> GetAllByGroupIdAsync(int groupId, GrMessQueryDTO grMessQueryDTO, string userId);
        Task<GrMessResponseDTO> CreateAsync(GrMessCreateDTO grMessCreateDTO);
        Task<bool> DeleteAsync(int id);
        Task<bool> RecallAsync(int id);
        Task<bool> DeleteAllByGroupIdAsync(int groupId);
    }
}