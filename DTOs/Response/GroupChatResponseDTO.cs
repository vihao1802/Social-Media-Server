using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SocialMediaServer.DTOs.Response
{
    public class GroupChatResponseDTO
    {
        public int Id {get; set;}
        public string name {get;set;} = string.Empty;
        public string avatar {get; set;} = string.Empty;
        public string adminId {get; set;} = string.Empty;
        public DateTime sent_at {get; set;}
        public string latest_message_content {get; set;} = string.Empty;
        public UserResponseDTO latest_message_sender {get; set;}
    }
}