
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SocialMediaServer.DTOs.Request;
using SocialMediaServer.Services.Interfaces;
namespace SocialMediaServer.Controllers
{

    [ApiController]
    [Authorize]
    [Route("api/user")]

    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet]
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

        [HttpGet("/me")]
        public async Task<IActionResult> GetCurrentUser()
        {
            var email = HttpContext.Items["Email"] as string;
            if (email != null)
            {
                var user = await _userService.GetUserByEmail(email);
                return Ok(user);
            }
            return Unauthorized("User is not logged in.");
        }


        [HttpGet(":username")]
        public async Task<IActionResult> GetUserByUsername([FromQuery] string username)
        {
            var user = await _userService.GetUserByUsername(username);
            if (user == null)
                return NotFound();
            return Ok(user);
        }


        [HttpGet(":email")]
        public async Task<IActionResult> GetUserByEmail([FromQuery] string email)
        {
            var user = await _userService.GetUserByEmail(email);
            if (user == null)
                return NotFound();
            return Ok(user);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateUser(UpdateUserDTO updateUserDTO)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var update_result = await _userService.UpdateUserInformation(updateUserDTO);

            if (update_result == null)
                return BadRequest("User not found!");

            if (!update_result.Succeeded)
                return BadRequest($"{update_result.Errors.FirstOrDefault()?.Description}");

            return NoContent();
        }


        [HttpDelete("/lock/{user_id}")]
        public async Task<IActionResult> DeleteUser(string user_id)
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