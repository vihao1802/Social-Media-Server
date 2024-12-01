using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using DotNetEnv;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages;
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
        private readonly ITokenService _tokenService;

        public AuthService(IAuthRepository AuthRepository, IUserRepository userRepository, IEmailSender emailSender, IUserService userService, ITokenService tokenService)
        {
            _AuthRepository = AuthRepository;
            _UserRepository = userRepository;
            _emailSender = emailSender;
            _userService = userService;
            _tokenService = tokenService;
        }

        public async Task Login(LoginDTO loginDto)
        {
            var user = await _AuthRepository.GetUserByEmail(loginDto.Email) ?? throw new AppError("User not found", 400);

            var check_locked_out = await _AuthRepository.CheckLockedOut(user);

            if (check_locked_out) throw new AppError("User is locked out", 400);

            var login_result = await _AuthRepository.Login(user, loginDto.Password);
            if (!login_result.Succeeded)
                throw new AppError("Invalid password", 400);
        }

        public async Task Register(RegisterDTO registerDto)
        {
            await RegisterUser(registerDto, false);
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

        public AuthenticationProperties ExternalLoginConfig(string provider, string redirectUrl)
        {
            if (redirectUrl == null) throw new AppError("Redirect URL is required", 400);

            var properties = _AuthRepository.ExternalLoginConfig(provider, redirectUrl) ?? throw new AppError("External login failed: Config failed", 500);
            return properties;
        }

        public async Task<LoginResponseDTO> HandleExternalAuthentication(ClaimsIdentity claimsIdentity)
        {

            var id = claimsIdentity?.Claims?.FirstOrDefault(c => c.Type.Contains("nameidentifier"))?.Value ?? throw new AppError("Id not found", 400);

            var name = claimsIdentity?.Name ?? throw new AppError("Name not found", 400);

            var email = claimsIdentity?.Claims?.FirstOrDefault(c => c.Type.Contains("emailaddress"))?.Value ?? throw new AppError("Email not found", 400);

            var pictureUrl = claimsIdentity?.Claims?.FirstOrDefault(c => c.Type.Contains("picture"))?.Value ?? throw new AppError("Profile picture not found", 400);

            var birthday = claimsIdentity?.Claims?.FirstOrDefault(c => c.Type.Contains("birthday"))?.Value ?? throw new AppError("Birthday not found", 400);

            var gender = claimsIdentity?.Claims?.FirstOrDefault(c => c.Type.Contains("gender"))?.Value ?? throw new AppError("Gender not found", 400);


            var user = await _userService.GetUserByEmail(email);

            if (user == null)
            {
                RegisterDTO registerDto = new()
                {
                    Id = id,
                    UserName = name,
                    Email = email,
                    Date_of_birth = DateTime.Parse(birthday),
                    Gender = gender,
                    Profile_img = pictureUrl,
                    Password = $"{Guid.NewGuid()}Aa@1234"
                };
                await RegisterUser(registerDto, true);

                user = await _userService.GetUserByEmail(email);
            }

            return new LoginResponseDTO
            {
                Email = email,
                Token = _tokenService.CreateToken(user),
            };

        }

        private async Task RegisterUser(RegisterDTO registerDTO, bool isFromExternal)
        {
            if (!Validate.IsEmailValid(registerDTO.Email))
                throw new AppError("Invalid email", 400);

            var user = await _AuthRepository.GetUserByEmail(registerDTO.Email);
            if (user != null)
                throw new AppError("Email existed", 400);

            var user_by_username = await _UserRepository.GetUserByUsername(registerDTO.UserName);
            if (user_by_username != null)
                throw new AppError("Username existed", 400);

            var newUser = new User
            {
                UserName = registerDTO.UserName,
                Email = registerDTO.Email,
                Date_of_birth = registerDTO.Date_of_birth,
                Gender = registerDTO.Gender,
                Is_external_user = isFromExternal,
            };

            if (isFromExternal)
            {
                newUser.Profile_img = registerDTO.Profile_img;
                newUser.Id = registerDTO.Id ?? throw new AppError("OpenId is missing when linking google account", 404);
            }

            var created_user_result = await _AuthRepository.Register(newUser, registerDTO.Password);

            if (!created_user_result.Succeeded)
                throw new AppError(created_user_result.Errors.FirstOrDefault().Description, 400);

            return;
        }
    }
}