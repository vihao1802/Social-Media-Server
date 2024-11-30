using DTOs.Response;
using System.ComponentModel.DataAnnotations;

namespace SocialMediaServer.DTOs.Request.MediaContent
{
    public class MediaContentQueryDTO : BaseQueryDTO
    {
        [RegularExpression(@"^(eq|neq|gt|gte|lt|lte):[\w\s,]*$", ErrorMessage = "Invalid filter format")]
        public string? Id { get; set; }
        [RegularExpression(@"^(eq|neq|like|in):[\w\s,]*$", ErrorMessage = "Invalid filter format")]
        public string? Media_type { get; set; }
        [RegularExpression(@"^(eq|neq|like|in):[\w\s,]*$", ErrorMessage = "Invalid filter format")]
        public string? Media_Url { get; set; }
        [RegularExpression(@"^(eq|neq|gt|gte|lt|lte):[\w\s,]*$", ErrorMessage = "Invalid filter format")]
        public string? PostId { get; set; }
    }
}