using Microsoft.EntityFrameworkCore;
using SocialMediaServer.Data;
using SocialMediaServer.ExceptionHandling;
using SocialMediaServer.Models;
using SocialMediaServer.Repositories.Interfaces;

namespace SocialMediaServer.Repositories.Implementations
{
    public class CommentReactionRepository : ICommentReactionRepository
    {
        private readonly ApplicationDBContext _dbContext;

        public CommentReactionRepository(ApplicationDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        public Task<List<CommentReaction>> GetAllByCommentIdAsync(int commentId)
        {
            var commentReactions = _dbContext.CommentReactions
                .Where(commentReaction => commentReaction.CommentId == commentId)
                .ToListAsync();
            foreach (var commentReaction in commentReactions.Result)
            {
                commentReaction.Comment = _dbContext.Comments.FirstOrDefault(p => p.Id == commentReaction.CommentId);
                commentReaction.User = _dbContext.Users.FirstOrDefault(u => u.Id == commentReaction.UserId);
            }
            return commentReactions;
        }

        public async Task<CommentReaction> CreateByCommentIdAsync(CommentReaction commentReaction)
        {
            _dbContext.CommentReactions.Add(commentReaction);
            await _dbContext.SaveChangesAsync();
            return commentReaction;
        }

        public async Task<CommentReaction> GetByIdAsync(int id)
        {
            var commentReaction = await _dbContext.CommentReactions.FirstOrDefaultAsync(pv => pv.Id == id)
                ?? throw new AppError("Comment Reaction not found", 404);
            commentReaction.Comment = _dbContext.Comments.FirstOrDefault(p => p.Id == commentReaction.CommentId);
            commentReaction.User = _dbContext.Users.FirstOrDefault(u => u.Id == commentReaction.UserId);

            return commentReaction;
                
        }

        public async Task<bool> DeleteByIdAsync(int id)
        {
            var commentReaction = await _dbContext.CommentReactions.FirstOrDefaultAsync(pv => pv.Id == id);
            if (commentReaction == null)
            {
                return false;
            }
            _dbContext.CommentReactions.Remove(commentReaction);
            await _dbContext.SaveChangesAsync();
            return true;
        }
    }
}
