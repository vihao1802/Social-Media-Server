namespace SocialMediaServer.DTOs.Request.GroupChat
{
    public class GrMessQueryDTO : BaseQueryDTO
    {
        public int? Id {get;set;}
        
        public string? Content {get; set;} = string.Empty;
        
        public string? Media_content {get; set;} = string.Empty;
        
        public int? GroupId {get; set;}
        
        public int? ReplyToId {get; set;}
    }
}