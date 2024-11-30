using DTOs.Response;
using SocialMediaServer.DTOs.Request.PostViewer;
using SocialMediaServer.Utils;

namespace SocialMediaServer.Services.Interfaces
{
    public interface IPostViewerService
    {
        Task<PaginatedResult<PostViewerResponseDTO>> GetAllByPostIdAsync(int postId, PostViewerQueryDTO postViewerQueryDTO);
        Task<PostViewerResponseDTO> CreateByPostIdAsync(PostViewerCreateDTO postViewerCreateDTO);

        Task<PostViewerResponseDTO> GetByIdAsync(int id);

        Task<bool> DeleteByIdAsync(int id);
    }
}
