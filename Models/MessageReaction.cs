namespace SocialMediaServer.Models
{
    public class MessageReaction
    {
        public int Id { get; set; }
        public int GroupMessageId { get; set; }
        public GroupMessenge GroupMessage { get; set; }
        public string UserId { get; set; }
        public User User { get; set; }
        public string ReactionType { get; set; } = string.Empty;
        public DateTime ReactedAt { get; set; } = DateTime.Now;
    }
}
