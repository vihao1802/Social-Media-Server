namespace SocialMediaServer.DTOs.Response
{
    public class GroupMemberResponseDTO
    {
        public int Id {get; set;}
        public GroupChatResponseDTO Group {get; set;}
        public UserResponseDTO User {get; set;}
        public DateTime Join_at { get; set; } = DateTime.Now;
    }
}