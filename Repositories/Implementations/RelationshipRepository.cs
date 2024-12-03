using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SocialMediaServer.Data;
using SocialMediaServer.DTOs.Request;
using SocialMediaServer.DTOs.Response;
using SocialMediaServer.Models;
using SocialMediaServer.Repositories.Interfaces;
using SocialMediaServer.Utils;

namespace SocialMediaServer.Repositories.Implementations
{
    public class RelationshipRepository : IRelationshipRepository
    {
        private readonly ApplicationDBContext _dbContext;
        private readonly UserManager<User> _userManager;
        public RelationshipRepository(ApplicationDBContext dbContext, UserManager<User> userManager)
        {
            _dbContext = dbContext;
            _userManager = userManager;
        }

        public async Task<Relationship?> GetRelationshipBetweenSenderAndReceiver(string sender_id, string receiver_id)
        {
            var r = await _dbContext.Relationships
            .Include(r => r.Sender)
            .Include(r => r.Receiver)
            .Where(r => r.SenderId.Equals(sender_id) && r.ReceiverId.Equals(receiver_id))
            .FirstOrDefaultAsync();

            return r;

        }

        public async Task DeleteRelationship(Relationship relationship)
        {
            _dbContext.Relationships.Remove(relationship);
            await _dbContext.SaveChangesAsync();
            return;
        }

        public async Task<List<Relationship>> GetUserFollowing(string user_id)
        {
            var list_following = await _dbContext.Relationships
            .Include(r => r.Receiver)
            .Where(r => r.Sender.Id.Equals(user_id) && r.Relationship_type == RelationshipType.Follow)
            .ToListAsync();

            return list_following;
        }

        public async Task<List<Relationship>> GetUser_Following_Follower(string user_id)
        {
            var list_following_follower = await _dbContext.Relationships
            .Include(r => r.Receiver)
            .Include(r => r.Sender)
            .Where(r => (r.Sender.Id == user_id || r.Receiver.Id == user_id) &&
            r.Relationship_type == RelationshipType.Follow && r.Status == RelationshipStatus.Accepted)
            .ToListAsync();

            return list_following_follower;
        }

        public async Task<List<Relationship>> GetUserFollower(string user_id)
        {
            var list_following = await _dbContext.Relationships
            .Include(r => r.Sender)
            .Where(r => r.Receiver.Id.Equals(user_id) && r.Relationship_type == RelationshipType.Follow)
            .ToListAsync();

            return list_following;
        }

        public async Task<List<Relationship>> GetUserBlockList(string user_id)
        {
            var block_list = await _dbContext.Relationships
            .Include(r => r.Receiver)
            .Where(r => r.Sender.Id.Equals(user_id) && r.Relationship_type == RelationshipType.Block)
            .ToListAsync();

            return block_list;
        }

        public async Task UpdateRelationship(Relationship r)
        {
            _dbContext.Relationships.Update(r);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<Relationship> CreateRelationship(Relationship relationship)
        {
            var r = await _dbContext.Relationships.AddAsync(relationship);
            await _dbContext.SaveChangesAsync();
            return r.Entity;
        }


        public async Task<Relationship> GetRelationshipById(int relationshipId)
        {
            var r = await _dbContext.Relationships.FindAsync(relationshipId);
            if (r == null)
            {
                return null;
            }

            return r;
        }

        public async Task<int> GetNumberOfFollowing(string user_id)
        {
            var quantity = await _dbContext.Relationships
            .Where(r => r.Sender.Id.Equals(user_id) && r.Relationship_type == RelationshipType.Follow && r.Status == RelationshipStatus.Accepted)
            .CountAsync();

            return quantity;
        }

        public async Task<int> GetNumberOfFollower(string user_id)
        {
            var quantity = await _dbContext.Relationships
            .Where(r => r.Receiver.Id.Equals(user_id) && r.Relationship_type == RelationshipType.Follow && r.Status == RelationshipStatus.Accepted)
            .CountAsync();

            return quantity;
        }

        // SELECT u.Id, u.UserName, COUNT(r1.ReceiverId) AS MutualFriends
        // FROM AspNetUsers u
        // JOIN Relationships r1 ON u.Id = r1.ReceiverId
        // JOIN Relationships r2 ON r1.SenderId= r2.ReceiverId
        // WHERE r2.SenderId = '51304637-5F13-44D1-B06B-8C406EC5FA01'
        //   AND u.Id NOT IN (
        //       SELECT ReceiverId FROM Relationships WHERE SenderId = '51304637-5F13-44D1-B06B-8C406EC5FA01'
        //   )
        // GROUP BY u.Id, u.UserName
        // ORDER BY MutualFriends DESC


        public async Task<PaginatedResult<RecommendationResponseDTO>> GetRecommendation(string userId, RecommendationQueryDTO recommendationQueryDTO)
        {

            // Lấy danh sách ReceiverId đã được SenderId gửi yêu cầu
            var excludedIds = _dbContext.Relationships
                .Where(r => r.SenderId == userId)
                .Select(r => r.ReceiverId);

            // Truy vấn chính
            var recommendations = _userManager.Users
                .Where(u => !excludedIds.Contains(u.Id)) // Loại trừ những người đã có quan hệ
                .Select(u => new RecommendationResponseDTO
                {
                    Id = u.Id,
                    UserName = u.UserName,
                    MutualFriends = _dbContext.Relationships
                        .Where(r1 => r1.ReceiverId == u.Id)
                        .Join(_dbContext.Relationships,
                            r1 => r1.SenderId,
                            r2 => r2.ReceiverId,
                            (r1, r2) => r2) // Join Relationships r1 với r2
                        .Count(r2 => r2.SenderId == userId) // Đếm số bạn chung
                })
                .OrderByDescending(x => x.MutualFriends);


            var recommendationQuery = recommendations
                .ApplyPaginationAsync(recommendationQueryDTO.Page, recommendationQueryDTO.PageSize);

            return await recommendationQuery;
        }
    }
}