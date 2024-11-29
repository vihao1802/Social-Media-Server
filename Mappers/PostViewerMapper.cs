
using DTOs.Response;
using SocialMediaServer.DTOs.Request.PostViewer;
using SocialMediaServer.DTOs.Response;
using SocialMediaServer.Models;

namespace SocialMediaServer.Mappers
{
    public static class PostViewerMapper
    {
        public static PostViewerResponseDTO PostViewerToPostViewerResponseDTO(this PostViewer postViewer)
        {
            if (postViewer == null)
                throw new ArgumentNullException(nameof(postViewer), "PostViewer cannot be null");

            var user = postViewer.User != null ? postViewer.User.UserToUserResponseDTO()
                                                  : new UserResponseDTO { Id = postViewer.UserId };

            var post = postViewer.Post != null ? postViewer.Post.PostToPostResponseDTO()
                                               : new PostResponseDTO { Id = postViewer.PostId };

            return new PostViewerResponseDTO
            {
                Id = postViewer.Id,
                postId = post.Id,
                userId = user.Id,
                Liked = postViewer.Liked
            };
        }

        public static PostViewer PostViewerCreateDTOToPostViewer(this PostViewerCreateDTO postViewerCreateDTO)
        {
            if (postViewerCreateDTO == null)
                throw new ArgumentNullException(nameof(postViewerCreateDTO), "PostViewerCreateDTO cannot be null");

            return new PostViewer
            {
                PostId = postViewerCreateDTO.PostId,
                UserId = postViewerCreateDTO.UserId,
                Liked = postViewerCreateDTO.Liked
            };
        }
    }
}
