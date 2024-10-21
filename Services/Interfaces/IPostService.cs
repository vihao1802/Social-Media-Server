using DTOs.Response;
using SocialMediaServer.DTOs.Request.Post;

namespace SocialMediaServer.Services.Interfaces
{
    public interface IPostService
    {
        Task<List<PostResponseDTO>> GetAllAsync();
        Task<List<PostResponseDTO>> GetAllByUserIdAsync(string userViewId);
        Task<List<PostResponseDTO>> GetAllByMeAsync();
        Task<PostResponseDTO> GetByIdAsync(int id);
        Task<PostResponseDTO> CreateAsync(PostCreateDTO postCreateDTO);
        Task<PostResponseDTO> UpdateAsync(PostUpdateDTO post, int id);
        Task<PostResponseDTO> PatchAsync(PostPatchDTO post, int id);
        Task<bool> DeleteAsync(int id);
    }
}
