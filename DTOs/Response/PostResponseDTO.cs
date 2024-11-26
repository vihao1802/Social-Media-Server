using SocialMediaServer.DTOs.Response;
using SocialMediaServer.Models;

namespace DTOs.Response
{
    public class PostResponseDTO
    {
        public int Id { get; set; }
        public string Content { get; set; } = string.Empty;
        public Visibility Visibility { get; set; }
        public bool Is_story { get; set; } = false;
        public DateTime Create_at { get; set; } = DateTime.Now;
        public UserResponseDTO Creator { get; set; }

        public int PostReactions { get; set; }
    }
}
