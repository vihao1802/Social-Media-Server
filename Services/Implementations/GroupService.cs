using System.Security.Claims;
using SocialMediaServer.DTOs.Request.GroupChat;
using SocialMediaServer.DTOs.Request.GroupMess;
using SocialMediaServer.DTOs.Response;
using SocialMediaServer.ExceptionHandling;
using SocialMediaServer.Mappers;
using SocialMediaServer.Models;
using SocialMediaServer.Repositories.Interfaces;
using SocialMediaServer.Services.Interfaces;

namespace SocialMediaServer.Services.Implementations
{
    public class GroupService : IGroupService
    {
        private readonly IUserRepository _userRepository;
        private readonly IGroupRepository _groupRepository;
        private readonly IGroupMemberRepository _grMemberRepository;
        private readonly IMediaService _mediaService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public GroupService(IUserRepository userRepository, IGroupRepository groupRepository, IMediaService mediaService
                            ,IGroupMemberRepository grMemberRepository, IHttpContextAccessor httpContextAccessor)
        {
            _userRepository = userRepository;
            _groupRepository = groupRepository;
            _mediaService = mediaService;
            _grMemberRepository = grMemberRepository;
            _httpContextAccessor = httpContextAccessor;
        }
        public async Task<GroupResponseDTO> CreateAsync(GroupCreateDTO groupCreateDTO)
        {
            var user1 = await _userRepository.GetUserById(groupCreateDTO.CreaterId);
            if(user1 == null)
                    throw new AppError("CreaterId not found", 404);
            if (groupCreateDTO.MembersId.Count < 2)
                throw new AppError("Must have at least 2 members", 404);

            List<User> user2 = new List<User>();
            foreach(var memberId in groupCreateDTO.MembersId){
                var user = await _userRepository.GetUserById(memberId);
                if(user == null)
                    throw new AppError("MemberId not found", 404);
                user2.Add(user);
            }

            var group = groupCreateDTO.GroupCreateDTOToGroup();

            if (groupCreateDTO.MediaFile != null)
            {
                // Danh sách các đuôi file ảnh hợp lệ
                var allowedExtensions = new List<string> { ".jpg", ".jpeg", ".png", ".gif" };
                string fileExtension = Path.GetExtension(groupCreateDTO.MediaFile.FileName).ToLower();

                // Kiểm tra xem đuôi file có nằm trong danh sách cho phép không
                if (allowedExtensions.Contains(fileExtension))
                {
                    // Upload file nếu hợp lệ
                    string mediaUrl = await _mediaService.UploadMediaAsync(groupCreateDTO.MediaFile, "GroupContent");
                    group.Group_avt = mediaUrl;
                }
                else
                {
                    throw new AppError("Media file is not valid", 400);
                }
            }

            var groupCreated = await _groupRepository.CreateAsync(group);
            //tạo thành viên
            await _grMemberRepository.CreateAsync(new GroupMember{
                    GroupChatId = groupCreated.Id,
                    GroupChat = groupCreated,
                    UserId = groupCreateDTO.CreaterId,
                    User = user1
                });
            foreach(var memberId in groupCreateDTO.MembersId){
                await _grMemberRepository.CreateAsync(new GroupMember{
                    GroupChatId = groupCreated.Id,
                    GroupChat = groupCreated,
                    UserId = memberId,
                    User = user2.Find(u => u.Id == memberId)
                });
            }
            return groupCreated.GroupToGroupResponseDTO();
        }

        public async Task<bool> EndGroup(int id)
        {
            var gr = await _groupRepository.GetByIdAsync(id);
            if(gr == null)
                    throw new AppError("Group not found", 404);
            gr.isDelete = true;
            await _groupRepository.UpdateAsync(gr);
            return true;
        }

        public async Task<GroupMessengeDTO> GetAllByGroupIdAsync(int grId, GrMessQueryDTO grMessQueryDTO)
        {
            var user = await GetCurrentUserAsync();
            var member = await _grMemberRepository.GetByUserIdAndGroupId(grId,user.Id);
            if(member == null)
                throw new AppError("Member not found", 404);
            var grMess = await _groupRepository.GetAllByGroupIdAsync(grId,grMessQueryDTO);
            return grMess;
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

        public async Task<GroupResponseDTO> GetByIdAsync(int id)
        {
            var group = await _groupRepository.GetByIdAsync(id);
            return group.GroupToGroupResponseDTO();
        }

        public async Task<GroupResponseDTO> UpdateAsync(GroupUpdateDTO grUpdateDTO, int id, IFormFile? mediaFile)
        {
            var groupToUpdate = await _groupRepository.GetByIdAsync(id);
            if (groupToUpdate == null)
                throw new AppError("Group not found", 404);

            var user1 = await _userRepository.GetUserById(grUpdateDTO.AdminId);
            if(user1 == null)
                    throw new AppError("Admin not found", 404);

            // await CheckPermissionAsync(groupToUpdate.UserId);

            var group = grUpdateDTO.GroupChatUpdateDTOToGroupChat(groupToUpdate);

            if (mediaFile != null)
            {
                // Danh sách các đuôi file ảnh hợp lệ
                var allowedExtensions = new List<string> { ".jpg", ".jpeg", ".png", ".gif" };
                string fileExtension = Path.GetExtension(mediaFile.FileName).ToLower();

                // Kiểm tra xem đuôi file có nằm trong danh sách cho phép không
                if (allowedExtensions.Contains(fileExtension))
                {
                    await _mediaService.DeleteMediaAsync(groupToUpdate.Group_avt, "GroupContent");
                    // Upload file nếu hợp lệ
                    string mediaUrl = await _mediaService.UploadMediaAsync(mediaFile, "GroupContent");
                    group.Group_avt = mediaUrl;
                }
                else
                {
                    throw new AppError("Media file is not valid", 400);
                }
            }

            var groupUpdated = await _groupRepository.UpdateAsync(group);

            return groupUpdated.GroupToGroupResponseDTO();
        }
    }
}