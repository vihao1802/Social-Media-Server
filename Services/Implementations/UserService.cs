using SocialMediaServer.DTOs.Response;
using SocialMediaServer.Repositories.Interfaces;
using SocialMediaServer.Mappers;
using SocialMediaServer.DTOs.Request;
using Microsoft.AspNetCore.Identity;
using SocialMediaServer.Models;
using SocialMediaServer.Services.Interfaces;

namespace SocialMediaServer.Services.Implementations
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        private readonly IAuthRepository _authRepository;
        public UserService(IUserRepository userRepository, IAuthRepository authRepository)
        {
            _userRepository = userRepository;
            _authRepository = authRepository;
        }

        public async Task<IdentityResult?> LockUser(Guid id)
        {
            var user = await _userRepository.GetUserById(id);
            if (user == null)
                return null;

            var result = await _userRepository.LockUser(user);
            return result;
        }

        public async Task<IdentityResult?> UnLockUser(Guid id)
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

        public async Task<UserResponseDTO?> GetUserById(Guid id)
        {
            var user = await _userRepository.GetUserById(id);
            return user?.UserToUserResponseDTO();
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



        public Task<List<UserResponseDTO>> SearchForUsers(string search_string)
        {
            throw new NotImplementedException();
        }

        public async Task<IdentityResult?> UpdateUserInformation(UpdateUserDTO updateUserDTO)
        {
            var user = await _userRepository.GetUserById(updateUserDTO.Id);
            if (user == null)
                return null;

            var check_unique_email = await GetUserByEmail(updateUserDTO.Email);
            if (check_unique_email != null && !check_unique_email.Id.Equals(updateUserDTO.Id))
                return IdentityResult.Failed(new IdentityError { Code = "Email", Description = "Email already exists!" });

            var check_unique_username = await GetUserByUsername(updateUserDTO.Username);
            if (check_unique_username != null && !check_unique_username.Id.Equals(updateUserDTO.Id))
                return IdentityResult.Failed(new IdentityError { Code = "Username", Description = "Username already exists!" });

            user.UserName = updateUserDTO.Username;
            user.Email = updateUserDTO.Email;
            user.PhoneNumber = updateUserDTO.PhoneNumber;
            user.Bio = updateUserDTO.Bio;
            user.Date_of_birth = updateUserDTO.Date_of_birth;
            user.Profile_img = updateUserDTO.Profile_img;
            user.Gender = updateUserDTO.Gender;

            var update_result = await _userRepository.UpdateUserInformation(user);

            return update_result;
        }
    }
}