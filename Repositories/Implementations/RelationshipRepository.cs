using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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

        public async Task<Relationship> FollowUser(Relationship relationship)
        {
            var r = await _dbContext.Relationships.AddAsync(relationship);
            await _dbContext.SaveChangesAsync();
            return r.Entity;
        }

        public Task UnfollowUser(Relationship relationship)
        {
            _dbContext.Relationships.Remove(relationship);
            _dbContext.SaveChanges();
            return Task.CompletedTask;
        }

        public async Task<List<Relationship>> GetUserFollowing(string user_id)
        {
            var list_following = await _dbContext.Relationships
            .Include(r => r.Receiver)
            .Where(r => r.Sender.Id.Equals(user_id))
            .ToListAsync();

            return list_following;
        }

        public async Task<List<Relationship>> GetUserFollower(string user_id)
        {
            var list_following = await _dbContext.Relationships
            .Include(r => r.Sender)
            .Where(r => r.Receiver.Id.Equals(user_id))
            .ToListAsync();

            return list_following;
        }
    }
}