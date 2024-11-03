using SocialMediaServer.DTOs.Request.GroupMember;
using SocialMediaServer.DTOs.Response;
using SocialMediaServer.Models;

namespace SocialMediaServer.Mappers
{
    public static class GroupMemberMapper
    {
        public static GroupMember GrMemberCreateDTOToGrMember(this GroupMemberCreateDTO grMemberCreateDTO)
        {
            if (grMemberCreateDTO == null)
                throw new ArgumentNullException(nameof(grMemberCreateDTO), "GrMemberCreateDTO cannot be null");

            return new GroupMember
            {
                GroupChatId = grMemberCreateDTO.GroupId,
                UserId = grMemberCreateDTO.UserId
            };
        }

        public static GroupMemberResponseDTO GrMemberToGrMemberResponseDTO(this GroupMember grMember)
        {
            if (grMember == null)
                throw new ArgumentNullException(nameof(grMember), "grMember cannot be null");

            var Group = grMember.GroupChat != null ? grMember.GroupChat.GrChatToGrChatResponseDTO()
                                                  : new GroupChatResponseDTO { Id = grMember.GroupChatId };

            var user = grMember.User != null ? grMember.User.UserToUserResponseDTO()
                                            : new UserResponseDTO { Id = grMember.UserId };


            return new GroupMemberResponseDTO
            {
                Id = grMember.Id,
                Group = Group,
                User = user,
                Join_at = grMember.Join_at
            };
        }

        public static GroupMember GrMemberUpdateDTOToGrMember(this GroupMemberUpdateDTO grMembertUpdateDTO, GroupMember grMember)
        {
            if (grMembertUpdateDTO == null)
                throw new ArgumentNullException(nameof(grMembertUpdateDTO), "GroupMemberUpdateDTO cannot be null");

            grMember.UserId = grMembertUpdateDTO.UserId;
            grMember.GroupChatId = grMembertUpdateDTO.GroupId;

            return grMember;
        }
    }
}