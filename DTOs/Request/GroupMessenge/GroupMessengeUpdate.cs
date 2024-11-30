using System.ComponentModel.DataAnnotations;

namespace SocialMediaServer.DTOs.Request.GroupMess
{
    public class GrMessUpdateDTO
    {
        [Required]
        public string Content {get; set;} = string.Empty;
        [Required]
        public string MediaContent {get; set;} = string.Empty;
        [Required]
        public int GroupId {get; set;}
        [Required]
        public int ReplyToId {get; set;}
    }
}