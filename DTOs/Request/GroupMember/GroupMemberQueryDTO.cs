using System.ComponentModel.DataAnnotations;

namespace SocialMediaServer.DTOs.Request.GroupMember
{
    public class GroupMemberQueryDTO : BaseQueryDTO
    {
        
        public int? Id {get; set;}
        [RegularExpression(@"^(eq|neq|gt|gte|lt|lte):[\w\s,]*$", ErrorMessage = "Invalid filter format")]
        public string? GroupId {get; set;}
        [RegularExpression(@"^(eq|neq|gt|gte|lt|lte|like|in):[\w\s,]*$", ErrorMessage = "Invalid filter format")]
        public string? UserId {get; set;}
        [RegularExpression(@"^(eq|neq|gt|gte|lt|lte):[\w\s,-]*$", ErrorMessage = "Invalid filter format")]
        public string? Join_at {get; set;}
    }
}