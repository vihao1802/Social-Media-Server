using DTOs.Response;
using SocialMediaServer.DTOs.Request.MediaContent;
using SocialMediaServer.Utils;

namespace SocialMediaServer.Services.Interfaces
{
    public interface ICommentReactionService
    {
        Task<PaginatedResult<CommentReactionResponseDTO>> GetAllByCommentIdAsync(int commentId, CommentReactionQueryDTO commentReactionQueryDTO);
        Task<CommentReactionResponseDTO> CreateByCommentIdAsync(CommentReactionCreateDTO commentReactionCreateDTO);

        Task<CommentReactionResponseDTO> GetByIdAsync(int id);

        Task<bool> DeleteByIdAsync(int id);
    }
}
