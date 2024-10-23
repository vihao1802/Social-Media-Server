using DTOs.Response;
using SocialMediaServer.DTOs.Request.MediaContent;
using SocialMediaServer.ExceptionHandling;
using SocialMediaServer.Mappers;
using SocialMediaServer.Models;
using SocialMediaServer.Repositories.Interfaces;
using SocialMediaServer.Services.Interfaces;
using System.Security.Claims;

namespace SocialMediaServer.Services.Implementations
{
    public class MediaContentService : IMediaContentService
    {
        private readonly IMediaContentRepository _mediaContentRepository;
        private readonly IPostRepository _postRepository;
        private readonly IUserRepository _userRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IMediaService _mediaService;

        public MediaContentService(IMediaContentRepository mediaContentRepository, 
            IPostRepository postRepository, IHttpContextAccessor httpContextAccessor, 
            IMediaService mediaService, IUserRepository userRepository)
        {
            _mediaContentRepository = mediaContentRepository;
            _postRepository = postRepository;
            _httpContextAccessor = httpContextAccessor;
            _mediaService = mediaService;
            _userRepository = userRepository;
        }

        public async Task<List<MediaContentResponseDTO>> GetAllAsync()
        {
            var mediaContents = await _mediaContentRepository.GetAllMediaContentAsync();
            var listMediaContentsDto = mediaContents.Select(mediaContent =>
            mediaContent.MediaContentToMediaContentResponseDTO());
            return listMediaContentsDto.ToList();
        }

        public async Task<List<MediaContentResponseDTO>> GetAllByPostIdAsync(int postId)
        {
            var mediaContents = await _mediaContentRepository.GetAllMediaContentByPostIdAsync(postId);
            var listMediaContentsDto = mediaContents.Select(mediaContent => 
            mediaContent.MediaContentToMediaContentResponseDTO());
            return listMediaContentsDto.ToList();
        }

        public async Task<MediaContentResponseDTO> GetByIdAsync(int id)
        {
            var mediaContent = await _mediaContentRepository.GetByIdAsync(id)
                ?? throw new AppError("Media content not found", 404);
            return mediaContent.MediaContentToMediaContentResponseDTO();
        }

        public async Task<MediaContentResponseDTO> CreateAsync(MediaContentCreateDTO mediaContentCreateDTO, 
            IFormFile mediaFile)
        {   
            if (mediaFile == null || mediaFile.Length == 0)
            {
                throw new AppError("Media file is required", 400);
            }    

            var post = await _postRepository.GetByIdAsync(mediaContentCreateDTO.PostId);
            if (post == null)
                throw new AppError("Post not found", 404);

            string mediaUrl = await _mediaService.UploadMediaAsync(mediaFile, "MediaContent");

            var mediaContent = mediaContentCreateDTO.MediaContentCreateDTOToMediaContent();
            mediaContent.Post = post;
            mediaContent.Media_Url = mediaUrl;

            var mediaContentCreated = await _mediaContentRepository.CreateAsync(mediaContent);
            return mediaContentCreated.MediaContentToMediaContentResponseDTO();
        }

        public async Task<MediaContentResponseDTO> UpdateAsync(MediaContentUpdateDTO mediaContentUpdateDTO,
            int id, IFormFile mediaFile)
        {
            if (mediaFile == null || mediaFile.Length == 0)
            {
                throw new AppError("Media file is required", 400);
            }

            var mediaContentToUpdate = await _mediaContentRepository.GetByIdAsync(id)
                ?? throw new AppError("Media content not found", 404);

            var post = await _postRepository.GetByIdAsync(mediaContentToUpdate.PostId);

            await CheckPermissionAsync(post.CreatorId);

            await _mediaService.DeleteMediaAsync(mediaContentToUpdate.Media_Url, "MediaContent");

            string mediaUrl = await _mediaService.UploadMediaAsync(mediaFile, "MediaContent");

            var mediaContent = mediaContentUpdateDTO.MediaContentUpdateDTOToMediaContent(mediaContentToUpdate);
            mediaContent.Media_Url = mediaUrl;

            var mediaContentUpdated = await _mediaContentRepository.UpdateAsync(mediaContent);

            return mediaContentUpdated.MediaContentToMediaContentResponseDTO();
        }

        public async Task<MediaContentResponseDTO> PatchAsync(MediaContentPatchDTO mediaContentPatchDTO, 
            int id, IFormFile? mediaFile)
        {
            var mediaContentToUpdate = await _mediaContentRepository.GetByIdAsync(id)
                ?? throw new AppError("Media content not found", 404);

            var post = await _postRepository.GetByIdAsync(mediaContentToUpdate.PostId);

            await CheckPermissionAsync(post.CreatorId);

            var mediaContent = mediaContentPatchDTO.MediaContentPatchDTOToMediaContent(mediaContentToUpdate);

            if (mediaFile != null)
            {
                await _mediaService.DeleteMediaAsync(mediaContentToUpdate.Media_Url, "MediaContent");

                string mediaUrl = await _mediaService.UploadMediaAsync(mediaFile, "MediaContent");
                mediaContent.Media_Url = mediaUrl;
            }

            var mediaContentUpdated = await _mediaContentRepository.UpdateAsync(mediaContent);

            return mediaContentUpdated.MediaContentToMediaContentResponseDTO();
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var mediaContent = await _mediaContentRepository.GetByIdAsync(id)
                ?? throw new AppError("Media content not found", 404);

            var post = await _postRepository.GetByIdAsync(mediaContent.PostId);

            await CheckPermissionAsync(post.CreatorId);

            await _mediaService.DeleteMediaAsync(mediaContent.Media_Url, "MediaContent");

            return await _mediaContentRepository.DeleteAsync(id);
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
