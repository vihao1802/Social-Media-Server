using SocialMediaServer.Models;
using System.ComponentModel.DataAnnotations;

namespace SocialMediaServer.DTOs.Request.Post
{
    public class PostPatchDTO { 
        public string? Content { get; set; }
        public Visibility? Visibility { get; set; }
        public bool? Is_story { get; set; }
        public string? CreatorId { get; set; }
    }
}
