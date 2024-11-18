using SocialMediaServer.DTOs.Request.MessageReaction;
using SocialMediaServer.DTOs.Response;
using SocialMediaServer.Utils;
using System.Threading.Tasks;

namespace SocialMediaServer.Services.Interfaces
{
    public interface IMessageReactionService
    {
        Task<PaginatedResult<MessageReactionResponseDTO>> GetAllReactionsAsync(MessageReactionQueryDTO queryDTO);
        Task<PaginatedResult<MessageReactionResponseDTO>> GetAllByGroupMessageIdAsync(int groupMessageId, MessageReactionQueryDTO queryDTO);
        Task<PaginatedResult<MessageReactionResponseDTO>> GetAllByUserIdAsync(string userId, MessageReactionQueryDTO queryDTO);
        Task<MessageReactionResponseDTO> GetByIdAsync(int id);
        Task<MessageReactionResponseDTO> CreateAsync(MessageReactionCreateDTO reactionCreateDTO);
        Task<MessageReactionResponseDTO> UpdateAsync(MessageReactionUpdateDTO reactionUpdateDTO, int id);
        Task<bool> DeleteAsync(int id);
        Task<bool> DeleteAllByGroupMessageIdAsync(int groupMessageId);
    }
}
