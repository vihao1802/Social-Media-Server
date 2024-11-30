using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SocialMediaServer.Models;

namespace SocialMediaServer.DTOs.Request
{
    public class MessengeDTO
    {
        public string? Content { get; set;}
        public int? ReplyToId { get; set; } = null;
        public List<IFormFile>? files {get; set; } = null;
    }
}