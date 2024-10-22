
using SocialMediaServer.Models;

namespace SocialMediaServer.Repositories.Interfaces
{
    public interface IPostViewerRepository
    {
       Task<List<PostViewer>> GetAllByPostIdAsync(int postId);
       Task<PostViewer> CreateByPostIdAsync(PostViewer postViewer);
       Task<PostViewer> GetByIdAsync(int id);
       Task<bool> DeleteByPostidAsync(int id, int postId);
    }
}