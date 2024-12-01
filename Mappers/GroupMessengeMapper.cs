using DTOs.Response;
using SocialMediaServer.DTOs.Request.GroupMess;
using SocialMediaServer.DTOs.Response;
using SocialMediaServer.Models;

namespace SocialMediaServer.Mappers
{
    public static class GroupMessMapper
    {
        public static GrMessResponseDTO GrMessToGrMessResponseDTO(this GroupMessenge groupMessenge, int maxDepth = 3, int currentDepth = 0)
        {
            if (groupMessenge == null)
                throw new ArgumentNullException(nameof(groupMessenge), "groupMessenge cannot be null");

            if (currentDepth >= maxDepth)
                return new GrMessResponseDTO
                {
                    Id = groupMessenge.Id,
                    Content = groupMessenge.Content,
                    Media_content = groupMessenge.Media_content ?? string.Empty,
                    GroupChat = groupMessenge.groupChat != null ? groupMessenge.groupChat.GrChatToGrChatResponseDTO() : new GroupChatResponseDTO { Id = groupMessenge.GroupChatId },
                    Sender = groupMessenge.Sender != null ? groupMessenge.Sender.UserToUserResponseDTO() : new UserResponseDTO { Id = groupMessenge.SenderId },
                    ReplyTo = null, // không ánh xạ ReplyTo nếu đạt độ sâu tối đa
                    Sent_at = groupMessenge.Sent_at
                };

            var sender = groupMessenge.Sender != null ? groupMessenge.Sender.UserToUserResponseDTO()
                                                    : new UserResponseDTO { Id = groupMessenge.SenderId };
            var group = groupMessenge.groupChat != null ? groupMessenge.groupChat.GrChatToGrChatResponseDTO()
                                                        : new GroupChatResponseDTO { Id = groupMessenge.GroupChatId };

            var replyTo = groupMessenge.ReplyTo != null 
                            ? groupMessenge.ReplyTo.GrMessToGrMessResponseDTO(maxDepth, currentDepth + 1)  
                            : null;

            var listMessReaction = groupMessenge.Reactions.Select(r => r.MessageReactionToMessageReactionResponseDTO()).ToList();

            return new GrMessResponseDTO
            {
                Id = groupMessenge.Id,
                Content = groupMessenge.Content,
                Media_content = groupMessenge.Media_content ?? string.Empty,
                GroupChat = group,
                Sender = sender,
                ReplyTo = replyTo,
                Sent_at = groupMessenge.Sent_at,
                Icons = listMessReaction
            };
        }

    
        public static GroupMessenge GroupMessengeCreateDTOToGroupMessenge(this GrMessCreateDTO grMessCreateDTO)
        {
            if (grMessCreateDTO == null)
                throw new ArgumentNullException(nameof(grMessCreateDTO), "GrMessCreateDTO cannot be null");

            return new GroupMessenge
            {
                Content = grMessCreateDTO.Content,
                GroupChatId = grMessCreateDTO.GroupId,
                SenderId = grMessCreateDTO.SenderId,
                
            };
        }
    }
}