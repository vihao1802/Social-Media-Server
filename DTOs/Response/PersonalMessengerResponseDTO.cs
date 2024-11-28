using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SocialMediaServer.DTOs.Response
{
    public class PersonalMessengerResponseDTO
    {
        // public int Id { get; set; }

        public UserResponseDTO Messenger { get; set; }

        public string SenderId { get; set; }

        public string Latest_message { get; set; }

        public DateTime? Message_created_at { get; set; }
    }
}