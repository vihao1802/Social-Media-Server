using System.ComponentModel.DataAnnotations;

namespace SocialMediaServer.DTOs.Request.GroupChat
{
    public class GroupCreateDTO
    {
        [Required]
        public string Name {get; set;} = string.Empty;
        public string Avatar {get; set;} = string.Empty;
        [Required]
        public string CreaterId {get; set;} = string.Empty;
        public List<string> MembersId { get; set; } = new List<string>();

        public IFormFile? MediaFile { get; set; }
    }
}