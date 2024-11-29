
using SocialMediaServer.DTOs.Request.PostViewer;
using SocialMediaServer.Models;
using SocialMediaServer.Utils;

namespace SocialMediaServer.Repositories.Interfaces
{
    public interface IPostViewerRepository
    {
        Task<PaginatedResult<PostViewer>> GetAllByPostIdAsync(int postId, PostViewerQueryDTO postViewerQueryDTO);
        Task<PostViewer> CreateByPostIdAsync(PostViewer postViewer);
        Task<PostViewer> GetByIdAsync(int id);
        Task<bool> DeleteByIdAsync(int id);
    }
}