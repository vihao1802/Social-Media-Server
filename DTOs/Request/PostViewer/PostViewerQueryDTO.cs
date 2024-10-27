using System.ComponentModel.DataAnnotations;

namespace SocialMediaServer.DTOs.Request.PostViewer
{
    public class PostViewerQueryDTO : BaseQueryDTO
    {
        [RegularExpression(@"^(eq|neq|gt|gte|lt|lte):[\w\s,]*$", ErrorMessage = "Invalid filter format")]
        public string? Id { get; set; }
        [RegularExpression(@"^(eq|neq|gt|gte|lt|lte):[\w\s,]*$", ErrorMessage = "Invalid filter format")]
        public string? PostId { get; set; }
        [RegularExpression(@"^(eq|neq|gt|gte|lt|lte|like|in):[\w\s,]*$", ErrorMessage = "Invalid filter format")]
        public string? UserId { get; set; }
    }
}
