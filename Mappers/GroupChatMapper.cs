using SocialMediaServer.DTOs.Request.GroupChat;
using SocialMediaServer.DTOs.Response;
using SocialMediaServer.Models;

namespace SocialMediaServer.Mappers
{
    public static class GroupChatMapper
    {
         public static GroupChatResponseDTO GrChatToGrChatResponseDTO(this GroupChat grChat )
        {
            if (grChat == null)
                throw new ArgumentNullException(nameof(grChat), "Group Chat cannot be null");

            return new GroupChatResponseDTO
            {
                Id = grChat.Id,
                name = grChat.Group_name,
                avatar = grChat.Group_avt
            };
        }

        public static GroupChat GrChatCreateDTOToPost(this GroupChatCreateDTO grChatCreateDTO)
        {
            if (grChatCreateDTO == null)
                throw new ArgumentNullException(nameof(grChatCreateDTO), "GroupChatCreateDTO cannot be null");

            return new GroupChat
            {
                Group_name = grChatCreateDTO.name,
                Group_avt = grChatCreateDTO.avatar
            };
        }

        public static GroupChat GroupChatUpdateDTOToGroupChat(this UpdateGrChatDTO GroupChatUpdateDTO, GroupChat GroupChat)
        {
            if (GroupChatUpdateDTO == null)
                throw new ArgumentNullException(nameof(GroupChatUpdateDTO), "GroupChatUpdateDTO cannot be null");

            GroupChat.Group_name = GroupChatUpdateDTO.Group_name;
            GroupChat.Group_avt = GroupChatUpdateDTO.Group_avt;

            return GroupChat;
        }
    }
}