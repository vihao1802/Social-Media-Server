using System.Security.Claims;
using SocialMediaServer.DTOs.Request.GroupChat;
using SocialMediaServer.DTOs.Response;
using SocialMediaServer.ExceptionHandling;
using SocialMediaServer.Mappers;
using SocialMediaServer.Repositories.Interfaces;
using SocialMediaServer.Services.Interfaces;
using SocialMediaServer.Utils;

namespace SocialMediaServer.Services.Implementations
{
    public class GroupChatService : IGroupChatService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IGroupChatRepository _grChatRepository;

        public GroupChatService(IGroupChatRepository grChatRepository, IHttpContextAccessor httpContextAccessor)
        {
            _grChatRepository = grChatRepository;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<GroupChatResponseDTO> CreateAsync(GroupChatCreateDTO groupChatCreateDTO)
        {
            var userId = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId == null)
                throw new AppError("You are not authorized", 401);
            groupChatCreateDTO.AdminId = userId;
            var createdGroup = await _grChatRepository.CreateAsync(groupChatCreateDTO);
            return createdGroup.GrChatToGrChatResponseDTO();
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var grChat = await _grChatRepository.GetByIdAsync(id);

            if (grChat == null)
                throw new AppError("Group Chat not found!", 404);

            return await _grChatRepository.DeleteAsync(id);
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
            var existingGroupChat = await _grChatRepository.GetByIdAsync(id);

            if (existingGroupChat == null)
                throw new AppError("Group Chat not found!", 404);

            existingGroupChat.Group_name = updateDto.Group_name;
            existingGroupChat.Group_avt = updateDto.Group_avt;
            existingGroupChat.Created_at = updateDto.Created_at ?? existingGroupChat.Created_at;

            var updatedGroupChat = await _grChatRepository.UpdateAsync(existingGroupChat);

            return updatedGroupChat.GrChatToGrChatResponseDTO();
        }

        public async Task TransferAdminAsync(int groupChatId, string newAdminId)
        {
            var groupChat = await _grChatRepository.GetByIdAsync(groupChatId);
            if (groupChat == null)
                throw new AppError("Group chat not found", 404);

            var isMember = groupChat.Members.Any(member => member.UserId == newAdminId);
            if (!isMember)
                throw new AppError("The specified user is not a member of the group", 400);

            groupChat.AdminId = newAdminId;
            await _grChatRepository.UpdateAsync(groupChat);
        }
    }
}
