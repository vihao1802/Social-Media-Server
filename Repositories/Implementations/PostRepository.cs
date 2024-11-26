using Microsoft.EntityFrameworkCore;
using SocialMediaServer.Data;
using SocialMediaServer.DTOs.Request.Post;
using SocialMediaServer.ExceptionHandling;
using SocialMediaServer.Models;
using SocialMediaServer.Repositories.Interfaces;
using SocialMediaServer.Utils;

namespace SocialMediaServer.Repositories.Implementations
{
    public class PostRepository : IPostRepository
    {
        private readonly ApplicationDBContext _dbContext;

        public PostRepository(ApplicationDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<PaginatedResult<Post>> GetAllPostsAsync(PostQueryDTO postQueryDTO)
        {
            var filterParams = new Dictionary<string, object?>
                    {
                        {"Id", postQueryDTO.Id},
                        {"Content", postQueryDTO.Content },
                        {"CreatorId", postQueryDTO.CreatorId },
                        {"Create_at", postQueryDTO.Create_at }
                    };

            var posts = _dbContext.Posts
                .Where(post => post.Visibility == Visibility.Public);

            foreach (var post in posts)
            {
                post.Creator = _dbContext.Users.FirstOrDefault(p => p.Id == post.CreatorId);
            }

            var postsQuery = posts
                .ApplyIncludes(postQueryDTO.Includes)
                .ApplyFilters(filterParams)
                .ApplySorting(postQueryDTO.Sort)
                .ApplyPaginationAsync(postQueryDTO.Page, postQueryDTO.PageSize);
            
            return await postsQuery;
        }

        public async Task<PaginatedResult<Post>> GetAllPostsPublicByUserIdAsync(string userId, PostQueryDTO postQueryDTO)
        {
            var filterParams = new Dictionary<string, object?>
                    {
                        {"Id", postQueryDTO.Id},
                        {"Content", postQueryDTO.Content },
                        {"CreatorId", postQueryDTO.CreatorId },
                        {"Create_at", postQueryDTO.Create_at }
                    };

            var posts = _dbContext.Posts
                .Where(post => post.CreatorId == userId && post.Visibility == Visibility.Public);
                
            foreach (var post in posts)
            {
                post.Creator = _dbContext.Users.FirstOrDefault(u => u.Id == post.CreatorId);
            }

            var postsQuery = posts
                .ApplyIncludes(postQueryDTO.Includes)
                .ApplyFilters(filterParams)
                .ApplySorting(postQueryDTO.Sort)
                .ApplyPaginationAsync(postQueryDTO.Page, postQueryDTO.PageSize);

            return await postsQuery;
        }

        public async Task<PaginatedResult<Post>> GetAllPostsOnlyFriendByUserIdAsync(string userId, PostQueryDTO postQueryDTO)
        {
            var filterParams = new Dictionary<string, object?>
                    {
                        {"Id", postQueryDTO.Id},
                        {"Content", postQueryDTO.Content },
                        {"CreatorId", postQueryDTO.CreatorId },
                        {"Create_at", postQueryDTO.Create_at }
                    };

            var posts = _dbContext.Posts
                .Where(post => post.CreatorId == userId && (post.Visibility == Visibility.Public || post.Visibility == Visibility.FriendsOnly));
                
            foreach (var post in posts)
            {
                post.Creator = _dbContext.Users.FirstOrDefault(u => u.Id == post.CreatorId);
            }

            var postsQuery = posts
                .ApplyIncludes(postQueryDTO.Includes)
                .ApplyFilters(filterParams)
                .ApplySorting(postQueryDTO.Sort)
                .ApplyPaginationAsync(postQueryDTO.Page, postQueryDTO.PageSize);

            return await postsQuery;
        }

        public async Task<List<Post>> GetAllStoriesOnlyFriendByUserIdAsync(string userId, PostQueryDTO postQueryDTO)
        {
           
            var posts = _dbContext.Posts
                .Where(post => post.CreatorId == userId && 
                        (post.Visibility == Visibility.Public || post.Visibility == Visibility.FriendsOnly) &&
                        post.Is_story == true);

            foreach (var post in posts)
            {
                post.Creator = _dbContext.Users.FirstOrDefault(u => u.Id == post.CreatorId);
            }

            return await posts.ToListAsync();
        }

        public async Task<PaginatedResult<Post>> GetAllPostsByMeAsync(string meId, PostQueryDTO postQueryDTO)
        {
            var filterParams = new Dictionary<string, object?>
                    {
                        {"Id", postQueryDTO.Id},
                        {"Content", postQueryDTO.Content },
                        {"CreatorId", postQueryDTO.CreatorId },
                        {"Create_at", postQueryDTO.Create_at }
                    };

            var posts = _dbContext.Posts
                .Where(post => post.CreatorId == meId);

            foreach (var post in posts)
            {
                post.Creator = _dbContext.Users.FirstOrDefault(u => u.Id == post.CreatorId);
            }

            var postsQuery = posts
                .ApplyIncludes(postQueryDTO.Includes)
                .ApplyFilters(filterParams)
                .ApplySorting(postQueryDTO.Sort)
                .ApplyPaginationAsync(postQueryDTO.Page, postQueryDTO.PageSize);

            return await postsQuery;
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
