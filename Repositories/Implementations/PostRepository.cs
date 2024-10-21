using Microsoft.EntityFrameworkCore;
using SocialMediaServer.Data;
using SocialMediaServer.Models;
using SocialMediaServer.Repositories.Interfaces;

namespace SocialMediaServer.Repositories.Implementations
{
    public class PostRepository : IPostRepository
    {
        private readonly ApplicationDBContext _dbContext;

        public PostRepository(ApplicationDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        public Task<List<Post>> GetAllPostsAsync()
        {
            var posts = _dbContext.Posts
                .Where(post => post.Visibility == Visibility.Public)
                .ToListAsync();
            return posts;
        }

        public Task<List<Post>> GetAllPostsPublicByUserIdAsync(string userId)
        {
            var posts = _dbContext.Posts
                .Where(post => post.CreatorId == userId && post.Visibility == Visibility.Public)
                .ToListAsync();
            return posts;
        }

        public Task<List<Post>> GetAllPostsOnlyFriendByUserIdAsync(string userId)
        {
            var posts = _dbContext.Posts
                .Where(post => post.CreatorId == userId && (post.Visibility == Visibility.Public || post.Visibility == Visibility.FriendsOnly))
                .ToListAsync();
            return posts;
        }

        public Task<List<Post>> GetAllPostsByMeAsync(string meId)
        {
            var posts = _dbContext.Posts
                .Where(post => post.CreatorId == meId)
                .ToListAsync();
            return posts;
        }

        public async Task<Post> GetByIdAsync(int id)
        {
            return await _dbContext.Posts.FirstOrDefaultAsync(p => p.Id == id) ?? throw new ArgumentNullException(nameof(id));
        }

        public async Task<Post> CreateAsync(Post post)
        {
            _dbContext.Posts.Add(post);
            await _dbContext.SaveChangesAsync();
            return post;
        }

        public async Task<Post> UpdateAsync(Post post)
        {
            _dbContext.Posts.Update(post);
            await _dbContext.SaveChangesAsync();
            return post;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var post = await _dbContext.Posts.FirstOrDefaultAsync(p => p.Id == id);
            if (post == null)
            {
                return false;
            }
            _dbContext.Posts.Remove(post);
            await _dbContext.SaveChangesAsync();
            return true;
        }
    }
}
