namespace SocialMediaServer.Models;
public class PostViewer
{
    public int Id { get; set; }
    public int PostId { get; set; }
    public Post Post { get; set; }
    public string UserId { get; set; }
    public User User { get; set; }
    public bool Liked { get; set; }
}