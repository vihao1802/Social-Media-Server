namespace SocialMediaServer.Models;
public class Messenge
{
    public int id { get; set; }
    public string Content { get; set; } = string.Empty;

    public DateTime Sent_at { get; set; } = DateTime.Now;

    public string SenderId { get; set; }
    public User Sender { get; set; }

    public string ReceiverId { get; set; }
    public User Receiver { get; set; }

    public int ReplyToId { get; set; }
    public Messenge ReplyTo { get; set; }

    public List<Messenge> Replies { get; set; } = new List<Messenge>();

    public List<MessengeMediaContent> MediaContents { get; set; } = new List<MessengeMediaContent>();

}