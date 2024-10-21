using DTOs.Response;
using SocialMediaServer.DTOs.Request.Post;
using SocialMediaServer.DTOs.Response;
using SocialMediaServer.Models;

namespace SocialMediaServer.Mappers
{
    public static class PostMapper
    {
        public static PostResponseDTO PostToPostResponseDTO(this Post post)
        {
            if (post == null)
                throw new ArgumentNullException(nameof(post), "Post cannot be null");

            var creatorDTO = post.Creator != null ? post.Creator.UserToUserResponseDTO()
                                                  : new UserResponseDTO { Id = post.CreatorId };

            return new PostResponseDTO
            {
                Id = post.Id,
                Content = post.Content,
                Visibility = post.Visibility,
                Is_story = post.Is_story,
                Create_at = post.Create_at,
                Creator = creatorDTO
            };
        }

        public static Post PostCreateDTOToPost(this PostCreateDTO postCreateDTO)
        {
            if (postCreateDTO == null)
                throw new ArgumentNullException(nameof(postCreateDTO), "PostCreateDTO cannot be null");

            return new Post
            {
                Content = postCreateDTO.Content,
                Visibility = postCreateDTO.Visibility,
                Is_story = postCreateDTO.Is_story,
                CreatorId = postCreateDTO.CreatorId,
            };
        }
    }
}
