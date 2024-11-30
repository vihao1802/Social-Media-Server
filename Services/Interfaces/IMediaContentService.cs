using DTOs.Response;
using SocialMediaServer.DTOs.Request.MediaContent;
using SocialMediaServer.Utils;

namespace SocialMediaServer.Services.Interfaces
{
    public interface IMediaContentService
    {
        Task<PaginatedResult<MediaContentResponseDTO>> GetAllAsync(MediaContentQueryDTO mediaContentQueryDTO);
        Task<PaginatedResult<MediaContentResponseDTO>> GetAllByPostIdAsync(int postId, MediaContentQueryDTO mediaContentQueryDTO);
        Task<MediaContentResponseDTO> GetByIdAsync(int id);
        Task<MediaContentResponseDTO> CreateAsync(MediaContentCreateDTO mediaContentCreateDTO, IFormFile mediaFile);
        Task<MediaContentResponseDTO> UpdateAsync(MediaContentUpdateDTO mediaContentUpdateDTO, int id, IFormFile mediaFile);
        Task<MediaContentResponseDTO> PatchAsync(MediaContentPatchDTO mediaContentPatchDTO, int id, IFormFile? mediaFile);
        Task<bool> DeleteAsync(int id);
    }
}
