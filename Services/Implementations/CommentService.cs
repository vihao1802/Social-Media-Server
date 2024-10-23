using DTOs.Response;
using SocialMediaServer.DTOs.Request.MediaContent;
using SocialMediaServer.ExceptionHandling;
using SocialMediaServer.Mappers;
using SocialMediaServer.Repositories.Interfaces;
using SocialMediaServer.Services.Interfaces;
using System.Security.Claims;

namespace SocialMediaServer.Services.Implementations
{
    public class CommentService : ICommentService
    {
        private readonly ICommentRepository _commentRepository;
        private readonly IPostRepository _postRepository;
        private readonly IUserRepository _userRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IMediaService _mediaService;

        public CommentService(ICommentRepository commentRepository, 
            IPostRepository postRepository, IHttpContextAccessor httpContextAccessor, 
            IMediaService mediaService, IUserRepository userRepository)
        {
            _commentRepository = commentRepository;
            _postRepository = postRepository;
            _httpContextAccessor = httpContextAccessor;
            _mediaService = mediaService;
            _userRepository = userRepository;
        }

        public async Task<List<CommentResponseDTO>> GetAllAsync()
        {
            var comments = await _commentRepository.GetAllCommentAsync();
            var listCommentsDto = comments.Select(comment =>
            comment.CommentToCommentResponseDTO());
            return listCommentsDto.ToList();
        }

        public async Task<List<CommentResponseDTO>> GetAllByPostIdAsync(int postId)
        {
            var comments = await _commentRepository.GetAllCommentByPostIdAsync(postId);
            var listCommentsDto = comments.Select(comment => 
            comment.CommentToCommentResponseDTO());
            return listCommentsDto.ToList();
        }

        public async Task<List<CommentResponseDTO>> GetAllByUserIdAsync(string userId)
        {
            var comments = await _commentRepository.GetCommentByUserIdAsync(userId);
            var listCommentsDto = comments.Select(comment => 
            comment.CommentToCommentResponseDTO());
            return listCommentsDto.ToList();
        }

        public async Task<CommentResponseDTO> GetByIdAsync(int id)
        {
            var comment = await _commentRepository.GetByIdAsync(id);
            return comment.CommentToCommentResponseDTO();
        }

        public async Task<CommentResponseDTO> CreateAsync(CommentCreateDTO commentCreateDTO, 
            IFormFile? mediaFile)
        {   
            
            var post = await _postRepository.GetByIdAsync(commentCreateDTO.PostId);

            if (post == null)
                throw new AppError("Post not found", 404);

            var user = await _userRepository.GetUserById(commentCreateDTO.UserId);

            if (user == null)
                throw new AppError("User not found", 404);

            var comment = commentCreateDTO.CommentCreateDTOToComment();

            if (commentCreateDTO.ParentCommentId != null)
            {
                var commentParent = await _commentRepository.GetByIdAsync(commentCreateDTO.ParentCommentId.Value);
                if (commentParent == null)
                    throw new AppError("Comment parent not found", 404);
                comment.ParentComment = commentParent;
            }

            if (mediaFile != null)
            {
                if (Path.GetExtension(mediaFile.FileName).ToLower() == ".gif")
                {
                    string mediaUrl = await _mediaService.UploadMediaAsync(mediaFile, "CommentContentGif");
                    comment.Content_gif = mediaUrl;
                }    
                else
                {
                    throw new AppError("Media file must be a gif", 400);
                }
            }

           
            comment.Post = post;
            comment.User = user;

            var commentCreated = await _commentRepository.CreateAsync(comment);

            return commentCreated.CommentToCommentResponseDTO();
        }

