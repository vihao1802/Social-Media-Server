using DTOs.Response;
using SocialMediaServer.DTOs.Request.MediaContent;

namespace SocialMediaServer.Services.Interfaces
{
    public interface IMediaContentService
    {
        Task<List<MediaContentResponseDTO>> GetAllAsync();
        Task<List<MediaContentResponseDTO>> GetAllByPostIdAsync(int postId);
        Task<MediaContentResponseDTO> GetByIdAsync(int id);
        Task<MediaContentResponseDTO> CreateAsync(MediaContentCreateDTO mediaContentCreateDTO, IFormFile mediaFile);
        Task<MediaContentResponseDTO> UpdateAsync(MediaContentUpdateDTO mediaContentUpdateDTO, int id, IFormFile mediaFile);
        Task<MediaContentResponseDTO> PatchAsync(MediaContentPatchDTO mediaContentPatchDTO, int id, IFormFile? mediaFile);
        Task<bool> DeleteAsync(int id);
    }
}
