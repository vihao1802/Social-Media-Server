namespace SocialMediaServer.Models
{
    public class GroupMember
    {
        public int Id { get; set; }
        public int GroupChatId { get; set; }
        public GroupChat GroupChat { get; set; }
        public string UserId { get; set; }
        public User User { get; set; }
        public DateTime Join_at { get; set; } = DateTime.Now;
    }
}
