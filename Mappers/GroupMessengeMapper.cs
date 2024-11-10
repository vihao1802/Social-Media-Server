using DTOs.Response;
using SocialMediaServer.DTOs.Request.GroupMess;
using SocialMediaServer.DTOs.Response;
using SocialMediaServer.Models;

namespace SocialMediaServer.Mappers
{
    public static class GroupMessMapper
    {
        public static GrMessResponseDTO GrMessToGrMessResponseDTO(this GroupMessenge grMess)
        {
            if (grMess == null)
                throw new ArgumentNullException(nameof(grMess), "Group Messenge cannot be null");

            var creatorDTO = grMess.Sender != null ? grMess.Sender.UserToUserResponseDTO()
                                                  : new UserResponseDTO { Id = grMess.SenderId };
            var replyToDto = grMess.ReplyTo != null ? grMess.ReplyTo.GrMessToGrMessResponseDTO()
                                                  : new GrMessResponseDTO {Id = grMess.ReplyToId};
            

            return new GrMessResponseDTO
            {
                Id = grMess.Id,
                Content = grMess.Content,
                Media_content = grMess.Media_content,
                GroupChat = grMess.groupChat.GrChatToGrChatResponseDTO(),
                Sender = creatorDTO,
                ReplyTo = replyToDto,
                Sent_at = grMess.Sent_at
            };
        }

        public static GroupMessenge PostCreateDTOToPost(this GrMessCreateDTO grMessCreateDTO)
        {
            if (grMessCreateDTO == null)
                throw new ArgumentNullException(nameof(grMessCreateDTO), "GrMessCreateDTO cannot be null");

            return new GroupMessenge
            {
                Content = grMessCreateDTO.Content,
                Media_content = grMessCreateDTO.MediaContent,
                GroupChatId = grMessCreateDTO.GroupId,
                ReplyToId = grMessCreateDTO.ReplyToId
            };
        }
    }
}