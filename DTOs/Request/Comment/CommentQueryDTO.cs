using DTOs.Response;
using SocialMediaServer.DTOs.Response;
using System.ComponentModel.DataAnnotations;

namespace SocialMediaServer.DTOs.Request.MediaContent
{
    public class CommentQueryDTO : BaseQueryDTO
    {
        [RegularExpression(@"^(eq|neq|gt|gte|lt|lte):[\w\s,]*$", ErrorMessage = "Invalid filter format")]
        public string? Id { get; set; }
        [RegularExpression(@"^(eq|neq|gt|gte|lt|lte|like|in):[\w\s,]*$", ErrorMessage = "Invalid filter format")]
        public string? Content { get; set; }
        [RegularExpression(@"^(eq|neq|gt|gte|lt|lte|like|in):[\w\s,]*$", ErrorMessage = "Invalid filter format")]
        public string? Content_gif { get; set; }
        [RegularExpression(@"^(eq|neq|gt|gte|lt|lte|like|in):[\w\s,]*$", ErrorMessage = "Invalid filter format")]
        public string? UserId { get; set; }
        [RegularExpression(@"^(eq|neq|gt|gte|lt|lte):[\w\s,]*$", ErrorMessage = "Invalid filter format")]
        public string? PostId { get; set; }
        [RegularExpression(@"^(eq|neq|gt|gte|lt|lte):[\w\s,]*$", ErrorMessage = "Invalid filter format")]
        public string? ParentCommentId { get; set; }
    }
}