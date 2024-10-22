using DTOs.Response;

namespace SocialMediaServer.DTOs.Request.MediaContent
{
    public class MediaContentPatchDTO
    {
        public string? Media_type { get; set; }
        public int? PostId { get; set; }
    }
}