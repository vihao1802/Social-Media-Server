using DTOs.Response;
using Microsoft.Extensions.Hosting;
using SocialMediaServer.DTOs.Request.PostViewer;
using SocialMediaServer.ExceptionHandling;
using SocialMediaServer.Mappers;
using SocialMediaServer.Models;
using SocialMediaServer.Repositories.Implementations;
using SocialMediaServer.Repositories.Interfaces;
using SocialMediaServer.Services.Interfaces;
using SocialMediaServer.Utils;
using System.Security.Claims;

namespace SocialMediaServer.Services.Implementations
{
    public class PostViewerService : IPostViewerService
    {
       private readonly IPostViewerRepository _postViewerRepository;
       private readonly IUserRepository _userRepository;
       private readonly IPostRepository _postRepository;
       private readonly IHttpContextAccessor _httpContextAccessor;

       public PostViewerService(IPostViewerRepository postViewerRepository, IUserRepository userRepository, 
           IPostRepository postRepository, IHttpContextAccessor httpContextAccessor)
       {
           _postViewerRepository = postViewerRepository;
           _userRepository = userRepository;
           _postRepository = postRepository;
           _httpContextAccessor = httpContextAccessor;
       }

       public async Task<PaginatedResult<PostViewerResponseDTO>> GetAllByPostIdAsync(int postId, PostViewerQueryDTO postViewerQueryDTO)
       {
            var postViewers = await _postViewerRepository.GetAllByPostIdAsync(postId, postViewerQueryDTO);
            var listPostViewersDto = postViewers.Items.Select(post => post.PostViewerToPostViewerResponseDTO()).ToList();

            return new PaginatedResult<PostViewerResponseDTO>(
                listPostViewersDto,
                postViewers.TotalItems,
                postViewers.Page,
                postViewers.PageSize);
        }

        public async Task<PostViewerResponseDTO> CreateByPostIdAsync(PostViewerCreateDTO postViewerCreateDTO)
        {
            var user = await _userRepository.GetUserById(postViewerCreateDTO.UserId);
            var post = await _postRepository.GetByIdAsync(postViewerCreateDTO.PostId);

            var postViewer = postViewerCreateDTO.PostViewerCreateDTOToPostViewer();

            if (user == null)
                throw new AppError("User not found", 404);
            if (post == null)
                throw new AppError("Post not found", 404);

            postViewer.User = user;
            postViewer.Post = post;

            var postViewerCreated = await _postViewerRepository.CreateByPostIdAsync(postViewer);

            return postViewerCreated.PostViewerToPostViewerResponseDTO();
        }

        public async Task<PostViewerResponseDTO> GetByIdAsync(int id)
        {
            var postViewer = await _postViewerRepository.GetByIdAsync(id);

            if (postViewer == null)
                throw new AppError("PostViewer not found", 404);

            return postViewer.PostViewerToPostViewerResponseDTO();
        }

        public async Task<bool> DeleteByIdAsync(int id)
        {
            var postViewer = await _postViewerRepository.GetByIdAsync(id);

            if (postViewer == null)
                throw new AppError("PostViewer not found", 404);

            await CheckPermissionAsync(postViewer.UserId);

            return await _postViewerRepository.DeleteByIdAsync(id);
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
