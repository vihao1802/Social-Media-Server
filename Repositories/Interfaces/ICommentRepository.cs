
using SocialMediaServer.DTOs.Request.MediaContent;
using SocialMediaServer.Models;
using SocialMediaServer.Utils;

namespace SocialMediaServer.Repositories.Interfaces
{
    public interface ICommentRepository
    {
       // L?y t?t c? các comment
       Task<PaginatedResult<Comment>> GetAllCommentAsync(CommentQueryDTO commentQueryDTO);
       // L?y t?t c? các comment c?a m?t post
       Task<PaginatedResult<Comment>> GetAllCommentByPostIdAsync(int postId, CommentQueryDTO commentQueryDTO);
       // L?y t?t c? các comment c?a m?t user
       Task<PaginatedResult<Comment>> GetCommentByUserIdAsync(string userId, CommentQueryDTO commentQueryDTO);
       // L?y m?t comment theo id
       Task<Comment> GetByIdAsync(int id);
       Task<Comment> CreateAsync(Comment comment);
       Task<Comment> UpdateAsync(Comment comment);
       Task<bool> DeleteAsync(int id);
       // Xóa t?t c? các comment c?a m?t post
       Task<bool> DeleteAllByPostIdAsync(int postId);
    }
}