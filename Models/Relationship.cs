namespace SocialMediaServer.Models;
public class Relationship
{
    public int Id { get; set; }
    public int SenderId { get; set; }
    public User Sender { get; set; }
    public int ReceiverId { get; set; }
    public User Receiver { get; set; }
    public required string Relationship_type { get; set; }  
    public required string Status { get; set; }  
    public DateTime Create_at { get; set; } = DateTime.Now;
}