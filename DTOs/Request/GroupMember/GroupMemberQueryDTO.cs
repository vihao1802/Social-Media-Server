namespace SocialMediaServer.DTOs.Request.GroupMember
{
    public class GroupMemberQueryDTO : BaseQueryDTO
    {
        public int? Id {get; set;}
        public int? GroupId {get; set;}
        public string? UserId {get; set;} = string.Empty;
        public string? Join_at {get; set;} = string.Empty;
    }
}