using DTOs.Response;
using SocialMediaServer.DTOs.Request.MediaContent;
using SocialMediaServer.DTOs.Response;
using SocialMediaServer.Models;

namespace SocialMediaServer.Mappers
{
    public static class CommentMapper
    {
        public static CommentResponseDTO CommentToCommentResponseDTO(this Comment comment)
        {
            if (comment == null)
                throw new ArgumentNullException(nameof(comment), "Comment cannot be null");

            var post = comment.Post != null ? comment.Post.PostToPostResponseDTO()
                                                  : new PostResponseDTO { Id = comment.PostId };

            var user = comment.User != null ? comment.User.UserToUserResponseDTO()
                                            : new UserResponseDTO { Id = comment.UserId };

            var parentComment = comment.ParentComment != null 
                                ?  comment.ParentComment.CommentToCommentResponseDTO()  
                                : null;


            return new CommentResponseDTO
            {
                Id = comment.Id,
                Content = comment.Content,
                Content_gif = comment.Content_gif,
                User = user,
                Post = post,
                CreatedAt = comment.CreatedAt,
                ParentComment = parentComment
            };
        }

        public static Comment CommentCreateDTOToComment(this CommentCreateDTO commentCreateDTO)
        {
            if (commentCreateDTO == null)
                throw new ArgumentNullException(nameof(commentCreateDTO), "CommentCreateDTO cannot be null");

            return new Comment
            {
                Content = commentCreateDTO.Content,
                UserId = commentCreateDTO.UserId,
                PostId = commentCreateDTO.PostId
            };
        }

        public static Comment CommentUpdateDTOToComment(this CommentUpdateDTO commentUpdateDTO, Comment comment)
        {
            if (commentUpdateDTO == null)
                throw new ArgumentNullException(nameof(commentUpdateDTO), "CommentUpdateDTO cannot be null");

            comment.Content = commentUpdateDTO.Content;
            comment.UserId = commentUpdateDTO.UserId;
            comment.PostId = commentUpdateDTO.PostId;

            return comment;
        }

        public static Comment CommentPatchDTOToComment(this CommentPatchDTO commentPatchDTO, Comment comment)
        {
            if (commentPatchDTO == null)
                throw new ArgumentNullException(nameof(commentPatchDTO), "CommentPatchDTO cannot be null");

            if (commentPatchDTO.Content != null)
                comment.Content = commentPatchDTO.Content;

            if (commentPatchDTO.PostId != null)
                comment.PostId = commentPatchDTO.PostId.Value;

            if (commentPatchDTO.UserId != null)
                comment.UserId = commentPatchDTO.UserId;

            return comment;
        }
    }
}
