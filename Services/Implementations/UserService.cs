using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using SocialMediaServer.DTOs;
using SocialMediaServer.DTOs.Request;
using SocialMediaServer.DTOs.Response;
using SocialMediaServer.Models;
using SocialMediaServer.Repositories.Interfaces;
using SocialMediaServer.Services.Interfaces;
using SocialMediaServer.Utils;

namespace SocialMediaServer.Services.Implementations
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<SignInResult>? Login(LoginDTO loginDto)
        {
            var user = await _userRepository.GetUserByEmail(loginDto.Email);
            if (user == null)
                return null;

            var login_result = await _userRepository.Login(user, loginDto.Password);
            return login_result;
        }

        public async Task<IdentityResult> Register(RegisterDTO registerDto)
        {
            if (!Validate.IsEmailValid(registerDto.Email))
                return IdentityResult.Failed(new IdentityError { Description = "Email is invalid" });

            var user = await _userRepository.GetUserByEmail(registerDto.Email);
            if (user != null)
                return IdentityResult.Failed(new IdentityError { Description = "Email existed!" });

            var newUser = new User
            {
                UserName = registerDto.UserName,
                Email = registerDto.Email,
                Date_of_birth = registerDto.Date_of_birth,
                Gender = registerDto.Gender,
            };
            var created_user_result = await _userRepository.Register(newUser, registerDto.Password);
            return created_user_result;
        }


    }
}