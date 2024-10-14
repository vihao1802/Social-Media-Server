namespace SocialMediaServer.Models;

public enum RelationshipType
{
    Block,
    Follow,
}

public enum RelationshipStatus
{
    Rejected,
    Pending,
    Accepted,
}
public class Relationship
{
    public int Id { get; set; }
    public string SenderId { get; set; }
    public User Sender { get; set; }
    public string ReceiverId { get; set; }
    public User Receiver { get; set; }
    public required RelationshipType Relationship_type { get; set; }
    public required RelationshipStatus Status { get; set; }
    public DateTime Create_at { get; set; } = DateTime.Now;
}