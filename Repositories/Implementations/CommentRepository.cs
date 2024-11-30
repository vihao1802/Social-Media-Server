using Microsoft.EntityFrameworkCore;
using SocialMediaServer.Data;
using SocialMediaServer.DTOs.Request.MediaContent;
using SocialMediaServer.DTOs.Request.PostViewer;
using SocialMediaServer.ExceptionHandling;
using SocialMediaServer.Models;
using SocialMediaServer.Repositories.Interfaces;
using SocialMediaServer.Utils;

namespace SocialMediaServer.Repositories.Implementations
{
    public class CommentRepository : ICommentRepository
    {
        private readonly ApplicationDBContext _dbContext;

        public CommentRepository(ApplicationDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<PaginatedResult<Comment>> GetAllCommentAsync(CommentQueryDTO commentQueryDTO)
        {
            var filterParams = new Dictionary<string, object?>
            {
                {"Id", commentQueryDTO.Id},
                {"ParentCommentId", commentQueryDTO.ParentCommentId },
                {"PostId", commentQueryDTO.PostId },
                {"UserId", commentQueryDTO.UserId },
                {"Content_gif", commentQueryDTO.Content_gif },
                {"Content", commentQueryDTO.Content }
            };

            var comments = _dbContext.Comments;

            foreach (var comment in comments)
            {
                comment.Post = _dbContext.Posts.FirstOrDefault(p => p.Id == comment.PostId);
                comment.User = _dbContext.Users.FirstOrDefault(u => u.Id == comment.UserId);
                comment.ParentComment = _dbContext.Comments.FirstOrDefault(c => c.Id == comment.CommentId);
            }

            var commentsQuery = comments
                .ApplyIncludes(commentQueryDTO.Includes)
                .ApplyFilters(filterParams)
                .ApplySorting(commentQueryDTO.Sort)
                .ApplyPaginationAsync(commentQueryDTO.Page, commentQueryDTO.PageSize);

            return await commentsQuery;
        }

        public async Task<PaginatedResult<Comment>> GetAllCommentByPostIdAsync(int postId, CommentQueryDTO commentQueryDTO)
        {
            var filterParams = new Dictionary<string, object?>
            {
                {"Id", commentQueryDTO.Id},
                {"ParentCommentId", commentQueryDTO.ParentCommentId },
                {"PostId", commentQueryDTO.PostId },
                {"UserId", commentQueryDTO.UserId },
                {"Content_gif", commentQueryDTO.Content_gif },
                {"Content", commentQueryDTO.Content }
            };

            var comments = _dbContext.Comments
                .Where(comment => comment.PostId == postId);

            foreach (var comment in comments)
            {
                comment.Post = _dbContext.Posts.FirstOrDefault(p => p.Id == comment.PostId);
                comment.User = _dbContext.Users.FirstOrDefault(u => u.Id == comment.UserId);
                comment.ParentComment = _dbContext.Comments.FirstOrDefault(c => c.Id == comment.CommentId);
            }

            var commentsQuery = comments
                .ApplyIncludes(commentQueryDTO.Includes)
                .ApplyFilters(filterParams)
                .ApplySorting(commentQueryDTO.Sort)
                .ApplyPaginationAsync(commentQueryDTO.Page, commentQueryDTO.PageSize);

            return await commentsQuery;
        }

        public async Task<PaginatedResult<Comment>> GetCommentByUserIdAsync(string userId, CommentQueryDTO commentQueryDTO)
        {
            var filterParams = new Dictionary<string, object?>
            {
                {"Id", commentQueryDTO.Id},
                {"ParentCommentId", commentQueryDTO.ParentCommentId },
                {"PostId", commentQueryDTO.PostId },
                {"UserId", commentQueryDTO.UserId },
                {"Content_gif", commentQueryDTO.Content_gif },
                {"Content", commentQueryDTO.Content }
            };

            var comments = _dbContext.Comments
                .Where(comment => comment.UserId == userId);

            foreach (var comment in comments)
            {
                comment.Post = _dbContext.Posts.FirstOrDefault(p => p.Id == comment.PostId);
                comment.User = _dbContext.Users.FirstOrDefault(u => u.Id == comment.UserId);
                comment.ParentComment = _dbContext.Comments.FirstOrDefault(c => c.Id == comment.CommentId);
            }

            var commentsQuery = comments
                .ApplyIncludes(commentQueryDTO.Includes)
                .ApplyFilters(filterParams)
                .ApplySorting(commentQueryDTO.Sort)
                .ApplyPaginationAsync(commentQueryDTO.Page, commentQueryDTO.PageSize);

            return await commentsQuery;
        }

        public async Task<Comment> GetByIdAsync(int id)
        {
            var comment = await _dbContext.Comments.FirstOrDefaultAsync(p => p.Id == id)
                ?? throw new AppError("Comment not found", 404);
            if (comment.CommentId != null)
            {
                comment.ParentComment = await _dbContext.Comments.FirstOrDefaultAsync(p => p.Id == comment.CommentId);
            }
            return comment;
        }

        public async Task<Comment> CreateAsync(Comment comment)
        {
            _dbContext.Comments.Add(comment);
            await _dbContext.SaveChangesAsync();
            return comment;
        }

        public async Task<Comment> UpdateAsync(Comment comment)
        {
            _dbContext.Comments.Update(comment);
            await _dbContext.SaveChangesAsync();
            return comment;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var comment = await _dbContext.Comments.FirstOrDefaultAsync(p => p.Id == id);
            if (comment == null)
            {
                return false;
            }
            _dbContext.Comments.Remove(comment);
            await _dbContext.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAllByPostIdAsync(int postId)
        {
            var comments = await _dbContext.Comments.Where(comment => comment.PostId == postId).ToListAsync();
            if (comments.Count == 0)
            {
                return false;
            }
            _dbContext.Comments.RemoveRange(comments);
            await _dbContext.SaveChangesAsync();
            return true;
        }
       
    }
}
