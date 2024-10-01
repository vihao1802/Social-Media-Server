using System.Net.NetworkInformation;

namespace SocialMediaServer.Models;
public class GroupMessenge
{
    public int Id { get; set; }

    public string Content { get; set; } = string.Empty;
    public string Media_content { get; set; } = string.Empty;
    public int GroupChatId { get; set; }
    public GroupChat groupChat { get; set; }

    public int SenderId { get; set; }
    public User Sender { get; set; }

    public int ReplyToId { get; set; }
    public GroupMessenge ReplyTo { get; set; }
    public DateTime Sent_at { get; set; } = DateTime.Now;

    public List<GroupMessenge> Replies { get; set; } = new List<GroupMessenge>();


}