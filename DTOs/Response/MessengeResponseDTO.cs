using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SocialMediaServer.Models;

namespace SocialMediaServer.DTOs.Response
{
    public class MessengeResponseDTO
    {
        public string Content { get; set; } = string.Empty;
        public DateTime Sent_at { get; set; } = DateTime.Now;
        public string SenderId { get; set; }
        public UserResponseDTO Sender { get; set; }
        public string ReceiverId { get; set; }
        public UserResponseDTO receiver { get; set; }
        public int? ReplyToId { get; set; }
        public List<MessengeMediaContent> MediaContents { get; set; } = new List<MessengeMediaContent>();
    }
}