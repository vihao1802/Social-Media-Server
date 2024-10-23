

namespace SocialMediaServer.DTOs.Request.MediaContent
{
    public class CommentPatchDTO
    {
        public string? Content { get; set; }
        public string? Content_gif { get; set; }
        public string? UserId { get; set; }
        public int? PostId { get; set; }
        public int? ParentCommentId { get; set; }
    }
}