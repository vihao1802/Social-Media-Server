using Microsoft.EntityFrameworkCore;
using SocialMediaServer.Data;
using SocialMediaServer.ExceptionHandling;
using SocialMediaServer.Models;
using SocialMediaServer.Repositories.Interfaces;

namespace SocialMediaServer.Repositories.Implementations
{
    public class CommentRepository : ICommentRepository
    {
        private readonly ApplicationDBContext _dbContext;

        public CommentRepository(ApplicationDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        public Task<List<Comment>> GetAllCommentAsync()
        {
            var comments = _dbContext.Comments.ToListAsync();
            return comments;
        }

        public Task<List<Comment>> GetAllCommentByPostIdAsync(int postId)
        {
            var comments = _dbContext.Comments
                .Where(comment => comment.PostId == postId)
                .ToListAsync();
            return comments;
        }

        public Task<List<Comment>> GetCommentByUserIdAsync(string userId)
        {
            var comments = _dbContext.Comments
                .Where(comment => comment.UserId == userId)
                .ToListAsync();
            return comments;
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
