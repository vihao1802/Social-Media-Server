
namespace SocialMediaServer.DTOs.Request.PostViewer
{
    public class PostViewerPatchDTO
    {
        public int? PostId { get; set; }
        public string? UserId { get; set; }
        public bool? Liked { get; set; } = true;
    }
}
