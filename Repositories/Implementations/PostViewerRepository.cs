using Microsoft.EntityFrameworkCore;
using SocialMediaServer.Data;
using SocialMediaServer.ExceptionHandling;
using SocialMediaServer.Models;
using SocialMediaServer.Repositories.Interfaces;

namespace SocialMediaServer.Repositories.Implementations
{
    public class PostViewerRepository : IPostViewerRepository
    {
        private readonly ApplicationDBContext _dbContext;

        public PostViewerRepository(ApplicationDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        public Task<List<PostViewer>> GetAllByPostIdAsync(int postId)
        {
            var postViewers = _dbContext.PostViewers
                .Where(postViewer => postViewer.PostId == postId)
                .ToListAsync();
            foreach (var postViewer in postViewers.Result)
            {
                postViewer.Post = _dbContext.Posts.FirstOrDefault(p => p.Id == postViewer.PostId);
                postViewer.User = _dbContext.Users.FirstOrDefault(u => u.Id == postViewer.UserId);
            }
            return postViewers;
        }

        public async Task<PostViewer> CreateByPostIdAsync(PostViewer postViewer)
        {
            _dbContext.PostViewers.Add(postViewer);
            await _dbContext.SaveChangesAsync();
            return postViewer;
        }

        public async Task<PostViewer> GetByIdAsync(int id)
        {
            var postViewer = await _dbContext.PostViewers.FirstOrDefaultAsync(pv => pv.Id == id)
                ?? throw new AppError("Post viewer not found", 404);
            postViewer.Post = await _dbContext.Posts.FirstOrDefaultAsync(p => p.Id == postViewer.PostId);
            postViewer.User = await _dbContext.Users.FirstOrDefaultAsync(u => u.Id == postViewer.UserId);

            return postViewer;
                
        }

        public async Task<bool> DeleteByIdAsync(int id)
        {
            var postViewer = await _dbContext.PostViewers.FirstOrDefaultAsync(pv => pv.Id == id);
            if (postViewer == null)
            {
                return false;
            }
            _dbContext.PostViewers.Remove(postViewer);
            await _dbContext.SaveChangesAsync();
            return true;
        }
    }
}
