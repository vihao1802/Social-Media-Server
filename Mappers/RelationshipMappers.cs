using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SocialMediaServer.DTOs.Response;
using SocialMediaServer.Models;

namespace SocialMediaServer.Mappers
{
    public static class RelationshipMappers
    {
        public static FollowingResponseDTO ToFollowingResponseDTO(this Relationship r)
        {
            return new FollowingResponseDTO
            {
                Id = r.Id,
                ReceiverId = r.ReceiverId,
                receiver = r.Receiver.UserToUserResponseDTO(),
                RelationshipType = (Convert.ToByte(r.Relationship_type) == 1) ? "Follow" : "Block",
                Status = (Convert.ToByte(r.Status) == 2) ? "Accepted" : (Convert.ToByte(r.Status) == 1) ? "Pending" : "Rejected",
                Create_at = r.Create_at
            };
        }

        public static FollowerResponseDTO ToFollowerResponseDTO(this Relationship r)
        {
            return new FollowerResponseDTO
            {
                Id = r.Id,
                SenderId = r.SenderId,
                Sender = r.Sender.UserToUserResponseDTO(),
                RelationshipType = (Convert.ToByte(r.Relationship_type) == 1) ? "Follow" : "Block",
                Status = (Convert.ToByte(r.Status) == 2) ? "Accepted" : (Convert.ToByte(r.Status) == 1) ? "Pending" : "Rejected",
                Create_at = r.Create_at
            };
        }
    }
}