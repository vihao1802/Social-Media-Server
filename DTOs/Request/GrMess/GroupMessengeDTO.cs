namespace SocialMediaServer.DTOs.Request.GroupMess
{
    public class GroupMessengeDTO
    {
        public int GroupId { get; set; }
        public string GroupName { get; set; }
        public List<MessageDTO> Messages { get; set; } = new List<MessageDTO>();
        public List<GroupMemberDTO> LeftMembers { get; set; } = new List<GroupMemberDTO>();
    }

    public class GroupMemberDTO
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public DateTime Join_at { get; set; }
        public DateTime? Left_at { get; set; }
    }

    public class MessageDTO
    {
        public int Id { get; set; }
        public string Content { get; set; }
        public string Media_content { get; set; }
        public DateTime Sent_at { get; set; }
        public string SenderId { get; set; }
    }
}