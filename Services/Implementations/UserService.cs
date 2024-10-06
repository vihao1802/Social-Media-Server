using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using SocialMediaServer.DTOs;
using SocialMediaServer.Models;
using SocialMediaServer.Repositories.Interfaces;
using SocialMediaServer.Services.Interfaces;

namespace SocialMediaServer.Services.Implementations
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<IdentityResult> Register(UserDTO userDto)
        {
            var newUser = new User
            {
                UserName = userDto.UserName,
                Email = userDto.Email,
                Date_of_birth = userDto.Date_of_birth,
                Gender = userDto.Gender,
            };
            var created_user_result = await _userRepository.Register(newUser, userDto.Password);
            return created_user_result;
        }


    }
}