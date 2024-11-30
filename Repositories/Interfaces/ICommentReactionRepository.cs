
using SocialMediaServer.DTOs.Request.MediaContent;
using SocialMediaServer.Models;
using SocialMediaServer.Utils;

namespace SocialMediaServer.Repositories.Interfaces
{
    public interface ICommentReactionRepository
    {
       Task<PaginatedResult<CommentReaction>> GetAllByCommentIdAsync(int commentId, CommentReactionQueryDTO commentReactionQueryDTO);
       Task<CommentReaction> CreateByCommentIdAsync(CommentReaction commentReaction);
       Task<CommentReaction> GetByIdAsync(int id);
       Task<bool> DeleteByIdAsync(int id);
    }
}