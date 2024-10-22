using SocialMediaServer.DTOs.Response;
using SocialMediaServer.Models;

namespace DTOs.Response
{
    public class PostViewerResponseDTO
    {
        public int Id { get; set; }
        public PostResponseDTO Post { get; set; }
        public UserResponseDTO User { get; set; }
        public bool Liked { get; set; }
    }
}
