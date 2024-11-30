
using SocialMediaServer.DTOs.Request.MediaContent;
using SocialMediaServer.Models;
using SocialMediaServer.Utils;

namespace SocialMediaServer.Repositories.Interfaces
{
    public interface IMediaContentRepository
    {
       Task<PaginatedResult<MediaContent>> GetAllMediaContentAsync(MediaContentQueryDTO mediaContentQueryDTO);
       Task<PaginatedResult<MediaContent>> GetAllMediaContentByPostIdAsync(int postId, MediaContentQueryDTO mediaContentQueryDTO);
       Task<MediaContent> GetByIdAsync(int id);
       Task<MediaContent> CreateAsync(MediaContent mediaContent);  
       Task<MediaContent> UpdateAsync(MediaContent mediaContent);
       Task<bool> DeleteAsync(int id);
    }
}