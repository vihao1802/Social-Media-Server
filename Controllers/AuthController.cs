using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Build.Framework;
using Microsoft.Extensions.Logging;
using NuGet.Protocol.Core.Types;
using SocialMediaServer.DTOs;
using SocialMediaServer.DTOs.Request;
using SocialMediaServer.DTOs.Response;
using SocialMediaServer.ExceptionHandling;
using SocialMediaServer.Models;
using SocialMediaServer.Services.Interfaces;

namespace SocialMediaServer.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController(IAuthService AuthService, IUserService userService, ITokenService tokenService) : ControllerBase
    {

        private readonly IAuthService _AuthService = AuthService;
        private readonly IUserService _userService = userService;
        private readonly ITokenService _tokenService = tokenService;

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDTO registerDto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                await _AuthService.Register(registerDto);

                return Ok(new RegisterResponseDTO
                {
                    Username = registerDto.UserName,
                    Email = registerDto.Email,
                    Date_of_birth = registerDto.Date_of_birth
                });
            }
            catch (System.Exception ex)
            {
                Console.WriteLine(ex);
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDTO loginDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var new_user = await _userService.GetUserByEmail(loginDto.Email);

            await _AuthService.Login(loginDto);

            return Ok(new LoginResponseDTO
            {
                Email = loginDto.Email,
                Token = _tokenService.CreateToken(new_user)
            });
        }

        [HttpGet("external-login/Google")]
        public IActionResult ExternalLoginGoogle(string returnUrl = "/api")
        {
            // Cấu hình URL để chuyển hướng sau khi xác thực thành công
            var redirectUrl = Url.Action(nameof(ExternalLoginCallback), "Auth", new { returnUrl });

            // Cấu hình các thuộc tính xác thực
            var properties = _AuthService.ExternalLoginConfig("Google", redirectUrl);

            // Chuyển hướng đến nhà cung cấp để bắt đầu xác thực
            return Challenge(properties, "Google");
        }

        [HttpGet("external-login/Facebook")]
        public IActionResult ExternalLoginFacebook(string returnUrl = "/api")
        {
            // Cấu hình URL để chuyển hướng sau khi xác thực thành công
            var redirectUrl = Url.Action(nameof(ExternalLoginCallback), "Auth", new { returnUrl });

            // Cấu hình các thuộc tính xác thực
            var properties = _AuthService.ExternalLoginConfig("Facebook", redirectUrl);

            // Chuyển hướng đến nhà cung cấp để bắt đầu xác thực
            return Challenge(properties, "Facebook");
        }

        [AllowAnonymous]
        [HttpGet("external-login-callback")]
        public async Task<IActionResult> ExternalLoginCallback(string returnUrl = "/")
        {
            // login
            AuthenticateResult info = await HttpContext.AuthenticateAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            var claimsIdentity = info?.Principal?.Identity as ClaimsIdentity ?? throw new AppError("External login failed: Missing claims", 400);

            LoginResponseDTO loginResponseDTO = await _AuthService.HandleExternalAuthentication(claimsIdentity);

            string clientDomain = Environment.GetEnvironmentVariable("CLIENT_DOMAIN") ?? throw new ArgumentException("Front end URL not found");

            return Redirect($"{clientDomain}{returnUrl}?token={loginResponseDTO.Token}&email={loginResponseDTO.Email}");
        }


        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            try
            {
                await _AuthService.Logout();
                return Ok("Logout success !");
            }
            catch (System.Exception)
            {
                return StatusCode(500, "Logout failed! Server error!");
            }

        }

        [HttpPost("forgot-password")]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordDTO fotgotpasswordDto)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values
                        .SelectMany(v => v.Errors)
                        .Select(e => e.ErrorMessage)
                        .ToList();

                throw new AppError($"Invalid params: {string.Join(", ", errors)}", 400);
            }

            await _AuthService.ForgotPassword(fotgotpasswordDto.Email);
            return Ok("Send verify mail success !");
        }

        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordDTO resetPasswordDto)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values
                        .SelectMany(v => v.Errors)
                        .Select(e => e.ErrorMessage)
                        .ToList();

                throw new AppError($"Invalid params: {string.Join(", ", errors)}", 400);
            }

            await _AuthService.ResetPassword(resetPasswordDto);

            return Ok("Reset password success !");

        }

        [HttpPost("update-password")]
        public async Task<IActionResult> UpdatePassword([FromBody] UpdatePasswordDTO updatePasswordDto)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values
                        .SelectMany(v => v.Errors)
                        .Select(e => e.ErrorMessage)
                        .ToList();

                throw new AppError($"Invalid params: {string.Join(", ", errors)}", 400);
            }

            await _AuthService.UpdatePassword(updatePasswordDto, User);

            return Ok(
                "Update password success !"
            );

        }

    }
}