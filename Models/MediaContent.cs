
namespace SocialMediaServer.Models;
public class MediaContent
{
    public int Id { get; set; }
    public string Media_type { get; set; }
    public string Media_Url { get; set; }
    public int PostId { get; set; }
    public Post Post { get; set; }
}