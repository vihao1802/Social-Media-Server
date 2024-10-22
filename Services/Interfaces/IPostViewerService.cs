using DTOs.Response;
using SocialMediaServer.DTOs.Request.PostViewer;

namespace SocialMediaServer.Services.Interfaces
{
    public interface IPostViewerService
    {
        Task<List<PostViewerResponseDTO>> GetAllByPostIdAsync(int postId);
        Task<PostViewerResponseDTO> CreateByPostIdAsync(PostViewerCreateDTO postViewerCreateDTO);

        Task<PostViewerResponseDTO> GetByIdAsync(int id);

        Task<bool> DeleteByPostIdAsync(int id, int postId);
    }
}
