using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SocialMediaServer.DTOs.Response
{
    public class FollowingResponseDTO
    {
        public int Id { get; set; }
        public string ReceiverId { get; set; }

        public UserResponseDTO receiver { get; set; }

        public string RelationshipType { get; set; }
        public string Status { get; set; }

        public DateTime Create_at { get; set; }
    }
}