namespace SocialMediaServer.Models
{
    public class Notification
    {
        public int Id { get; set; }
        public int GroupId { get; set; }          
        public GroupChat Group { get; set; }   
        public string Content { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}
