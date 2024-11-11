using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SocialMediaServer.DTOs.Request.GroupChat
{
    public class GroupChatCreateDTO
    {
        [Required]
        public string name {get; set;} = string.Empty;
        [Required]
        public string avatar {get; set;} = string.Empty;
        [Required]
        public string AdminId {get; set;} = string.Empty;
    }
}