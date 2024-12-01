using Microsoft.EntityFrameworkCore;
using SocialMediaServer.Data;
using SocialMediaServer.DTOs.Request.Post;
using SocialMediaServer.DTOs.Request.PostViewer;
using SocialMediaServer.ExceptionHandling;
using SocialMediaServer.Models;
using SocialMediaServer.Repositories.Interfaces;
using SocialMediaServer.Utils;

namespace SocialMediaServer.Repositories.Implementations
{
    public class PostViewerRepository : IPostViewerRepository
    {
        private readonly ApplicationDBContext _dbContext;

        public PostViewerRepository(ApplicationDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<PaginatedResult<PostViewer>> GetAllByPostIdAsync(int postId, PostViewerQueryDTO postViewerQueryDTO)
        {
            var filterParams = new Dictionary<string, object?>
            {
                {"Id", postViewerQueryDTO.Id},
                {"PostId", postViewerQueryDTO.PostId },
                {"UserId", postViewerQueryDTO.UserId }
            };

            var postViewers = _dbContext.PostViewers
                .Where(postViewer => postViewer.PostId == postId);

            foreach (var postViewer in postViewers)
            {
                postViewer.Post = _dbContext.Posts.FirstOrDefault(p => p.Id == postViewer.PostId);
                postViewer.User = _dbContext.Users.FirstOrDefault(u => u.Id == postViewer.UserId);
            }
            var postViewersQuery = postViewers
                .ApplyIncludes(postViewerQueryDTO.Includes)
                .ApplyFilters(filterParams)
                .ApplySorting(postViewerQueryDTO.Sort)
                .ApplyPaginationAsync(postViewerQueryDTO.Page, postViewerQueryDTO.PageSize);

            return await postViewersQuery;
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
