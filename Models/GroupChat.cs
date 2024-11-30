namespace SocialMediaServer.Models;
public class GroupChat
{
    public int Id { get; set; }
    public string Group_name { get; set; } = string.Empty;
    public string Group_avt { get; set; } = string.Empty;
    public string AdminId {get; set;} = string.Empty;
    public bool isDelete {get; set;} = false;
    public DateTime Created_at { get; set; } = DateTime.Now;
    public List<GroupMember> Members { get; set; } = new List<GroupMember>();
    public List<GroupMessenge> Messages { get; set; } = new List<GroupMessenge>();
    public List<Notification> Notifications { get; set; }
}