using DTOs.Response;
using SocialMediaServer.DTOs.Request.Post;
using SocialMediaServer.ExceptionHandling;
using SocialMediaServer.Mappers;
using SocialMediaServer.Models;
using SocialMediaServer.Repositories.Interfaces;
using SocialMediaServer.Services.Interfaces;
using System.Security.Claims;

namespace SocialMediaServer.Services.Implementations
{
    public class PostService : IPostService
    {
        private readonly IPostRepository _postRepository;
        private readonly IUserRepository _userRepository;
        private readonly IRelationshipRepository _relationshipRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public PostService(IPostRepository postRepository, IUserRepository userRepository, 
            IHttpContextAccessor httpContextAccessor, IRelationshipRepository relationshipRepository)
        {
            _postRepository = postRepository;
            _userRepository = userRepository;
            _httpContextAccessor = httpContextAccessor;
            _relationshipRepository = relationshipRepository;
        }

        public async Task<List<PostResponseDTO>> GetAllAsync()
        {
            var posts = await _postRepository.GetAllPostsAsync();
            var listPostsDto = posts.Select(post => post.PostToPostResponseDTO());
            return listPostsDto.ToList();
        }

        public async Task<List<PostResponseDTO>> GetAllByMeAsync()
        {
            var userLogin = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            List<PostResponseDTO> listPostsDto;
           
            if (userLogin != null)
            {
                var posts = await _postRepository.GetAllPostsByMeAsync(userLogin);
                listPostsDto = posts.Select(post => post.PostToPostResponseDTO()).ToList();
            }    
            else
            {
                throw new AppError("You are not authorized to see this content!", 401);
            }
            
            return listPostsDto;
        }

        public async Task<List<PostResponseDTO>> GetAllByUserIdAsync(string userViewId)
        {
            var userId = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (userId == null)
                throw new AppError("You are not authorized to see this content", 401);

            var relationshipUserView_User = await _relationshipRepository.GetRelationshipBetweenSenderAndReceiver(userId, userViewId);
            var relationshipUser_UserView = await _relationshipRepository.GetRelationshipBetweenSenderAndReceiver(userViewId, userId);

            List<PostResponseDTO> listPostsDto;
            List<Post> posts;
            if (relationshipUserView_User == null && relationshipUser_UserView == null)
            {
                posts = await _postRepository.GetAllPostsPublicByUserIdAsync(userViewId);
                listPostsDto = posts.Select(post => post.PostToPostResponseDTO()).ToList();
            }    
            else 
                if (relationshipUserView_User?.Relationship_type == RelationshipType.Block ||
                    relationshipUser_UserView.Relationship_type == RelationshipType.Block)
                {
                    throw new AppError("You are not authorized to see this content", 401);
                }
                else
                if (relationshipUser_UserView.Relationship_type == RelationshipType.Follow &&
                    relationshipUserView_User?.Relationship_type == RelationshipType.Follow)
                {
                    posts = await _postRepository.GetAllPostsOnlyFriendByUserIdAsync(userViewId);
                    listPostsDto = posts.Select(post => post.PostToPostResponseDTO()).ToList();
                }
                else 
                {
                    posts = await _postRepository.GetAllPostsPublicByUserIdAsync(userViewId);
                    listPostsDto = posts.Select(post => post.PostToPostResponseDTO()).ToList(); 
                }

            return listPostsDto;
        }

        public async Task<PostResponseDTO> GetByIdAsync(int id)
        {
            var post = await _postRepository.GetByIdAsync(id);

            if (post == null)
                throw new AppError("Post not found!", 404);

            return post.PostToPostResponseDTO();
        }

        public async Task<PostResponseDTO> CreateAsync(PostCreateDTO postCreateDTO)
        {
            var user = await _userRepository.GetUserById(postCreateDTO.CreatorId);

            if (user == null)
                throw new AppError("User not found!", 404);

            var post = postCreateDTO.PostCreateDTOToPost();
            post.Creator = user;

            var createdPost = await _postRepository.CreateAsync(post);

            return createdPost.PostToPostResponseDTO();
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var post = await _postRepository.GetByIdAsync(id);

            if (post == null)
                throw new AppError("Post not found!", 404);
            
            await CheckPermissionAsync(post.CreatorId);

            return await _postRepository.DeleteAsync(id);
        }

        public async Task<PostResponseDTO> UpdateAsync(PostUpdateDTO post, int id)
        {
            var postToUpdate = await _postRepository.GetByIdAsync(id);

            if (postToUpdate == null)
                throw new AppError("Post not found!", 404);

            await CheckPermissionAsync(postToUpdate.CreatorId);

            var user = await _userRepository.GetUserById(post.CreatorId);

            if (user == null)
                throw new AppError("User not found!", 404);

            postToUpdate.Content = post.Content;
            postToUpdate.Visibility = post.Visibility;
            postToUpdate.Is_story = post.Is_story;
            postToUpdate.Creator = user;

            var updatedPost = await _postRepository.UpdateAsync(postToUpdate);

            return updatedPost.PostToPostResponseDTO();
        }

        public async Task<PostResponseDTO> PatchAsync(PostPatchDTO post, int id)
        {
            var postToUpdate = await _postRepository.GetByIdAsync(id);

            if (postToUpdate == null)
                throw new AppError("Post not found!", 404);

            await CheckPermissionAsync(postToUpdate.CreatorId);

            if (post.CreatorId != null)
            {
                var user = await _userRepository.GetUserById(post.CreatorId);
                if (user == null)
                    throw new AppError("User not found!", 404);
                postToUpdate.Creator = user;
            }

            if (!string.IsNullOrEmpty(post.Content))
                postToUpdate.Content = post.Content;

            if (post.Visibility != null)
                postToUpdate.Visibility = (Visibility) post.Visibility;

            if (post.Is_story != null)
                postToUpdate.Is_story = (bool) post.Is_story;


            var updatedPost = await _postRepository.UpdateAsync(postToUpdate);

            return updatedPost.PostToPostResponseDTO();
        }

        private async Task<User> GetCurrentUserAsync()
        {
            var userId = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId == null)
                throw new AppError("You are not authorized", 401);

            var user = await _userRepository.GetUserById(userId);
            if (user == null)
                throw new AppError("User login not found", 404);

            return user;
        }

        private async Task CheckPermissionAsync(string creatorId)
        {
            var user = await GetCurrentUserAsync();
            var roles = await _userRepository.GetUsersRoles(user);

            if (creatorId != user.Id && !roles.Contains("Admin"))
                throw new AppError("You do not have permission to perform this action", 401);
        }
    }
}
