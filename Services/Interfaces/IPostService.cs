using DTOs.Response;
using SocialMediaServer.DTOs.Request.Post;
using SocialMediaServer.Utils;

namespace SocialMediaServer.Services.Interfaces
{
    public interface IPostService
    {
        Task<PaginatedResult<PostResponseDTO>> GetAllAsync(PostQueryDTO postQueryDTO);
        Task<PaginatedResult<PostResponseDTO>> GetAllByUserIdAsync(string userViewId, PostQueryDTO postQueryDTO);
        Task<List<PostResponseDTO>> GetAllStoryFriendAsync(PostQueryDTO postQueryDTO);
        Task<PaginatedResult<PostResponseDTO>> GetAllByMeAsync(PostQueryDTO postQueryDTO);
        Task<PostResponseDTO> GetByIdAsync(int id);
        Task<PostResponseDTO> CreateAsync(PostCreateDTO postCreateDTO);
        Task<PostResponseDTO> UpdateAsync(PostUpdateDTO post, int id);
        Task<PostResponseDTO> PatchAsync(PostPatchDTO post, int id);
        Task<bool> DeleteAsync(int id);
    }
}
