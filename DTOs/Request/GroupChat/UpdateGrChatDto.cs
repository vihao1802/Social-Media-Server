using System.ComponentModel.DataAnnotations;

namespace SocialMediaServer.DTOs.Request.GroupChat
{
    public class UpdateGrChatDTO
    {
        [Required]
        public string Group_name {get; set;} = string.Empty;
        [Required]
        public string Group_avt {get; set;} = string.Empty;
        [Required]
        public DateTime? Created_at {get; set;}
        [Required]
        public string AdminId {get; set;} = string.Empty;
        
    }
}