        public async Task<CommentResponseDTO> UpdateAsync(CommentUpdateDTO commentUpdateDTO,
            int id, IFormFile? mediaFile)
        {
           
            var commentToUpdate = await _commentRepository.GetByIdAsync(id);

            if (commentToUpdate == null)
                throw new AppError("Comment not found", 404);

            var post = await _postRepository.GetByIdAsync(commentUpdateDTO.PostId);

            if (post == null)
                throw new AppError("Post not found", 404);

            var user = await _userRepository.GetUserById(commentUpdateDTO.UserId);

            if (user == null)
                throw new AppError("User not found", 404);

            //var post = await _postRepository.GetByIdAsync(mediaContentToUpdate.PostId);

            //var userId = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            //if (userId == null)
            //    throw new AppError("You are not authorized", 401);

            //if (post.CreatorId != userId)
            //    throw new AppError("You do not have permision to delete this like", 401);

            var comment = commentUpdateDTO.CommentUpdateDTOToComment(commentToUpdate);

            if (commentUpdateDTO.ParentCommentId != null)
            {
                var commentParent = await _commentRepository.GetByIdAsync(commentUpdateDTO.ParentCommentId.Value);
                if (commentParent == null)
                    throw new AppError("Comment parent not found", 404);
                comment.ParentComment = commentParent;
            }

            if (mediaFile != null)
            {
                if (Path.GetExtension(mediaFile.FileName).ToLower() == ".gif")
                {
                    await _mediaService.DeleteMediaAsync(commentToUpdate.Content_gif, "CommentContentGif");

                    string mediaUrl = await _mediaService.UploadMediaAsync(mediaFile, "CommentContentGif");
                    comment.Content_gif = mediaUrl;
                }
                else
                {
                    throw new AppError("Media file must be a gif", 400);
                }
            }

            comment.Post = post;
            comment.User = user;


            var commentUpdated = await _commentRepository.UpdateAsync(comment);

            return commentUpdated.CommentToCommentResponseDTO();
        }

        public async Task<CommentResponseDTO> PatchAsync(CommentPatchDTO commentPatchDTO,
            int id, IFormFile? mediaFile)
        {
            var commentToUpdate = await _commentRepository.GetByIdAsync(id);

            if (commentToUpdate == null)
                throw new AppError("Comment not found", 404);

            //var post = await _postRepository.GetByIdAsync(mediaContentToUpdate.PostId);

            //var userId = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            //if (userId == null)
            //    throw new AppError("You are not authorized", 401);

            //if (post.CreatorId != userId)
            //    throw new AppError("You do not have permision to delete this like", 401);

            var comment = commentPatchDTO.CommentPatchDTOToComment(commentToUpdate);

            if (commentPatchDTO.UserId != null)
            {
                var user = await _userRepository.GetUserById(commentPatchDTO.UserId);
                if (user == null)
                    throw new AppError("User not found", 404);
                comment.User = user;
            }

            if (commentPatchDTO.PostId != null)
            {
                var post = await _postRepository.GetByIdAsync(commentPatchDTO.PostId.Value);
                if (post == null)
                    throw new AppError("Post not found", 404);
                comment.Post = post;
            }

            if (commentPatchDTO.ParentCommentId != null)
            {
                var commentParent = await _commentRepository.GetByIdAsync(commentPatchDTO.ParentCommentId.Value);
                if (commentParent == null)
                    throw new AppError("Comment parent not found", 404);
                comment.ParentComment = commentParent;
            }

            if (mediaFile != null)
            {
                if (Path.GetExtension(mediaFile.FileName).ToLower() == ".gif")
                {
                    await _mediaService.DeleteMediaAsync(commentToUpdate.Content_gif, "CommentContentGif");

                    string mediaUrl = await _mediaService.UploadMediaAsync(mediaFile, "CommentContentGif");
                    comment.Content_gif = mediaUrl;
                }
                else
                {
                    throw new AppError("Media file must be a gif", 400);
                }
            }

            var commentUpdated = await _commentRepository.UpdateAsync(comment);

            return commentUpdated.CommentToCommentResponseDTO();
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var comment = await _commentRepository.GetByIdAsync(id);

            if (comment == null)
                throw new AppError("Comment not found", 404);

            //var post = await _postRepository.GetByIdAsync(mediaContent.PostId);

            //var userId = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            //if (userId == null)
            //    throw new AppError("You are not authorized", 401);

            //if (post.CreatorId != userId)
            //    throw new AppError("You do not have permision to delete this like", 401);

            await _mediaService.DeleteMediaAsync(comment.Content_gif, "CommentContentGif");

            return await _commentRepository.DeleteAsync(id);
        }

        public async Task<bool> DeleteAllByPostIdAsync(int postId)
        {
            var comments = await _commentRepository.GetAllCommentByPostIdAsync(postId);

            foreach (var comment in comments)
            {
                await _mediaService.DeleteMediaAsync(comment.Content_gif, "CommentContentGif");
            }

            return await _commentRepository.DeleteAllByPostIdAsync(postId);
        }
    }
}
