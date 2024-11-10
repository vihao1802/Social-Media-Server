using System.Security.Claims;
using SocialMediaServer.DTOs.Request.GroupMember;
using SocialMediaServer.DTOs.Response;
using SocialMediaServer.ExceptionHandling;
using SocialMediaServer.Mappers;
using SocialMediaServer.Models;
using SocialMediaServer.Repositories.Interfaces;
using SocialMediaServer.Services.Interfaces;

namespace SocialMediaServer.Services.Implementations
{
    public class GroupMemberService : IGroupMemberService
    {
        private readonly IGroupMemberRepository _groupMemberRepository;
        private readonly IUserRepository _userRepository;
        private readonly IGroupRepository _groupRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public GroupMemberService(IGroupMemberRepository groupMemberRepository, IUserRepository userRepository
        ,IGroupRepository groupRepository, IHttpContextAccessor httpContextAccessor)
        {
            _groupMemberRepository = groupMemberRepository;
            _userRepository = userRepository;
            _groupRepository = groupRepository;
            _httpContextAccessor = httpContextAccessor;
        }
        public async Task<MemberResponseDTO> CreateAsync(int gr_id, string user_id)
        {
            var user = await _userRepository.GetUserById(user_id);

            if (user == null)
                throw new AppError("User not found", 404);

            var group = await _groupRepository.GetByIdAsync(gr_id);

            if (group == null)
                throw new AppError("Group not found", 404);

            var admin = await GetCurrentUserAsync();
            if(!admin.Id.Equals(group.AdminId))
                throw new AppError("Not Admin", 404);
                
            var createdGrMember = await _groupMemberRepository.CreateAsync(new GroupMember{
                GroupChatId = gr_id,
                GroupChat = group,
                UserId = user_id,
                User = user
            });
            return createdGrMember.GroupMemberToGroupMemberResponseDTO();
        }

        public async Task<bool> DeleteAsync(int member_id)
        {
            var member = await _groupMemberRepository.GetByIdAsync(member_id);
            if(member == null)
                throw new AppError("Member not found", 404);
            var admin = await GetCurrentUserAsync();
            if(!admin.Id.Equals(member.GroupChat.AdminId))
                return false;
            member.isDelete = true;
            await _groupMemberRepository.UpdateAsync(member);
            return true;
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

        public Task<MemberResponseDTO> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<MemberResponseDTO> UpdateAsync(GroupMemberUpdateDTO grMemberUpdateDTO, int id)
        {
            throw new NotImplementedException();
        }
    }
}