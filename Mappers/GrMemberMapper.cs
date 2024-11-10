using DTOs.Response;
using SocialMediaServer.DTOs.Request.GroupChat;
using SocialMediaServer.DTOs.Request.MediaContent;
using SocialMediaServer.DTOs.Response;
using SocialMediaServer.Models;

namespace SocialMediaServer.Mappers
{
    public static class GroupMemberMapper
    {
        public static MemberResponseDTO GroupMemberToGroupMemberResponseDTO(this GroupMember groupMember)
        {
            if (groupMember == null)
                throw new ArgumentNullException(nameof(groupMember), "groupMember cannot be null");

            return new MemberResponseDTO
            {
                Id = groupMember.Id,
                GroupChat = groupMember.GroupChat.GroupToGroupResponseDTO(),
                User = groupMember.User.UserToUserResponseDTO(),
                Join_at = groupMember.Join_at
            };
        }

        // public static GroupChat GroupChatUpdateDTOToGroupChat(this GroupUpdateDTO GroupChatUpdateDTO, GroupChat GroupChat)
        // {
        //     if (GroupChatUpdateDTO == null)
        //         throw new ArgumentNullException(nameof(GroupChatUpdateDTO), "GroupChatUpdateDTO cannot be null");

        //     GroupChat.Group_name = GroupChatUpdateDTO.Group_name;
        //     GroupChat.Group_avt = GroupChatUpdateDTO.Group_avt;
        //     GroupChat.AdminId = GroupChatUpdateDTO.AdminId;

        //     return GroupChat;
        // }

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
