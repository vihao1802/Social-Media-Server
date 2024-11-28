using System.Security.Claims;
using DTOs.Response;
using SocialMediaServer.DTOs.Request.GroupChat;
using SocialMediaServer.DTOs.Request.GroupMess;
using SocialMediaServer.ExceptionHandling;
using SocialMediaServer.Mappers;
using SocialMediaServer.Models;
using SocialMediaServer.Repositories.Interfaces;
using SocialMediaServer.Services.Interfaces;
using SocialMediaServer.Utils;

namespace SocialMediaServer.Services.Implementations
{
    public class GroupMessengeService : IGroupMessengeService
    {
        private readonly IGroupMessengeRepository _grMessRepository;
        private readonly IUserRepository _userRepository;
        private readonly IGroupChatRepository _groupChatRepository;
        private readonly IMediaService _mediaService;
        private readonly IGroupMemberRepository _grMemberRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public GroupMessengeService(IGroupMemberRepository grMemberRepository,IGroupMessengeRepository grMessRepository, IMediaService mediaService, IUserRepository userRepository,
        IGroupChatRepository groupChatRepository, IHttpContextAccessor httpContextAccessor)
        {
            _grMessRepository = grMessRepository;
            _mediaService = mediaService;
            _userRepository = userRepository;
            _groupChatRepository = groupChatRepository;
            _grMemberRepository = grMemberRepository;
        }

        public async Task<GrMessResponseDTO> CreateAsync(GrMessCreateDTO grMessCreateDTO)
        {
            if(grMessCreateDTO.Content == null && grMessCreateDTO.MediaFile == null)
                throw new AppError("Data null", 404);
            
            var group = await _groupChatRepository.GetByIdAsync(grMessCreateDTO.GroupId);

            if (group == null)
                throw new AppError("Group not found", 404);

            var user = await _userRepository.GetUserById(grMessCreateDTO.SenderId);

            if (user == null)
                throw new AppError("Sender not found", 404);

            var grMess = grMessCreateDTO.GroupMessengeCreateDTOToGroupMessenge();

            if (grMessCreateDTO.ReplyToId != null)
            {
                var replyTo = await _grMessRepository.GetByIdAsync(grMessCreateDTO.ReplyToId.Value);
                if (replyTo == null)
                    throw new AppError("ReplyTo not found", 404);
                grMess.ReplyTo = replyTo;
            }

            if (grMessCreateDTO.MediaFile != null)
            {
                // Danh sách các đuôi file ảnh hợp lệ
                var allowedExtensions = new List<string> { ".jpg", ".jpeg", ".png", ".gif",".mp4", ".avi", ".mov", ".mkv", ".flv" };
                string fileExtension = Path.GetExtension(grMessCreateDTO.MediaFile.FileName).ToLower();

                // Kiểm tra xem đuôi file có nằm trong danh sách cho phép không
                if (allowedExtensions.Contains(fileExtension))
                {
                    // Upload file nếu hợp lệ
                    string mediaUrl = await _mediaService.UploadMediaAsync(grMessCreateDTO.MediaFile, "GroupMessenges");
                    grMess.Media_content = mediaUrl;
                }
                else
                {
                    throw new AppError("Media file is not valid", 400);
                }
            }

           
            grMess.groupChat = group;
            grMess.Sender = user;

            var grMessCreated = await _grMessRepository.CreateAsync(grMess);

            return grMessCreated.GrMessToGrMessResponseDTO();
        }

        public async Task<bool> DeleteAllByGroupIdAsync(int groupId)
        {
            var grMess = await _grMessRepository.GetAllByGroupIdAsync(new GrMessQueryDTO(), groupId);

            // Xóa từng media của các tin nhắn trong nhóm
            foreach (var message in grMess.Items)
            {
                await _mediaService.DeleteMediaAsync(message.Media_content, "GroupMessenges");
            }

            return await _grMessRepository.DeleteAllByGroupIdAsync(groupId);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var grMess = await _grMessRepository.GetByIdAsync(id);

            if (grMess == null)
                throw new AppError("Message not found", 404);

            if (!string.IsNullOrEmpty(grMess.Media_content))
            {
                await _mediaService.DeleteMediaAsync(grMess.Media_content, "GroupMessenges");
            }

            return await _grMessRepository.DeleteAsync(id);
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

        public async Task<PaginatedResult<GrMessResponseDTO>> GetAllByGroupIdAsync(int groupId, GrMessQueryDTO grMessQueryDTO)
        {
            var user = await GetCurrentUserAsync();
            if (user == null)
            {
                throw new AppError("You are not authorized", 401);
            }

            // Kiểm tra xem nhóm có tồn tại hay không
            var group = await _groupChatRepository.GetByIdAsync(groupId);
            if (group == null)
            {
                throw new AppError("Group not found", 404);
            }

            // Kiểm tra xem người dùng hiện tại có phải là thành viên của nhóm không
            var isMember = await _grMemberRepository.IsMemberOfGroupAsync(group.Id, user.Id);
            if (!isMember)
            {
                throw new AppError("Member not found", 404);
            }

            // Lấy danh sách tin nhắn của nhóm với điều kiện phù hợp
            var groupMessages = await _grMessRepository.GetAllByGroupIdAsync(grMessQueryDTO, groupId);

            // Chuyển đổi danh sách tin nhắn sang DTO
            var messageDtoList = groupMessages.Items.Select(x => x.GrMessToGrMessResponseDTO()).ToList();

            return new PaginatedResult<GrMessResponseDTO>(
                messageDtoList,
                groupMessages.TotalItems,
                groupMessages.Page,
                groupMessages.PageSize
            );
        }

        public async Task<bool> RecallAsync(int id)
        {
            return await _grMessRepository.RecallAsync(id);
        }
    }
}
