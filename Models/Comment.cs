namespace SocialMediaServer.Models;
public class Comment
{
    public int Id { get; set; }
    public string Content { get; set; } = string.Empty;
    public string Content_gif { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public string UserId { get; set; }
    public User User { get; set; }
    public int PostId { get; set; }
    public Post Post { get; set; }

    public int? CommentId { get; set; }
    public Comment? ParentComment { get; set; }

    public List<Comment> Replies { get; set; } = new List<Comment>();
    public List<CommentReaction> CommentReactions { get; set; } = new List<CommentReaction>();
}