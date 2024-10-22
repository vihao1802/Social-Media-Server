using SocialMediaServer.DTOs.Response;
using SocialMediaServer.Models;

namespace DTOs.Response
{
    public class MediaContentResponseDTO
    {
        public int Id { get; set; }
        public string Media_type { get; set; }
        public string Media_Url { get; set; }
        public PostResponseDTO Post { get; set; }
    }
}
