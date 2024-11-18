using System.ComponentModel.DataAnnotations;

namespace SocialMediaServer.DTOs.Request.GroupMess
{
    public class GrMessCreateDTO
{
    [Required]
    public string Content { get; set; } = string.Empty;
    public string? MediaContent { get; set; }
    [Required]
    public int GroupId { get; set; }
    public int? ReplyToId { get; set; } // Cho phép null
    [Required]
    public string SenderId { get; set; } = string.Empty;
    public IFormFile? MediaFile { get; set; }
}
}