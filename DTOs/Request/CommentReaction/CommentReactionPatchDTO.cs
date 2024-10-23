using DTOs.Response;
using SocialMediaServer.DTOs.Response;
using System.ComponentModel.DataAnnotations;

namespace SocialMediaServer.DTOs.Request.MediaContent
{
    public class CommentReactionPatchDTO
    {
        public string? UserId { get; set; }
        public int? CommentId { get; set; }
    }
}