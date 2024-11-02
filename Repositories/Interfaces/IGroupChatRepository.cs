using SocialMediaServer.DTOs.Request.GroupChat;
using SocialMediaServer.Models;
using SocialMediaServer.Utils;

namespace SocialMediaServer.Repositories.Interfaces
{
    public interface IGroupChatRepository
    {
       
       Task<GroupChat> GetByIdAsync(int id);
       Task<GroupChat> CreateAsync(GroupChatCreateDTO groupChat);
       Task<GroupChat> UpdateAsync(GroupChat groupChat);
       Task<bool> DeleteAsync(int id);
       Task<PaginatedResult<GroupChat>> GetAllAsync(GroupChatQueryDTO postQueryDTO);
       Task<List<GroupChat>> SearchByNameAsync(string searchString);
    }
}