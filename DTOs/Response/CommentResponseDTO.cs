using SocialMediaServer.DTOs.Response;
using SocialMediaServer.Models;

namespace DTOs.Response
{
    public class CommentResponseDTO
    {
        public int Id { get; set; }
        public string Content { get; set; } = string.Empty;
        public string Content_gif { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public UserResponseDTO User { get; set; }
        public PostResponseDTO Post { get; set; }
        public CommentResponseDTO? ParentComment { get; set; }

    }
}
