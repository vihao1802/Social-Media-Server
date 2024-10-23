using Microsoft.EntityFrameworkCore;
using SocialMediaServer.Data;
using SocialMediaServer.ExceptionHandling;
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
            foreach (var post in posts.Result)
            {
                post.Creator = _dbContext.Users.FirstOrDefault(u => u.Id == post.CreatorId);
            }
            return posts;
        }

        public Task<List<Post>> GetAllPostsPublicByUserIdAsync(string userId)
        {
            var posts = _dbContext.Posts
                .Where(post => post.CreatorId == userId && post.Visibility == Visibility.Public)
                .ToListAsync();
            foreach (var post in posts.Result)
            {
                post.Creator = _dbContext.Users.FirstOrDefault(u => u.Id == post.CreatorId);
            }
            return posts;
        }

        public Task<List<Post>> GetAllPostsOnlyFriendByUserIdAsync(string userId)
        {
            var posts = _dbContext.Posts
                .Where(post => post.CreatorId == userId && (post.Visibility == Visibility.Public || post.Visibility == Visibility.FriendsOnly))
                .ToListAsync();
            foreach (var post in posts.Result)
            {
                post.Creator = _dbContext.Users.FirstOrDefault(u => u.Id == post.CreatorId);
            }
            return posts;
        }

        public Task<List<Post>> GetAllPostsByMeAsync(string meId)
        {
            var posts = _dbContext.Posts
                .Where(post => post.CreatorId == meId)
                .ToListAsync();
            foreach (var post in posts.Result)
            {
                post.Creator = _dbContext.Users.FirstOrDefault(u => u.Id == post.CreatorId);
            }
            return posts;
        }

        public async Task<Post> GetByIdAsync(int id)
        {
            var post = await _dbContext.Posts.FirstOrDefaultAsync(p => p.Id == id)
                ?? throw new AppError("Post not found", 404);
            post.Creator = await _dbContext.Users.FirstOrDefaultAsync(u => u.Id == post.CreatorId);
            return post;
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
