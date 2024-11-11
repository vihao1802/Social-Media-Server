namespace SocialMediaServer.DTOs.Request.GroupChat
{
    public class GroupChatQueryDTO : BaseQueryDTO
    {
        public string? Id { get; set; }
        public string? Group_name { get; set; }
        public string? Group_avt { get; set; }
        public string? Created_at { get; set; }
        public string? AdminId {get; set;}
        public string? Includes {get; set;}
    }
}