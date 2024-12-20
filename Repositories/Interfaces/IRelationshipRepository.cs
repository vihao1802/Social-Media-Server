using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SocialMediaServer.DTOs.Request;
using SocialMediaServer.DTOs.Response;
using SocialMediaServer.Models;
using SocialMediaServer.Utils;

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
        Task UpdateRelationship(Relationship r);


        // Sử dụng cho messenge
        Task<Relationship> GetRelationshipById(int relationShipId);
        Task<int> GetNumberOfFollowing(string user_id);
        Task<int> GetNumberOfFollower(string user_id);

        Task<PaginatedResult<RecommendationResponseDTO>> GetRecommendation(string userId, RecommendationQueryDTO recommendationQueryDTO);
    }
}