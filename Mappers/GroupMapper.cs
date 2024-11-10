using DTOs.Response;
using SocialMediaServer.DTOs.Request.GroupChat;
using SocialMediaServer.DTOs.Request.MediaContent;
using SocialMediaServer.DTOs.Response;
using SocialMediaServer.Models;

namespace SocialMediaServer.Mappers
{
    public static class GroupMapper
    {
        public static GroupResponseDTO GroupToGroupResponseDTO(this GroupChat groupChat)
        {
            if (groupChat == null)
                throw new ArgumentNullException(nameof(groupChat), "groupChat cannot be null");

            return new GroupResponseDTO
            {
                Id = groupChat.Id,
                Name = groupChat.Group_name,
                Avatar = groupChat.Group_avt,
                Create_date = groupChat.Created_at
            };
        }

        public static GroupChat GroupCreateDTOToGroup(this GroupCreateDTO groupCreateDTO)
        {
            if (groupCreateDTO == null)
                throw new ArgumentNullException(nameof(groupCreateDTO), "groupCreateDTO cannot be null");

            return new GroupChat
            {
                Group_avt = groupCreateDTO.Avatar,
                Group_name = groupCreateDTO.Name,
                AdminId = groupCreateDTO.CreaterId
            };
        }

        public static GroupChat GroupChatUpdateDTOToGroupChat(this GroupUpdateDTO GroupChatUpdateDTO, GroupChat GroupChat)
        {
            if (GroupChatUpdateDTO == null)
                throw new ArgumentNullException(nameof(GroupChatUpdateDTO), "GroupChatUpdateDTO cannot be null");

            GroupChat.Group_name = GroupChatUpdateDTO.Group_name;
            GroupChat.Group_avt = GroupChatUpdateDTO.Group_avt;
            GroupChat.AdminId = GroupChatUpdateDTO.AdminId;

            return GroupChat;
        }

        // public static Comment CommentPatchDTOToComment(this CommentPatchDTO commentPatchDTO, Comment comment)
        // {
        //     if (commentPatchDTO == null)
        //         throw new ArgumentNullException(nameof(commentPatchDTO), "CommentPatchDTO cannot be null");

        //     if (commentPatchDTO.Content != null)
        //         comment.Content = commentPatchDTO.Content;

        //     if (commentPatchDTO.PostId != null)
        //         comment.PostId = commentPatchDTO.PostId.Value;

        //     if (commentPatchDTO.UserId != null)
        //         comment.UserId = commentPatchDTO.UserId;

        //     return comment;
        // }
    }
}
