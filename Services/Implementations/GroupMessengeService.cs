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
        private readonly IMediaService _mediaService;

        public GroupMessengeService(IGroupMessengeRepository grMessRepository, IMediaService mediaService)
        {
            _grMessRepository = grMessRepository;
            _mediaService = mediaService;
        }

        public async Task<GrMessResponseDTO> CreateAsync(GrMessCreateDTO grMessCreateDTO)
        {
            string? mediaUrl = null;

            if (grMessCreateDTO.MediaFile != null)
            {
                mediaUrl = await _mediaService.UploadMediaAsync(grMessCreateDTO.MediaFile, "GroupMessenges");
            }

            var groupMessage = new GroupMessenge
            {
                Content = grMessCreateDTO.Content,
                Media_content = mediaUrl,
                GroupChatId = grMessCreateDTO.GroupId,
                SenderId = grMessCreateDTO.SenderId,
                ReplyToId = grMessCreateDTO.ReplyToId
            };

            var createdMessage = await _grMessRepository.CreateAsync(groupMessage);

            return createdMessage.GrMessToGrMessResponseDTO();
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

        public async Task<PaginatedResult<GrMessResponseDTO>> GetAllByGroupIdAsync(int groupId, GrMessQueryDTO grMessQueryDTO)
        {
            var grMess = await _grMessRepository.GetAllByGroupIdAsync(grMessQueryDTO, groupId);
            
            var listsDto = grMess.Items.Select(x => x.GrMessToGrMessResponseDTO()).ToList();

            return new PaginatedResult<GrMessResponseDTO>(
                listsDto,
                grMess.TotalItems,
                grMess.Page,
                grMess.PageSize);
        }
    }
}
