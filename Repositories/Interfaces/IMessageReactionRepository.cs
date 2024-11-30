using SocialMediaServer.DTOs.Request.MessageReaction;
using SocialMediaServer.Models;
using SocialMediaServer.Utils;
using System.Threading.Tasks;

namespace SocialMediaServer.Repositories.Interfaces
{
    public interface IMessageReactionRepository
    {
        Task<PaginatedResult<MessageReaction>> GetAllReactionsAsync(MessageReactionQueryDTO queryDTO);
        Task<MessageReaction> GetByIdAsync(int id);
        Task<MessageReaction> CreateAsync(MessageReaction messageReaction);
        Task<MessageReaction> UpdateAsync(MessageReaction messageReaction);
        Task<bool> DeleteAsync(int id);
        Task<bool> DeleteAllByGroupMessageIdAsync(int groupMessageId);
    }
}
