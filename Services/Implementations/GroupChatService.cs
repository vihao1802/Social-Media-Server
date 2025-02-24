using System.Security.Claims;
using SocialMediaServer.DTOs.Request.GroupChat;
using SocialMediaServer.DTOs.Response;
using SocialMediaServer.ExceptionHandling;
using SocialMediaServer.Mappers;
using SocialMediaServer.Models;
using SocialMediaServer.Repositories.Interfaces;
using SocialMediaServer.Services.Interfaces;
using SocialMediaServer.Utils;

namespace SocialMediaServer.Services.Implementations
{
    public class GroupChatService : IGroupChatService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IGroupChatRepository _grChatRepository;
        private readonly IMediaService _mediaService;
        private readonly IUserRepository _userRepository;
        private readonly INotificationRepository _notificationRepository;
        public GroupChatService(IGroupChatRepository grChatRepository, IHttpContextAccessor httpContextAccessor, IMediaService mediaService
        ,IUserRepository userRepository, INotificationRepository notificationRepository)
        {
            _grChatRepository = grChatRepository;
            _httpContextAccessor = httpContextAccessor;
            _mediaService = mediaService;
            _userRepository = userRepository;
            _notificationRepository = notificationRepository;
        }

        public async Task<GroupChatResponseDTO> CreateAsync(GroupChatCreateDTO groupChatCreateDTO,IFormFile? mediaFile)
        {
            // var userId = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            
            var user = await _userRepository.GetUserById(groupChatCreateDTO.AdminId);
            if(user == null)
                throw new AppError("Admin not found", 404);

            if (mediaFile != null)
            {
                // Danh sách các đuôi file ảnh hợp lệ
                var allowedExtensions = new List<string> { ".jpg", ".jpeg", ".png", ".gif" };
                string fileExtension = Path.GetExtension(mediaFile.FileName).ToLower();

                // Kiểm tra xem đuôi file có nằm trong danh sách cho phép không
                if (allowedExtensions.Contains(fileExtension))
                {
                    // Upload file nếu hợp lệ
                    string mediaUrl = await _mediaService.UploadMediaAsync(mediaFile, "GroupContent");
                    groupChatCreateDTO.avatar = mediaUrl;
                }
                else
                {
                    throw new AppError("Media file is not valid", 400);
                }
            } else
                groupChatCreateDTO.avatar = "string";

            var createdGroup = await _grChatRepository.CreateAsync(groupChatCreateDTO);
            var notification = await _notificationRepository.CreateAsync(new Notification{
                GroupId = createdGroup.Id,
                Group = createdGroup,
                Content = user.UserName + " created group",
            });
            return createdGroup.GrChatToGrChatResponseDTO();
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var grChat = await _grChatRepository.GetByIdAsync(id);
            if (grChat == null)
                throw new AppError("Group Chat not found!", 404);

            return await _grChatRepository.DeleteAsync(id);
        }

        public async Task<bool> DeleteGrAsync(int id)
        {
            var grChat = await _grChatRepository.GetByIdAsync(id);
            if (grChat == null)
                throw new AppError("Group Chat not found!", 404);
            var userId = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId == null || !userId.Equals(grChat.AdminId))
                throw new AppError("You are not authorized", 401);
            grChat.isDelete = true;
            await _grChatRepository.UpdateAsync(grChat);
            return true;
        }

        public async Task<PaginatedResult<GroupChatResponseDTO>> GetAllAsync(GroupChatQueryDTO grChatQueryDTO)
        {
            var grChat = await _grChatRepository.GetAllAsync(grChatQueryDTO);

            var listgrChatDto = grChat.Items.Select(grchat => grchat.GrChatToGrChatResponseDTO()).ToList();

            return new PaginatedResult<GroupChatResponseDTO>(
                listgrChatDto,
                grChat.TotalItems,
                grChat.Page,
                grChat.PageSize);
        }

        public async Task<GroupChatResponseDTO> GetByIdAsync(int id)
        {
            var grChat = await _grChatRepository.GetByIdAsync(id);

            if (grChat == null)
                throw new AppError("Group Chat not found!", 404);

            return grChat.GrChatToGrChatResponseDTO();
        }

        public async Task<List<GroupChatResponseDTO>> SearchForNames(string searchString)
        {
            var matchingGroups = await _grChatRepository.SearchByNameAsync(searchString);

            return matchingGroups.Select(grchat => grchat.GrChatToGrChatResponseDTO()).ToList();
        }

        public async Task<GroupChatResponseDTO> UpdateAsync(UpdateGrChatDTO updateDto, int id)
        {
            var groupToUpdate = await _grChatRepository.GetByIdAsync(id);
            if (groupToUpdate == null)
                throw new AppError("Group not found", 404);

            var group = updateDto.GroupChatUpdateDTOToGroupChat(groupToUpdate);

            if (updateDto.mediaFile != null)
            {
                // Danh sách các đuôi file ảnh hợp lệ
                var allowedExtensions = new List<string> { ".jpg", ".jpeg", ".png", ".gif" };
                string fileExtension = Path.GetExtension(updateDto.mediaFile.FileName).ToLower();

                // Kiểm tra xem đuôi file có nằm trong danh sách cho phép không
                if (allowedExtensions.Contains(fileExtension))
                {
                    await _mediaService.DeleteMediaAsync(groupToUpdate.Group_avt, "GroupContent");
                    // Upload file nếu hợp lệ
                    string mediaUrl = await _mediaService.UploadMediaAsync(updateDto.mediaFile, "GroupContent");
                    group.Group_avt = mediaUrl;
                }
                else
                {
                    throw new AppError("Media file is not valid", 400);
                }
            }

            var groupUpdated = await _grChatRepository.UpdateAsync(group);

            return groupUpdated.GrChatToGrChatResponseDTO();
        }

        public async Task TransferAdminAsync(int groupChatId, string newAdminId)
        {
            var groupChat = await _grChatRepository.GetByIdAsync(groupChatId);
            if (groupChat == null)
                throw new AppError("Group chat not found", 404);
            var userId = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId == null)
                throw new AppError("You are not authorized", 401);
            if(!userId.Equals(groupChat.AdminId))
                throw new AppError("You are not authorized", 401);
            var isMember = groupChat.Members.Any(member => member.UserId == newAdminId);
            if (!isMember)
                throw new AppError("The specified user is not a member of the group", 400);

            groupChat.AdminId = newAdminId;
            await _grChatRepository.UpdateAsync(groupChat);
        }

        public async Task<PaginatedResult<GroupChatResponseDTO>> GetAllByUserAsync(GroupChatQueryDTO grChatQueryDTO, string userId)
        {
            var user = await _userRepository.GetUserById(userId);
            if(user == null)
                throw new AppError("User not found", 404);
            var grChat = await _grChatRepository.GetAllByUserAsync(userId);

            var listgrChatDto = grChat.Items.OrderByDescending(i => i.Created_at).Select(grchat => grchat.GrChatToGrChatResponseDTO()).ToList();

            return new PaginatedResult<GroupChatResponseDTO>(
                listgrChatDto,
                grChat.TotalItems,
                grChat.Page,
                grChat.PageSize);
        }
    }
}
