
using SocialMediaServer.Models;

namespace SocialMediaServer.Repositories.Interfaces
{
    public interface ICommentReactionRepository
    {
       Task<List<CommentReaction>> GetAllByCommentIdAsync(int commentId);
       Task<CommentReaction> CreateByCommentIdAsync(CommentReaction commentReaction);
       Task<CommentReaction> GetByIdAsync(int id);
       Task<bool> DeleteByIdAsync(int id);
    }
}