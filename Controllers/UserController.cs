
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using SocialMediaServer.DTOs.Request;
using SocialMediaServer.ExceptionHandling;
using SocialMediaServer.Services.Interfaces;
namespace SocialMediaServer.Controllers
{

    [ApiController]
    [Authorize]
    [Route("api/user")]

    public class UserController(IUserService userService) : ControllerBase
    {
        private readonly IUserService _userService = userService;

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAllUser()
        {
            var users = await _userService.GetAllUsers();
            return Ok(users);
        }

        [HttpGet("{user_id}")]
        public async Task<IActionResult> GetUserById(string user_id)
        {
            var user = await _userService.GetUserById(user_id);
            if (user == null)
                return NotFound();
            return Ok(user);
        }

        [HttpGet("me")]
        public async Task<IActionResult> GetCurrentUser()
        {
            var current_user = await _userService.GetCurrentUser(User);

            return Ok(current_user);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateUser(UpdateUserDTO updateUserDTO)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var current_user = await _userService.GetCurrentUser(User);

            await _userService.UpdateUserInformation(current_user.Id, updateUserDTO);

            return NoContent();
        }


        [HttpDelete("/lock/{user_id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> LockUser(string user_id)
        {
            var delete_result = await _userService.LockUser(user_id);
            if (delete_result == null)
                return BadRequest("User not found!");

            if (!delete_result.Succeeded)
                return StatusCode(500, $"{delete_result.Errors.FirstOrDefault()?.Description}");
            else
                return NoContent();
        }
        [HttpPut("/unlock/{user_id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UnlockUser(string user_id)
        {
            var unlock_result = await _userService.UnLockUser(user_id);

            if (unlock_result == null)
                return BadRequest("User not found!");

            if (!unlock_result.Succeeded)
                return StatusCode(500, $"{unlock_result.Errors.FirstOrDefault()?.Description}");
            else
                return NoContent();

        }


    }
}