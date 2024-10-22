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
            return await _dbContext.PostViewers.FirstOrDefaultAsync(pv => pv.Id == id);
                
        }

        public async Task<bool> DeleteByPostidAsync(int id, int postId)
        {
            var postViewer = await _dbContext.PostViewers.FirstOrDefaultAsync(pv => pv.Id == id && pv.PostId == postId);
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
