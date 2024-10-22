using System.ComponentModel.DataAnnotations;

namespace SocialMediaServer.DTOs.Request.PostViewer
{
    public class PostViewerUpdateDTO
    {
        [Required]
        public int PostId { get; set; }
        [Required]
        public string UserId { get; set; }
        [Required]
        public bool Liked { get; set; } = true;
    }
}
