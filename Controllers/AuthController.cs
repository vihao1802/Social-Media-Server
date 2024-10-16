using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
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
    [Route("auth")]
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
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDTO loginDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var login_result = await _AuthService.Login(loginDto);
            if (login_result == null)
            {
                return BadRequest("User not exist !");
            }
            else if (login_result == Microsoft.AspNetCore.Identity.SignInResult.LockedOut)
            {
                return StatusCode(403, "User is locked out !");
            }
            else if (login_result.Succeeded)
            {
                var new_user = await _userService.GetUserByEmail(loginDto.Email);
                return Ok(new LoginResponseDTO
                {
                    Email = loginDto.Email,
                    Token = _tokenService.CreateToken(new_user)
                });
            }
            else
            {
                return BadRequest("Invalid email or password !");
            }

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
                return BadRequest(ModelState);
            try
            {
                var send_verify_mail = await _AuthService.ForgotPassword(fotgotpasswordDto.Email);
                if (send_verify_mail == null)
                {
                    return BadRequest("User not exist !");
                }
                else if (send_verify_mail.Succeeded)
                {
                    return Ok("Send verify mail success !");
                }
                else
                {
                    return BadRequest("Send verify mail failed !");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }

        }

        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordDTO resetPasswordDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            try
            {
                var result = await _AuthService.ResetPassword(resetPasswordDto);
                if (result == null)
                {
                    return BadRequest("User not exist !");
                }
                else if (result.Succeeded)
                {
                    return Ok("Reset password success !");
                }
                else
                {
                    return BadRequest($"Reset password failed !{result.Errors.FirstOrDefault()?.Description}");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPost("update-password")]
        public async Task<IActionResult> UpdatePassword([FromBody] UpdatePasswordDTO updatePasswordDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            try
            {
                var result = await _AuthService.UpdatePassword(updatePasswordDto);

                if (result == null)
                {
                    return BadRequest("User not exist !");
                }
                else if (result.Succeeded)
                {
                    return Ok(
                        "Update password success !"
                    );
                }
                else
                {
                    return BadRequest($"Update password failed ! {result.Errors.FirstOrDefault()?.Description}");
                }
            }
            catch (System.Exception)
            {

                throw;
            }
        }

    }
}