using DTOs.Response;
using Microsoft.EntityFrameworkCore;
using SocialMediaServer.Data;
using SocialMediaServer.DTOs.Request.Post;
using SocialMediaServer.ExceptionHandling;
using SocialMediaServer.Mappers;
using SocialMediaServer.Models;
using SocialMediaServer.Repositories.Interfaces;
using SocialMediaServer.Services.Interfaces;
using SocialMediaServer.Utils;
using System.Security.Claims;

namespace SocialMediaServer.Services.Implementations
{
    public class PostService : IPostService
    {
        private readonly IPostRepository _postRepository;
        private readonly IUserRepository _userRepository;
        private readonly IRelationshipRepository _relationshipRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ApplicationDBContext _dBContext;

        public PostService(IPostRepository postRepository, IUserRepository userRepository,
            IHttpContextAccessor httpContextAccessor, IRelationshipRepository relationshipRepository,
            ApplicationDBContext dBContext)
        {
            _postRepository = postRepository;
            _userRepository = userRepository;
            _httpContextAccessor = httpContextAccessor;
            _relationshipRepository = relationshipRepository;
            _dBContext = dBContext;
        }

        public async Task<PaginatedResult<PostResponseDTO>> GetAllAsync(PostQueryDTO postQueryDTO)
        {
            var posts = await _postRepository.GetAllPostsAsync(postQueryDTO);

            var listPostsDto = posts.Items.Select(post => post.PostToPostResponseDTO()).ToList();

            var postIds = listPostsDto.Select(postDto => postDto.Id).ToList();

            // Truy vấn số lượng PostReactions cho tất cả bài viết trong một lần
            var postReactions = await _dBContext.PostViewers
                .Where(pv => postIds.Contains(pv.PostId))
                .GroupBy(pv => pv.PostId)
                .Select(group => new { PostId = group.Key, ReactionCount = group.Count() })
                .ToDictionaryAsync(x => x.PostId, x => x.ReactionCount);

            // Gán PostReactions vào từng PostResponseDTO
            foreach (var postDto in listPostsDto)
            {
                postDto.PostReactions = postReactions.ContainsKey(postDto.Id) ? postReactions[postDto.Id] : 0;
            }

            return new PaginatedResult<PostResponseDTO>(
                listPostsDto,
                posts.TotalItems,
                posts.Page,
                posts.PageSize);
        }

        public async Task<PaginatedResult<PostResponseDTO>> GetAllByMeAsync(PostQueryDTO postQueryDTO)
        {
            var userLogin = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            PaginatedResult<PostResponseDTO> listPostsDto;

            if (userLogin != null)
            {
                var posts = await _postRepository.GetAllPostsByMeAsync(userLogin, postQueryDTO);
                listPostsDto = new PaginatedResult<PostResponseDTO>(
                    posts.Items.Select(post => post.PostToPostResponseDTO()).ToList(),
                    posts.TotalItems,
                    posts.Page,
                    posts.PageSize);
            }
            else
            {
                throw new AppError("You are not authorized to see this content!", 401);
            }

            return listPostsDto;
        }

        public async Task<List<PostResponseDTO>> GetAllStoryFriendAsync(PostQueryDTO postQueryDTO)
        {
            var userLogin = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userLogin))
                throw new AppError("You are not authorized to see this content!", 401);

            var listUserFollower = await _relationshipRepository.GetUserFollowing(userLogin);

            List<Post> postFriends = new List<Post>();

            foreach (var user in listUserFollower)
            {
                if (user != null)
                {
                    // Kiểm tra quan hệ bạn bè hai chiều (mutual follow)
                    var relationshipUserView_User = await _relationshipRepository.GetRelationshipBetweenSenderAndReceiver(userLogin, user.ReceiverId);


                    if (relationshipUserView_User?.Relationship_type == RelationshipType.Follow)
                    {
                        // Lấy bài viết dạng story của người dùng
                        var posts = await _postRepository.GetAllStoriesOnlyFriendByUserIdAsync(user.ReceiverId, postQueryDTO);
                        if (posts != null)
                            postFriends.AddRange(posts); // Thêm tất cả bài viết
                    }
                }
            }

            if (!postFriends.Any())
                return new List<PostResponseDTO>();

            var listPostsDto = postFriends.Select(post => post.PostToPostResponseDTO()).ToList();

            return listPostsDto;
        }

        public async Task<PaginatedResult<PostResponseDTO>> GetAllByUserIdAsync(string userViewId, PostQueryDTO postQueryDTO)
        {
            var userId = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (userId == null)
                throw new AppError("You are not authorized to see this content", 401);

            var relationshipUserView_User = await _relationshipRepository.GetRelationshipBetweenSenderAndReceiver(userId, userViewId);
            var relationshipUser_UserView = await _relationshipRepository.GetRelationshipBetweenSenderAndReceiver(userViewId, userId);

            PaginatedResult<PostResponseDTO> listPostsDto;
            PaginatedResult<Post> posts;
            if (relationshipUserView_User == null && relationshipUser_UserView == null)
            {
                posts = await _postRepository.GetAllPostsPublicByUserIdAsync(userViewId, postQueryDTO);
                listPostsDto = new PaginatedResult<PostResponseDTO>(
                    posts.Items.Select(post => post.PostToPostResponseDTO()).ToList(),
                    posts.TotalItems,
                    posts.Page,
                    posts.PageSize);
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
                posts = await _postRepository.GetAllPostsOnlyFriendByUserIdAsync(userViewId, postQueryDTO);
                listPostsDto = new PaginatedResult<PostResponseDTO>(
                    posts.Items.Select(post => post.PostToPostResponseDTO()).ToList(),
                    posts.TotalItems,
                    posts.Page,
                    posts.PageSize);
            }
            else
            {
                posts = await _postRepository.GetAllPostsPublicByUserIdAsync(userViewId, postQueryDTO);
                listPostsDto = new PaginatedResult<PostResponseDTO>(
                    posts.Items.Select(post => post.PostToPostResponseDTO()).ToList(),
                    posts.TotalItems,
                    posts.Page,
                    posts.PageSize);
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
                postToUpdate.Visibility = (Visibility)post.Visibility;

            if (post.Is_story != null)
                postToUpdate.Is_story = (bool)post.Is_story;


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
