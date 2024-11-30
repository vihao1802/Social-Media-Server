namespace SocialMediaServer.DTOs.Request.MessageReaction
{
    public class MessageReactionQueryDTO : BaseQueryDTO
    {
        public int? Id { get; set; }
        public int? GroupMessageId { get; set; }
        public string? UserId { get; set; }
        public string? ReactionType { get; set; }
    }
}
