using DTOs.Response;
using SocialMediaServer.DTOs.Response;
using SocialMediaServer.Models;
using System.ComponentModel.DataAnnotations;

namespace SocialMediaServer.DTOs.Request.MediaContent
{
    public class CommentReactionQueryDTO : BaseQueryDTO
    {
        public string? Id { get; set; }
        public string? UserId { get; set; }
        public string? CommentId { get; set; }
        public string? Reaction_at { get; set; }
    }
}