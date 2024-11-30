using Microsoft.EntityFrameworkCore;
using SocialMediaServer.Data;
using SocialMediaServer.DTOs.Request.MessageReaction;
using SocialMediaServer.ExceptionHandling;
using SocialMediaServer.Models;
using SocialMediaServer.Repositories.Interfaces;
using SocialMediaServer.Utils;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SocialMediaServer.Repositories.Implementations
{
    public class MessageReactionRepository : IMessageReactionRepository
    {
        private readonly ApplicationDBContext _dbContext;

        public MessageReactionRepository(ApplicationDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<PaginatedResult<MessageReaction>> GetAllReactionsAsync(MessageReactionQueryDTO queryDTO)
        {
            var filterParams = new Dictionary<string, object?>
            {
                { "Id", queryDTO.Id },
                { "GroupMessageId", queryDTO.GroupMessageId },
                { "UserId", queryDTO.UserId },
                { "ReactionType", queryDTO.ReactionType }
            };

            var reactions = _dbContext.MessageReactions
                .Include(r => r.GroupMessage)
                .Include(r => r.User);

            var reactionsQuery = reactions
                .ApplyFilters(filterParams)
                .ApplySorting(queryDTO.Sort)
                .ApplyPaginationAsync(queryDTO.Page, queryDTO.PageSize);

            return await reactionsQuery;
        }

        public async Task<MessageReaction> GetByIdAsync(int id)
        {
            var reaction = await _dbContext.MessageReactions
                .Include(r => r.GroupMessage)
                .Include(r => r.User)
                .FirstOrDefaultAsync(r => r.Id == id);

            if (reaction == null)
                throw new AppError("Message reaction not found", 404);

            return reaction;
        }

        public async Task<MessageReaction> CreateAsync(MessageReaction messageReaction)
        {
            _dbContext.MessageReactions.Add(messageReaction);
            await _dbContext.SaveChangesAsync();
            return messageReaction;
        }

        public async Task<MessageReaction> UpdateAsync(MessageReaction messageReaction)
        {
            _dbContext.MessageReactions.Update(messageReaction);
            await _dbContext.SaveChangesAsync();
            return messageReaction;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var reaction = await _dbContext.MessageReactions.FirstOrDefaultAsync(r => r.Id == id);
            if (reaction == null)
                return false;

            _dbContext.MessageReactions.Remove(reaction);
            await _dbContext.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAllByGroupMessageIdAsync(int groupMessageId)
        {
            var reactions = await _dbContext.MessageReactions
                .Where(r => r.GroupMessageId == groupMessageId)
                .ToListAsync();

            if (!reactions.Any())
                return false;

            _dbContext.MessageReactions.RemoveRange(reactions);
            await _dbContext.SaveChangesAsync();
            return true;
        }
    }
}
