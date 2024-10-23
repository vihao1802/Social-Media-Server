using SocialMediaServer.DTOs.Response;
using SocialMediaServer.Models;

namespace DTOs.Response
{
    public class CommentReactionResponseDTO
    {
        public int Id { get; set; }
        public UserResponseDTO User { get; set; }
        public CommentResponseDTO Comment { get; set; }
        public DateTime Reaction_at { get; set; }

    }
}
