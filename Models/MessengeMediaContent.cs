namespace SocialMediaServer.Models;

public class MessengeMediaContent
{
    public int Id { get; set; }
    public string Media_url { get; set; } = string.Empty;
    public string Media_type { get; set; } = string.Empty;
    public int MessengeId { get; set; }
    public Messenge Messenge { get; set; }
}