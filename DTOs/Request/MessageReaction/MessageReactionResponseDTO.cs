using SocialMediaServer.DTOs.Response;

public class MessageReactionResponseDTO
{
    public int Id {get;set;}
    public int GroupMessageId { get; set; }
    public UserResponseDTO User { get; set; }
    public string ReactionType { get; set; }
    public DateTime ReactedAt { get; set; }
}