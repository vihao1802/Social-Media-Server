using DTOs.Response;
using SocialMediaServer.DTOs.Request.MediaContent;
using SocialMediaServer.DTOs.Response;
using SocialMediaServer.Models;

namespace SocialMediaServer.Mappers
{
    public static class ReactionMessMapper
    {
        public static MessageReactionResponseDTO MessageReactionToMessageReactionResponseDTO(this MessageReaction messReaction)
        {
            if (messReaction == null)
                throw new ArgumentNullException(nameof(messReaction), "MessReaction cannot be null");

            var user = messReaction.User != null ? messReaction.User.UserToUserResponseDTO()
                                            : new UserResponseDTO { Id = messReaction.UserId };


            return new MessageReactionResponseDTO
            {
                Id = messReaction.Id,
                GroupMessageId = messReaction.GroupMessageId,
                User = user,
                ReactionType = messReaction.ReactionType,
                ReactedAt = messReaction.ReactedAt
            };
        }

        
    }
}
