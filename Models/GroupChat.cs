namespace SocialMediaServer.Models;
public class GroupChat
{
    public int Id { get; set; }
    public string Group_name { get; set; } = string.Empty;
    public string Group_avt { get; set; } = string.Empty;

    public DateTime Created_at { get; set; } = DateTime.Now;
    public List<GroupMember> Members { get; set; } = new List<GroupMember>();
}