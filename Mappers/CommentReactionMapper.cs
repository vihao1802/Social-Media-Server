
using DTOs.Response;
using SocialMediaServer.DTOs.Request.MediaContent;
using SocialMediaServer.DTOs.Request.PostViewer;
using SocialMediaServer.DTOs.Response;
using SocialMediaServer.Models;

namespace SocialMediaServer.Mappers
{
    public static class CommentReactionMapper
    {
        public static CommentReactionResponseDTO CommentReactionToCommentReactionResponseDTO(this CommentReaction commentReaction)
        {
            if (commentReaction == null)
                throw new ArgumentNullException(nameof(commentReaction), "Comment Reaction cannot be null");

            var user = commentReaction.User != null ? commentReaction.User.UserToUserResponseDTO()
                                                  : new UserResponseDTO { Id = commentReaction.UserId };

            var comment = commentReaction.Comment != null ? commentReaction.Comment.CommentToCommentResponseDTO()
                                               : new CommentResponseDTO { Id = commentReaction.CommentId };

            return new CommentReactionResponseDTO
            {
                Id = commentReaction.Id,
                commentId = comment.Id,
                userId = user.Id,
                Reaction_at = commentReaction.Reaction_at,
            };
        }

        public static CommentReaction CommentReactionCreateDTOToCommentReaction(this CommentReactionCreateDTO commentReactionCreateDTO)
        {
            if (commentReactionCreateDTO == null)
                throw new ArgumentNullException(nameof(commentReactionCreateDTO), "CommentReactionCreateDTO cannot be null");

            return new CommentReaction
            {
                CommentId = commentReactionCreateDTO.CommentId,
                UserId = commentReactionCreateDTO.UserId
            };
        }
    }
}
