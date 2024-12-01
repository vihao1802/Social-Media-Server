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
        Task<List<Relationship>> GetUser_Following_Follower(string user_id);
        Task<List<Relationship>> GetUserBlockList(string user_id);
        Task<Relationship> CreateRelationship(Relationship relationship);
        Task DeleteRelationship(Relationship relationship);
        Task<Relationship> GetRelationshipBetweenSenderAndReceiver(string sender_id, string receiver_id);
        Task ChangeRelationshipType(Relationship r);


        // Sử dụng cho messenge
        Task<Relationship> GetRelationshipById(int relationShipId);
        Task<int> GetFollowingQuantity(string user_id);
        Task<int> GetFollowerQuantity(string user_id);
    }
}