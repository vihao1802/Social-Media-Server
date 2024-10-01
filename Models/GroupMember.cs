namespace SocialMediaServer.Models
{
    public class GroupMember
    {
        public int Id { get; set; }
        public int GroupId { get; set; }
        public GroupChat GroupChat { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
        public DateTime Join_at { get; set; } = DateTime.Now;
    }
}
