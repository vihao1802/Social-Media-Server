namespace SocialMediaServer.DTOs.Request.Notification
{
    public class NotificationQueryDTO : BaseQueryDTO
    {
        public int? Id { get; set; }
        public int? GroupId { get; set; }
        public string? Content { get; set; }
    }
}
