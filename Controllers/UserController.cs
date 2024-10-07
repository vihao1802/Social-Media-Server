using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NuGet.Protocol.Core.Types;
using SocialMediaServer.DTOs;
using SocialMediaServer.DTOs.Request;
using SocialMediaServer.DTOs.Response;
using SocialMediaServer.Models;
using SocialMediaServer.Services.Interfaces;

namespace SocialMediaServer.Controllers
{
    [Route("auth/register")]
    [ApiController]
    public class UserController : ControllerBase
    {

        private readonly IUserService _userService;
        private readonly ITokenService _tokenService;
        public UserController(IUserService userService, ITokenService tokenService)
        {
            _userService = userService;
            _tokenService = tokenService;
        }

        [HttpPost]
        public async Task<IActionResult> Register([FromBody] RegisterDTO registerDto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var result = await _userService.Register(registerDto);
                if (!result.Succeeded)
                {
                    return BadRequest(result.Errors.FirstOrDefault()?.Description);
                }
                else
                {
                    return Ok(new RegisterResponseDTO
                    {
                        Username = registerDto.UserName,
                        Email = registerDto.Email,
                        Date_of_birth = registerDto.Date_of_birth
                    });
                }
            }
            catch (System.Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPost("/auth/login")]
        public async Task<IActionResult> Login([FromBody] LoginDTO loginDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var login_result = await _userService.Login(loginDto);
            if (login_result == null)
            {
                return BadRequest("User not exist !");
            }
            else if (login_result.Succeeded)
            {
                return Ok(new LoginResponseDTO
                {
                    Email = loginDto.Email,
                    Token = _tokenService.CreateToken(loginDto)
                });
            }
            else
            {
                return BadRequest("Invalid email or password !");
            }

        }

    }
}