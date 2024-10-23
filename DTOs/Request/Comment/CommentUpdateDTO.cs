using DTOs.Response;
using SocialMediaServer.DTOs.Response;
using System.ComponentModel.DataAnnotations;

namespace SocialMediaServer.DTOs.Request.MediaContent
{
    public class CommentUpdateDTO
    {
        [Required]
        public string Content { get; set; } = string.Empty;
        public string? Content_gif { get; set; }
        [Required]
        public string UserId { get; set; } = string.Empty;
        [Required]
        public int PostId { get; set; }
        public int? ParentCommentId { get; set; }
    }
}