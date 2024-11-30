using SocialMediaServer.DTOs.Response;
using SocialMediaServer.Models;

namespace DTOs.Response
{
    public class PostViewerResponseDTO
    {
        public int Id { get; set; }
        public string userId { get; set; }
        public int postId { get; set; }
        public bool Liked { get; set; }
    }
}
