using DTOs.Response;
using SocialMediaServer.DTOs.Response;
using System.ComponentModel.DataAnnotations;

namespace SocialMediaServer.DTOs.Request.MediaContent
{
    public class CommentReactionCreateDTO
    {
        [Required]
        public string UserId { get; set; } = string.Empty;
        [Required]
        public int CommentId { get; set; }
    }
}