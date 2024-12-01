using SocialMediaServer.DTOs.Request.GroupMember;
using SocialMediaServer.DTOs.Response;
using SocialMediaServer.ExceptionHandling;
using SocialMediaServer.Mappers;
using SocialMediaServer.Repositories.Interfaces;
using SocialMediaServer.Services.Interfaces;
using SocialMediaServer.Utils;

namespace SocialMediaServer.Services.Implementations
{
    public class GroupMemberService : IGroupMemberService
    {
        private readonly IGroupMemberRepository _grMemberRepository;
        private readonly IGroupChatRepository _grChatRepository;
        private readonly IUserRepository _userRepository;
        public GroupMemberService(IGroupMemberRepository groupMemberRepository, IGroupChatRepository groupChatRepository, IUserRepository userRepository)
        {
            _grMemberRepository = groupMemberRepository;
            _grChatRepository = groupChatRepository;
            _userRepository = userRepository;
        }
        public async Task<GroupMemberResponseDTO> CreateAsync(GroupMemberCreateDTO grMemberCreateDTO)
        {
            var group = await _grChatRepository.GetByIdAsync(grMemberCreateDTO.GroupId);

            if (group == null)
                throw new AppError("Group not found", 404);

            var user = await _userRepository.GetUserById(grMemberCreateDTO.UserId);

            if (user == null)
                throw new AppError("User not found", 404);

            var member = await _grMemberRepository.GetByGroupAndUser(group.Id, user.Id);
            if(member != null){
                if(!member.isLeft){
                    throw new AppError("Member already exists");
                } else {
                    member.isLeft = false;
                    member.Left_at = DateTime.Now;
                    var memberUpdate = await _grMemberRepository.UpdateAsync(member);
                    return memberUpdate.GrMemberToGrMemberResponseDTO();
                }
            }

            var grMember = grMemberCreateDTO.GrMemberCreateDTOToGrMember();
           
            grMember.GroupChat = group;
            grMember.User = user;
            grMember.Join_at = DateTime.Now;

            var grMemberCreated = await _grMemberRepository.CreateAsync(grMember);

            return grMemberCreated.GrMemberToGrMemberResponseDTO();
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var grMember = await _grMemberRepository.GetByIdAsync(id);

            if (grMember == null)
                throw new AppError("grMember not found", 404);

            return await _grMemberRepository.DeleteAsync(id);
        }

        public async Task<bool> OutGroupAsync(int id)
        {
            var grMember = await _grMemberRepository.GetByIdAsync(id);

            if (grMember == null)
                throw new AppError("grMember not found", 404);

            grMember.isLeft = true;
            grMember.Left_at = DateTime.Now;
            await _grMemberRepository.UpdateAsync(grMember);

            return true;
        }

        public async Task<PaginatedResult<GroupMemberResponseDTO>> GetAllAsync(GroupMemberQueryDTO grMemberQueryDTO)
        {
            var grMembers = await _grMemberRepository.GetAllAsync(grMemberQueryDTO);
            var count = grMembers.Items.Count();
            Console.WriteLine("size: " + count);
            var listgrMembersDto = grMembers.Items.Select(grMember =>
            grMember.GrMemberToGrMemberResponseDTO()).ToList();

            return new PaginatedResult<GroupMemberResponseDTO>(
                listgrMembersDto,
                grMembers.TotalItems,
                grMembers.Page,
                grMembers.PageSize);
        }

        public async Task<PaginatedResult<GroupMemberResponseDTO>> GetAllByGroupIdAsync(int groupId, GroupMemberQueryDTO grMemberQueryDTO)
        {
            var grMember = await _grMemberRepository.GetAllByGroupIdAsync(groupId, grMemberQueryDTO);
            var listgrMemberDto = grMember.Items.Select(x =>
            x.GrMemberToGrMemberResponseDTO()).ToList();

            return new PaginatedResult<GroupMemberResponseDTO>(
                listgrMemberDto,
                grMember.TotalItems,
                grMember.Page,
                grMember.PageSize);
        }

        public async Task<PaginatedResult<GroupMemberResponseDTO>> GetAllByUserIdAsync(string userId, GroupMemberQueryDTO grMemberQueryDTO)
        {
            var grMembers = await _grMemberRepository.GetByUserIdAsync(userId, grMemberQueryDTO);
            var listgrMembersDto = grMembers.Items.Select(x =>
            x.GrMemberToGrMemberResponseDTO()).ToList();

            return new PaginatedResult<GroupMemberResponseDTO>(
                listgrMembersDto,
                grMembers.TotalItems,
                grMembers.Page,
                grMembers.PageSize);
        }

        public async Task<GroupMemberResponseDTO> GetByIdAsync(int id)
        {
            var grMember = await _grMemberRepository.GetByIdAsync(id);
            return grMember.GrMemberToGrMemberResponseDTO();
        }

        public async Task<GroupMemberResponseDTO> UpdateAsync(GroupMemberUpdateDTO grMemberUpdateDTO, int id)
        {
            var grMemberToUpdate = await _grMemberRepository.GetByIdAsync(id);

            if (grMemberToUpdate == null)
                throw new AppError("GrMember not found", 404);

            var group = await _grChatRepository.GetByIdAsync(grMemberUpdateDTO.GroupId);

            if (group == null)
                throw new AppError("Group not found", 404);

            var user = await _userRepository.GetUserById(grMemberUpdateDTO.UserId);

            if (user == null)
                throw new AppError("User not found", 404);

            var grMember = grMemberUpdateDTO.GrMemberUpdateDTOToGrMember(grMemberToUpdate);

            grMember.GroupChat = group;
            grMember.User = user;


            var grMemberUpdated = await _grMemberRepository.UpdateAsync(grMember);

            return grMemberUpdated.GrMemberToGrMemberResponseDTO();
        }
    }
}