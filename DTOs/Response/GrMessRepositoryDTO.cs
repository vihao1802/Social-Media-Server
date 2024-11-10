using SocialMediaServer.DTOs.Response;
using SocialMediaServer.Models;

namespace DTOs.Response
{
    public class GrMessResponseDTO
    {
        public int Id { get; set; }

        public string Content { get; set; } = string.Empty;
        public string Media_content { get; set; } = string.Empty;
        public GroupChatResponseDTO GroupChat { get; set; }
        public UserResponseDTO Sender { get; set; }
        public GrMessResponseDTO ReplyTo { get; set; }
        public DateTime Sent_at { get; set; } = DateTime.Now;

        // public List<GrMessResponseDTO> Replies { get; set; } = new List<GrMessResponseDTO>();
    }
}
