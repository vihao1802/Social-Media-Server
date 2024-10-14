using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SocialMediaServer.Models;

namespace SocialMediaServer.Repositories.Interfaces
{
    public interface IRelationshipRepository
    {
        Task<List<Relationship>> GetUserFollowing(string user_id);
        Task<List<Relationship>> GetUserFollower(string user_id);
        Task<Relationship> FollowUser(Relationship relationship);
        Task UnfollowUser(Relationship relationship);
        Task<Relationship> GetRelationshipBetweenSenderAndReceiver(string sender_id, string receiver_id);
    }
}