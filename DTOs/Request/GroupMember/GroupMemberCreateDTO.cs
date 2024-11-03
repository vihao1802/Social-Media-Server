using System.ComponentModel.DataAnnotations;

namespace SocialMediaServer.DTOs.Request.GroupMember
{
    public class GroupMemberCreateDTO
    {
        [Required]
        public int GroupId {get; set;}
        [Required]
        public string UserId {get; set;} = string.Empty;

    }
}