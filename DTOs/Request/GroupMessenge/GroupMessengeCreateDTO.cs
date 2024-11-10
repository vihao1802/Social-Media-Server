using System.ComponentModel.DataAnnotations;

namespace SocialMediaServer.DTOs.Request.GroupMess
{
    public class GrMessCreateDTO
    {
        [Required]
        public string Content {get; set;} = string.Empty;
        [Required]
        public string MediaContent {get; set;} = string.Empty;
        [Required]
        public int GroupId {get; set;}
        [Required]
        public int ReplyToId {get; set;}
        [Required]
        public string SenderId {get; set;} = string.Empty;
        public IFormFile? MediaFile { get; set; }
    }
}