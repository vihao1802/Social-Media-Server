
namespace SocialMediaServer.Models;

public class Post
{
    public int Id { get; set; }
    public string Content { get; set; } = string.Empty;
    public Visibility Visibility { get; set; }
    public bool Is_story { get; set; } = false;
    public DateTime Create_at { get; set; } = DateTime.Now;

    public int CreatorId { get; set; }
    public User Creator { get; set; }


    public List<MediaContent> MediaContents { get; set; } = new List<MediaContent>();
    public List<PostViewer> PostReactions { get; set; } = new List<PostViewer>();
    public List<Comment> Comments { get; set; } = new List<Comment>();
}
public enum Visibility
{
    Public,         // Visible to everyone
    FriendsOnly,    // Visible only to friends
    Private         // Visible only to the post owner
}