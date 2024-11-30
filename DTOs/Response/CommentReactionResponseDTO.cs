using SocialMediaServer.DTOs.Response;
using SocialMediaServer.Models;

namespace DTOs.Response
{
    public class CommentReactionResponseDTO
    {
        public int Id { get; set; }
        public string userId { get; set; }
        public int commentId { get; set; }
        public DateTime Reaction_at { get; set; }

    }
}
