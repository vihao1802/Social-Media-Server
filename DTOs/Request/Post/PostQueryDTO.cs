using SocialMediaServer.Models;
using System.ComponentModel.DataAnnotations;

namespace SocialMediaServer.DTOs.Request.Post
{
    public class PostQueryDTO : BaseQueryDTO {
        [RegularExpression(@"^(eq|neq|gt|gte|lt|lte):[\w\s,]*$", ErrorMessage = "Invalid filter format")]
        public string? Id { get; set; }
        [RegularExpression(@"^(eq|neq|like|in):[\w\s,]*$", ErrorMessage = "Invalid filter format")]
        public string? Content { get; set; }

        [RegularExpression(@"^(eq|neq|gt|gte|lt|lte):[\w\s,-]*$", ErrorMessage = "Invalid filter format")]
        public string? Create_at { get; set; }
        [RegularExpression(@"^(eq|neq|gt|gte|lt|lte|like|in):[\w\s,]*$", ErrorMessage = "Invalid filter format")]
        public string? CreatorId { get; set; }
    }
}
