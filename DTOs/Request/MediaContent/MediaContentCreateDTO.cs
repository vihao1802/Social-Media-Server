using DTOs.Response;
using System.ComponentModel.DataAnnotations;

namespace SocialMediaServer.DTOs.Request.MediaContent
{
    public class MediaContentCreateDTO
    {
        [Required]
        public string Media_type { get; set; } = null!;

        [Required]
        public int PostId { get; set; }

        public override string ToString()
        {
            return $"Media_type: {Media_type}, PostId: {PostId}";
        }
    }


}