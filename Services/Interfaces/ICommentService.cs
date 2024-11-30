using DTOs.Response;
using SocialMediaServer.DTOs.Request.MediaContent;
using SocialMediaServer.Utils;

namespace SocialMediaServer.Services.Interfaces
{
    public interface ICommentService
    {
        Task<PaginatedResult<CommentResponseDTO>> GetAllAsync(CommentQueryDTO commentQueryDTO);
        Task<PaginatedResult<CommentResponseDTO>> GetAllByPostIdAsync(int postId, CommentQueryDTO commentQueryDTO);
        Task<PaginatedResult<CommentResponseDTO>> GetAllByUserIdAsync(string userId, CommentQueryDTO commentQueryDTO);
        Task<CommentResponseDTO> GetByIdAsync(int id);
        Task<CommentResponseDTO> CreateAsync(CommentCreateDTO commentCreateDTO, IFormFile? mediaFile);
        Task<CommentResponseDTO> UpdateAsync(CommentUpdateDTO commentUpdateDTO, int id, IFormFile? mediaFile);
        Task<CommentResponseDTO> PatchAsync(CommentPatchDTO commentPatchDTO, int id, IFormFile? mediaFile);
        Task<bool> DeleteAsync(int id);
        Task<bool> DeleteAllByPostIdAsync(int postId);
    }
}
