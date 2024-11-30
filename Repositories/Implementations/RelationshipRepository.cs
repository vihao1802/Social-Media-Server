using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SocialMediaServer.Data;
using SocialMediaServer.Models;
using SocialMediaServer.Repositories.Interfaces;

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

        public async Task ChangeRelationshipType(Relationship r)
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


        public async Task<Relationship> GetRelationshipById(int relationshipId){
            var r = await _dbContext.Relationships.FindAsync(relationshipId); 
            if (r == null){
                return null;
            }

            return r;
        }
    }
}