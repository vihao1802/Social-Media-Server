using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Routing;
using SocialMediaServer.DTOs;
using SocialMediaServer.DTOs.Request;
using SocialMediaServer.DTOs.Response;
using SocialMediaServer.Models;
using SocialMediaServer.Repositories.Interfaces;
using SocialMediaServer.Services.Interfaces;
using SocialMediaServer.Utils;

namespace SocialMediaServer.Services.Implementations
{
    public class AuthService : IAuthService
    {
        private readonly IAuthRepository _AuthRepository;
        private readonly IUserRepository _UserRepository;
        private readonly IEmailSender _emailSender;
        public AuthService(IAuthRepository AuthRepository, IUserRepository userRepository, IEmailSender emailSender)
        {
            _AuthRepository = AuthRepository;
            _UserRepository = userRepository;
            _emailSender = emailSender;
        }

        public async Task<SignInResult?> Login(LoginDTO loginDto)
        {
            var user = await _AuthRepository.GetUserByEmail(loginDto.Email);
            if (user == null)
                return null;

            var check_locked_out = await _AuthRepository.CheckLockedOut(user);

            if (check_locked_out)
                return SignInResult.LockedOut;
            var login_result = await _AuthRepository.Login(user, loginDto.Password);
            return login_result;
        }

        public async Task<IdentityResult> Register(RegisterDTO registerDto)
        {
            if (!Validate.IsEmailValid(registerDto.Email))
                return IdentityResult.Failed(new IdentityError { Description = "Email is invalid" });

            var user = await _AuthRepository.GetUserByEmail(registerDto.Email);
            if (user != null)
                return IdentityResult.Failed(new IdentityError { Description = "Email existed!" });

            var user_by_username = await _UserRepository.GetUserByUsername(registerDto.UserName);
            if (user_by_username != null)
                return IdentityResult.Failed(new IdentityError { Description = "Username existed!" });

            var newUser = new User
            {
                UserName = registerDto.UserName,
                Email = registerDto.Email,
                Date_of_birth = registerDto.Date_of_birth,
                Gender = registerDto.Gender,
            };
            var created_user_result = await _AuthRepository.Register(newUser, registerDto.Password);
            return created_user_result;
        }

        public async Task<IdentityResult> Logout()
        {
            var result = await _AuthRepository.Logout();
            return result;
        }

        public async Task<IdentityResult?> ForgotPassword(string email)
        {
            var user_request = await _AuthRepository.GetUserByEmail(email);
            if (user_request == null)
                return null;
            var reset_token = await _AuthRepository.GetPasswordResetToken(user_request);
            try
            {
                string url = $"https://localhost:5001/auth/reset-password?email={email}&token={reset_token}";
                string email_body = $"Click the link below to reset your password {reset_token}";
                await _emailSender.SendEmailAsync(email, "Verify your email", email_body);
                return IdentityResult.Success;

            }
            catch (Exception ex)
            {
                return IdentityResult.Failed(new IdentityError { Description = ex.ToString() });
            }

        }

        public async Task<IdentityResult?> ResetPassword(ResetPasswordDTO resetPasswordDTO)
        {
            var user_request = await _AuthRepository.GetUserByEmail(resetPasswordDTO.Email);
            if (user_request == null)
                return null;

            var validate_password = await _AuthRepository.ValidatePassword(user_request, resetPasswordDTO.NewPassword);
            if (!validate_password.Succeeded)
                return IdentityResult.Failed(new IdentityError { Description = "Password is invalid" });

            var result = await _AuthRepository.ResetPassword(user_request, resetPasswordDTO.ResetToken, resetPasswordDTO.NewPassword);
            return result;
        }

        public async Task<IdentityResult?> UpdatePassword(UpdatePasswordDTO updatePasswordDTO)
        {
            var user_request = await _AuthRepository.GetUserByEmail(updatePasswordDTO.Email);
            if (user_request == null)
                return null;

            var validate_password = await _AuthRepository.ValidatePassword(user_request, updatePasswordDTO.NewPassword);
            if (!validate_password.Succeeded)
                return IdentityResult.Failed(new IdentityError { Description = "Password is invalid" });

            var result = await _AuthRepository.UpdatePassword(user_request, updatePasswordDTO.CurrentPassword, updatePasswordDTO.NewPassword);
            return result;
        }

    }
}