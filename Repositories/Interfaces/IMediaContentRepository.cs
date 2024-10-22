
using SocialMediaServer.Models;

namespace SocialMediaServer.Repositories.Interfaces
{
    public interface IMediaContentRepository
    {
       Task<List<MediaContent>> GetAllMediaContentAsync();
       Task<List<MediaContent>> GetAllMediaContentByPostIdAsync(int postId);
       Task<MediaContent> GetByIdAsync(int id);
       Task<MediaContent> CreateAsync(MediaContent mediaContent);  
       Task<MediaContent> UpdateAsync(MediaContent mediaContent);
       Task<bool> DeleteAsync(int id);
    }
}