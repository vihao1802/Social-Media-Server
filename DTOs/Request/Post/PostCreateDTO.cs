using SocialMediaServer.Models;
using System.ComponentModel.DataAnnotations;

namespace SocialMediaServer.DTOs.Request.Post
{
    public class PostCreateDTO { 
        [Required]
        public string Content { get; set; } = string.Empty;
        [Required]
        public Visibility Visibility { get; set; }
        [Required]
        public bool Is_story { get; set; } = false;
        [Required]
        public string CreatorId { get; set; }
    }
}
