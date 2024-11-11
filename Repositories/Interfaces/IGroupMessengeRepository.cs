using SocialMediaServer.DTOs.Request.GroupChat;
using SocialMediaServer.Models;
using SocialMediaServer.Utils;

namespace SocialMediaServer.Repositories.Interfaces
{
    public interface IGroupMessengeRepository
    {
        Task<PaginatedResult<GroupMessenge>> GetAllByGroupIdAsync(GrMessQueryDTO grMessQueryDTO, int idGroup);
        Task<GroupMessenge> CreateAsync(GroupMessenge groupMessenge);
        Task<bool> DeleteAsync(int id);
        Task<GroupMessenge> GetByIdAsync(int id);
        Task<bool> DeleteAllByGroupIdAsync(int grMessId);
    }
}