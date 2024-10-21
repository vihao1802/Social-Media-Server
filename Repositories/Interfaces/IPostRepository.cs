
using Microsoft.AspNetCore.Identity;
using SocialMediaServer.DTOs;
using SocialMediaServer.Models;

namespace SocialMediaServer.Repositories.Interfaces
{
    public interface IPostRepository
    {
       Task<List<Post>> GetAllPostsAsync();
       Task<List<Post>> GetAllPostsPublicByUserIdAsync(string userId);
       Task<List<Post>> GetAllPostsOnlyFriendByUserIdAsync(string userId);
       Task<List<Post>> GetAllPostsByMeAsync(string meId);
       Task<Post> GetByIdAsync(int id);
       Task<Post> CreateAsync(Post post);  
       Task<Post> UpdateAsync(Post post);
       Task<bool> DeleteAsync(int id);
    }
}