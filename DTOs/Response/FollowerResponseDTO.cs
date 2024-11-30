using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SocialMediaServer.DTOs.Response
{
    public class FollowerResponseDTO
    {
        public int Id { get; set; }
        public string SenderId { get; set; }
        public UserResponseDTO Sender { get; set; }

        public string RelationshipType { get; set; }
        public string Status { get; set; }

        public DateTime Create_at { get; set; }
    }
}