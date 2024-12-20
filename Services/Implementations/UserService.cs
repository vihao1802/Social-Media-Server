using SocialMediaServer.DTOs.Response;
using SocialMediaServer.Repositories.Interfaces;
using SocialMediaServer.Mappers;
using SocialMediaServer.DTOs.Request;
using Microsoft.AspNetCore.Identity;
using SocialMediaServer.Models;
using SocialMediaServer.Services.Interfaces;
using System.Security.Claims;
using SocialMediaServer.ExceptionHandling;
using Microsoft.AspNetCore.JsonPatch;
using SocialMediaServer.Utils;
using SocialMediaServer.DTOs.Request.User;

namespace SocialMediaServer.Services.Implementations
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        private readonly IAuthRepository _authRepository;
        private readonly IMediaService _mediaService;
        public UserService(IUserRepository userRepository, IAuthRepository authRepository, IMediaService mediaService)
        {
            _userRepository = userRepository;
            _authRepository = authRepository;
            _mediaService = mediaService;
        }

        public async Task<IdentityResult?> LockUser(string id)
        {
            var user = await _userRepository.GetUserById(id);
            if (user == null)
                return null;

            var result = await _userRepository.LockUser(user);
            return result;
        }

        public async Task<IdentityResult?> UnLockUser(string id)
        {
            var user = await _userRepository.GetUserById(id);
            if (user == null)
                return null;

            var result = await _userRepository.UnLockUser(user);
            return result;
        }

        public async Task<List<UserResponseDTO?>> GetAllUsers()
        {
            var users = await _userRepository.GetAllUsers();
            var ListUsersDto = users.Select(user => user?.UserToUserResponseDTO()).ToList();
            return ListUsersDto;
        }

        public async Task<UserResponseDTO?> GetUserById(string id)
        {
            var user = await _userRepository.GetUserById(id);
            return user?.UserToUserResponseDTO();
        }

        public async Task<UserResponseDTO> GetCurrentUser(ClaimsPrincipal principal)
        {
            if (principal != null)
            {
                var user = await _userRepository.GetUserByClaimPrincipal(principal) ?? throw new AppError("Unauthorized !", 401);
                return user.UserToUserResponseDTO();
            }

            throw new AppError("User not found", 404);
        }

        public async Task<UserResponseDTO?> GetUserByUsername(string username)
        {
            var user = await _userRepository.GetUserByUsername(username);
            return user?.UserToUserResponseDTO();
        }

        public async Task<UserResponseDTO?> GetUserByEmail(string email)
        {
            var user = await _authRepository.GetUserByEmail(email);
            return user?.UserToUserResponseDTO();
        }

        public async Task<PaginatedResult<UserResponseDTO>> SearchForUser(UserQueryDTO userQueryDTO)
        {
            var users = await _userRepository.SearchForUser(userQueryDTO);

            var ListUsersDto = users.Items.Select(user => user.UserToUserResponseDTO()).ToList();

            return new PaginatedResult<UserResponseDTO>(ListUsersDto, users.TotalItems, users.PageSize, users.Page);
        }

        public async Task UpdateUserInformation(string userId, UpdateUserDTO updateUserDTO)
        {
            var user = await _userRepository.GetUserById(userId) ?? throw new AppError("User not found", 404);

            user.PhoneNumber = updateUserDTO.PhoneNumber;
            user.Bio = updateUserDTO.Bio;
            user.Date_of_birth = updateUserDTO.Date_of_birth;
            user.Gender = updateUserDTO.Gender != null ? updateUserDTO.Gender : user.Gender;

            await _userRepository.UpdateUserInformation(user);

            return;
        }

        public async Task UpdateUserAvatar(string userId, IFormFile file)
        {
            var user = await _userRepository.GetUserById(userId) ?? throw new AppError("User not found", 404);

            var profile_img = await _mediaService.UploadMediaAsync(file, "SocialMediaProfileImages");

            user.Profile_img = profile_img;

            await _userRepository.UpdateUserInformation(user);
        }
    }
}