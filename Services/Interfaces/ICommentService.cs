using DTOs.Response;
using SocialMediaServer.DTOs.Request.MediaContent;

namespace SocialMediaServer.Services.Interfaces
{
    public interface ICommentService
    {
        Task<List<CommentResponseDTO>> GetAllAsync();
        Task<List<CommentResponseDTO>> GetAllByPostIdAsync(int postId);
        Task<List<CommentResponseDTO>> GetAllByUserIdAsync(string userId);
        Task<CommentResponseDTO> GetByIdAsync(int id);
        Task<CommentResponseDTO> CreateAsync(CommentCreateDTO commentCreateDTO, IFormFile? mediaFile);
        Task<CommentResponseDTO> UpdateAsync(CommentUpdateDTO commentUpdateDTO, int id, IFormFile? mediaFile);
        Task<CommentResponseDTO> PatchAsync(CommentPatchDTO commentPatchDTO, int id, IFormFile? mediaFile);
        Task<bool> DeleteAsync(int id);
        Task<bool> DeleteAllByPostIdAsync(int postId);
    }
}
