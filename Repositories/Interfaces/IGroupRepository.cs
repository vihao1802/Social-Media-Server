using SocialMediaServer.DTOs.Request.GroupMess;
using SocialMediaServer.Models;
using SocialMediaServer.Utils;

namespace SocialMediaServer.Repositories.Interfaces
{
    public interface IGroupRepository
    {
        Task<GroupChat> GetByIdAsync(int id);
        Task<GroupChat> CreateAsync(GroupChat groupChat);
        Task<GroupChat> UpdateAsync(GroupChat groupChat);
        Task<bool> DeleteAsync(int id);
        Task<GroupMessengeDTO> GetAllByGroupIdAsync(int grId, GrMessQueryDTO grMessQueryDTO);
    }
}