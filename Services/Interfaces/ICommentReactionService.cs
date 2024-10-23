using DTOs.Response;
using SocialMediaServer.DTOs.Request.MediaContent;

namespace SocialMediaServer.Services.Interfaces
{
    public interface ICommentReactionService
    {
        Task<List<CommentReactionResponseDTO>> GetAllByCommentIdAsync(int commentId);
        Task<CommentReactionResponseDTO> CreateByCommentIdAsync(CommentReactionCreateDTO commentReactionCreateDTO);

        Task<CommentReactionResponseDTO> GetByIdAsync(int id);

        Task<bool> DeleteByIdAsync(int id);
    }
}
