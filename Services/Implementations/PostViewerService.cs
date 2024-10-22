using DTOs.Response;
using SocialMediaServer.DTOs.Request.PostViewer;
using SocialMediaServer.ExceptionHandling;
using SocialMediaServer.Mappers;
using SocialMediaServer.Repositories.Implementations;
using SocialMediaServer.Repositories.Interfaces;
using SocialMediaServer.Services.Interfaces;
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

       public async Task<List<PostViewerResponseDTO>> GetAllByPostIdAsync(int postId)
       {
            var postViewers = await _postViewerRepository.GetAllByPostIdAsync(postId);
            var listPostViewersDto = postViewers.Select(post => post.PostViewerToPostViewerResponseDTO());
            return listPostViewersDto.ToList();
        }

        public async Task<PostViewerResponseDTO> CreateByPostIdAsync(PostViewerCreateDTO postViewerCreateDTO)
        {
            var userId = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (userId == null)
                throw new AppError("You are not authorized", 401);

            var user = await _userRepository.GetUserById(userId);
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

        public async Task<bool> DeleteByPostIdAsync(int id, int postId)
        {
            var postViewer = await _postViewerRepository.GetByIdAsync(id);

            if (postViewer == null)
                throw new AppError("PostViewer not found", 404);

            var userId = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (userId == null)
                throw new AppError("You are not authorized", 401);

            if (postViewer.UserId != userId)
                throw new AppError("You do not have permision to delete this like", 401);

            return await _postViewerRepository.DeleteByPostidAsync(id, postId);
        }
    }
}
