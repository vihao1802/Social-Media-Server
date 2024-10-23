
using SocialMediaServer.Models;

namespace SocialMediaServer.Repositories.Interfaces
{
    public interface ICommentRepository
    {
       // L?y t?t c? các comment
       Task<List<Comment>> GetAllCommentAsync();
       // L?y t?t c? các comment c?a m?t post
       Task<List<Comment>> GetAllCommentByPostIdAsync(int postId);
       // L?y t?t c? các comment c?a m?t user
       Task<List<Comment>> GetCommentByUserIdAsync(string userId);
       // L?y m?t comment theo id
       Task<Comment> GetByIdAsync(int id);
       Task<Comment> CreateAsync(Comment comment);
       Task<Comment> UpdateAsync(Comment comment);
       Task<bool> DeleteAsync(int id);
       // Xóa t?t c? các comment c?a m?t post
       Task<bool> DeleteAllByPostIdAsync(int postId);
    }
}