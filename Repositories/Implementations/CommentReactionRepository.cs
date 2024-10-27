using Microsoft.EntityFrameworkCore;
using SocialMediaServer.Data;
using SocialMediaServer.DTOs.Request.MediaContent;
using SocialMediaServer.ExceptionHandling;
using SocialMediaServer.Models;
using SocialMediaServer.Repositories.Interfaces;
using SocialMediaServer.Utils;

namespace SocialMediaServer.Repositories.Implementations
{
    public class CommentReactionRepository : ICommentReactionRepository
    {
        private readonly ApplicationDBContext _dbContext;

        public CommentReactionRepository(ApplicationDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<PaginatedResult<CommentReaction>> GetAllByCommentIdAsync(int commentId, CommentReactionQueryDTO commentReactionQueryDTO)
        {
            var filterParams = new Dictionary<string, object?>
            {
                {"Id", commentReactionQueryDTO.Id},
                {"UserId", commentReactionQueryDTO.UserId },
                {"CommentId", commentReactionQueryDTO.CommentId },
                {"Reaction_at", commentReactionQueryDTO.Reaction_at }
            };

            var commentReactions = _dbContext.CommentReactions
                .Where(commentReaction => commentReaction.CommentId == commentId);

            foreach (var commentReaction in commentReactions)
            {
                commentReaction.Comment = _dbContext.Comments.FirstOrDefault(p => p.Id == commentReaction.CommentId);
                commentReaction.User = _dbContext.Users.FirstOrDefault(u => u.Id == commentReaction.UserId);
            }

            var commentReactionsQuery = commentReactions
                .ApplyIncludes(commentReactionQueryDTO.Includes)
                .ApplyFilters(filterParams)
                .ApplySorting(commentReactionQueryDTO.Sort)
                .ApplyPaginationAsync(commentReactionQueryDTO.Page, commentReactionQueryDTO.PageSize);

            return await commentReactionsQuery;
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
