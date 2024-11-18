using System.ComponentModel.DataAnnotations;

public class MessageReactionCreateDTO
{
    [Required]
    public int GroupMessageId { get; set; }
    [Required]
    public string UserId { get; set; }
    [Required]
    public string ReactionType { get; set; }
}
