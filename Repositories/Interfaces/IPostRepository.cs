
using Microsoft.AspNetCore.Identity;
using SocialMediaServer.DTOs;
using SocialMediaServer.DTOs.Request.Post;
using SocialMediaServer.Models;
using SocialMediaServer.Utils;

namespace SocialMediaServer.Repositories.Interfaces
{
    public interface IPostRepository
    {
       Task<PaginatedResult<Post>> GetAllPostsAsync(PostQueryDTO postQueryDTO);
       Task<PaginatedResult<Post>> GetAllPostsPublicByUserIdAsync(string userId, PostQueryDTO postQueryDTO);
       Task<List<Post>> GetAllStoriesOnlyFriendByUserIdAsync(string userId, PostQueryDTO postQueryDTO);
       Task<PaginatedResult<Post>> GetAllPostsOnlyFriendByUserIdAsync(string userId, PostQueryDTO postQueryDTO);
       Task<PaginatedResult<Post>> GetAllPostsByMeAsync(string meId, PostQueryDTO postQueryDTO);
       Task<Post> GetByIdAsync(int id);
       Task<Post> CreateAsync(Post post);  
       Task<Post> UpdateAsync(Post post);
       Task<bool> DeleteAsync(int id);
    }
}