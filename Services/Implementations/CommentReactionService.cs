using DTOs.Response;
using SocialMediaServer.DTOs.Request.MediaContent;
using SocialMediaServer.DTOs.Request.PostViewer;
using SocialMediaServer.ExceptionHandling;
using SocialMediaServer.Mappers;
using SocialMediaServer.Models;
using SocialMediaServer.Repositories.Implementations;
using SocialMediaServer.Repositories.Interfaces;
using SocialMediaServer.Services.Interfaces;
using System.Security.Claims;

namespace SocialMediaServer.Services.Implementations
{
    public class CommentReactionService : ICommentReactionService
    {
       private readonly ICommentReactionRepository _commentReactionRepository;
       private readonly IUserRepository _userRepository;
       private readonly ICommentRepository _commentRepository;
       private readonly IHttpContextAccessor _httpContextAccessor;

       public CommentReactionService(ICommentReactionRepository commentReactionRepository, IUserRepository userRepository,
           ICommentRepository commentRepository, IHttpContextAccessor httpContextAccessor)
       {
           _commentReactionRepository = commentReactionRepository;
           _userRepository = userRepository;
           _commentRepository = commentRepository;
           _httpContextAccessor = httpContextAccessor;
       }

       public async Task<List<CommentReactionResponseDTO>> GetAllByCommentIdAsync(int commentId)
       {
            var commentReactions = await _commentReactionRepository.GetAllByCommentIdAsync(commentId);
            var listCommentReactionsDto = commentReactions.Select(commentReaction => commentReaction.CommentReactionToCommentReactionResponseDTO());
            return listCommentReactionsDto.ToList();
        }

        public async Task<CommentReactionResponseDTO> CreateByCommentIdAsync(CommentReactionCreateDTO commentReactionCreateDTO)
        {
            var user = await _userRepository.GetUserById(commentReactionCreateDTO.UserId);
            var comment = await _commentRepository.GetByIdAsync(commentReactionCreateDTO.CommentId);

            var commentReaction = commentReactionCreateDTO.CommentReactionCreateDTOToCommentReaction();

            if (user == null)
                throw new AppError("User not found", 404);
            if (comment == null)
                throw new AppError("Comment not found", 404);

            commentReaction.User = user;
            commentReaction.Comment = comment;

            var commentReactionCreated = await _commentReactionRepository.CreateByCommentIdAsync(commentReaction);

            return commentReactionCreated.CommentReactionToCommentReactionResponseDTO();
        }

        public async Task<CommentReactionResponseDTO> GetByIdAsync(int id)
        {
            var commentReaction = await _commentReactionRepository.GetByIdAsync(id);

            if (commentReaction == null)
                throw new AppError("CommentReaction not found", 404);

            return commentReaction.CommentReactionToCommentReactionResponseDTO();
        }

        public async Task<bool> DeleteByIdAsync(int id)
        {
            var commentReaction = await _commentReactionRepository.GetByIdAsync(id);

            if (commentReaction == null)
                throw new AppError("CommentReaction not found", 404);

            await CheckPermissionAsync(commentReaction.UserId);

            return await _commentReactionRepository.DeleteByIdAsync(id);
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
