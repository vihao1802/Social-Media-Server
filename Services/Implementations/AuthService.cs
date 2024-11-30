using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using DotNetEnv;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Routing;
using SocialMediaServer.DTOs;
using SocialMediaServer.DTOs.Request;
using SocialMediaServer.DTOs.Response;
using SocialMediaServer.ExceptionHandling;
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
        private readonly IUserService _userService;

        public AuthService(IAuthRepository AuthRepository, IUserRepository userRepository, IEmailSender emailSender, IUserService userService)
        {
            _AuthRepository = AuthRepository;
            _UserRepository = userRepository;
            _emailSender = emailSender;
            _userService = userService;
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

        public async Task Register(RegisterDTO registerDto)
        {
            if (!Validate.IsEmailValid(registerDto.Email))
                throw new AppError("Invalid email", 400);

            var user = await _AuthRepository.GetUserByEmail(registerDto.Email);
            if (user != null)
                throw new AppError("Email existed", 400);

            var user_by_username = await _UserRepository.GetUserByUsername(registerDto.UserName);
            if (user_by_username != null)
                throw new AppError("Username existed", 400);

            var newUser = new User
            {
                UserName = registerDto.UserName,
                Email = registerDto.Email,
                Date_of_birth = registerDto.Date_of_birth,
                Gender = registerDto.Gender,
            };

            var created_user_result = await _AuthRepository.Register(newUser, registerDto.Password);

            if (!created_user_result.Succeeded)
                throw new AppError(created_user_result.Errors.FirstOrDefault().Description, 400);

            return;
        }

        public async Task<IdentityResult> Logout()
        {
            var result = await _AuthRepository.Logout();
            return result;
        }

        public async Task ForgotPassword(string email)
        {
            var user_request = await _AuthRepository.GetUserByEmail(email) ?? throw new AppError("User not found", 400);

            var reset_token = await _AuthRepository.GetPasswordResetToken(user_request);

            string clientDomain = Environment.GetEnvironmentVariable("CLIENT_DOMAIN") ?? throw new ArgumentException("Front end URL not found");
            string url = $"{clientDomain}/api/auth/reset-password?token={reset_token}&email={email}";
            string email_body = $"Click the link below to reset your password {url}";

            try
            {
                await _emailSender.SendEmailAsync(email, "Verify your email", email_body);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw new AppError("Send email failed ", 400);
            }

        }

        public async Task ResetPassword(ResetPasswordDTO resetPasswordDTO)
        {
            var user_request = await _AuthRepository.GetUserByEmail(resetPasswordDTO.Email) ?? throw new AppError("User not found", 400);

            var validate_password = await _AuthRepository.ValidatePassword(user_request, resetPasswordDTO.NewPassword);
            if (!validate_password.Succeeded)
                throw new AppError("Password is invalid", 400);

            var result = await _AuthRepository.ResetPassword(user_request, resetPasswordDTO.ResetToken, resetPasswordDTO.NewPassword);

            if (!result.Succeeded)
                throw new AppError("Invalid token", 400);

        }

        public async Task UpdatePassword(UpdatePasswordDTO updatePasswordDTO, ClaimsPrincipal principal)
        {
            var user_request = await _UserRepository.GetUserByClaimPrincipal(principal) ?? throw new AppError("User not found", 400);

            var validate_password = await _AuthRepository.ValidatePassword(user_request, updatePasswordDTO.NewPassword);

            if (!validate_password.Succeeded)
                throw new AppError("Invalid password: " + validate_password.Errors.FirstOrDefault()?.Description, 400);

            var result = await _AuthRepository.UpdatePassword(user_request, updatePasswordDTO.CurrentPassword, updatePasswordDTO.NewPassword);

            if (!result.Succeeded) throw new AppError("Update password failed: " + result.Errors.FirstOrDefault()?.Description, 400);
        }
    }
}