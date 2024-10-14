using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Build.Logging;
using SocialMediaServer.Services.Interfaces;

namespace SocialMediaServer.Controllers
{
    [ApiController]
    [Route("api/relationship")]
    [Authorize]
    public class RelationshipController : ControllerBase
    {
        private readonly IRelationshipService _relationshipService;
        private readonly IUserService _userService;
        public RelationshipController(IRelationshipService relationshipService, IUserService userService)
        {
            _relationshipService = relationshipService;
            _userService = userService;
        }


        [HttpGet("me/following")]
        public async Task<IActionResult> GetCurrentUserFollowing()
        {
            var email = HttpContext.Items["Email"] as string;
            if (email != null)
            {
                var user = await _userService.GetUserByEmail(email);

                var list_following = await _relationshipService.GetUserFollowing(user.Id);

                return Ok(list_following);

            }
            return Unauthorized("User is not logged in.");
        }

        [HttpGet("{user_id}/following")]
        public async Task<IActionResult> GetUserFollowing([FromRoute] string user_id)
        {

            var list_following = await _relationshipService.GetUserFollowing(user_id);

            if (list_following == null)
                return BadRequest("User does not exist.");
            return Ok(list_following);



        }

        [HttpPost("following")]
        public async Task<IActionResult> FollowUser([FromBody] string user_id)
        {
            try
            {
                var sender = await _userService.GetUserByEmail(HttpContext.Items["Email"] as string);

                if (sender == null)
                    return Unauthorized("User is not logged in.");

                if (user_id == null)
                    return BadRequest("Receiver id is required.");

                await _relationshipService.FollowUser(sender.Id, user_id);

                return CreatedAtAction("FollowUser", new { user_id }, "Followed user successfully.");

            }
            catch (System.Exception ex)
            {
                Console.WriteLine(ex);
                return StatusCode(500, "Internal Server Error");
            }

        }

        [HttpPost("{user_id}/unfollow")]
        public async Task<IActionResult> UnFollowUser([FromRoute] string user_id)
        {
            try
            {
                var sender = await _userService.GetUserByEmail(HttpContext.Items["Email"] as string);

                if (sender == null)
                    return Unauthorized("User is not logged in.");

                if (user_id == null)
                    return BadRequest("Receiver id is required.");


                await _relationshipService.UnFollowUser(sender.Id, user_id);

                return NoContent();

            }
            catch (System.Exception ex)
            {
                Console.WriteLine(ex);
                return StatusCode(500, "Internal Server Error");
            }
        }

        [HttpGet("me/follower")]
        public async Task<IActionResult> GetCurrentUserFollower()
        {
            if (HttpContext.Items["Email"] is string email)
            {
                var user = await _userService.GetUserByEmail(email);

                var list_following = await _relationshipService.GetUserFollower(user.Id);

                return Ok(list_following);

            }
            return Unauthorized("User is not logged in.");
        }

        [HttpGet("{user_id}/follower")]
        public async Task<IActionResult> GetUserFollower([FromRoute] string user_id)
        {
            var list_following = await _relationshipService.GetUserFollower(user_id);

            if (list_following == null)
                return BadRequest("User does not exist.");
            return Ok(list_following);
        }

        [HttpGet("me/block-list")]
        public async Task<IActionResult> GetCurrentUserBlockList()
        {
            return Ok();
        }

        [HttpPost("block")]
        public async Task<IActionResult> BlockUser([FromBody] string user_id)
        {
            return Ok();
        }

        [HttpPost("unblock")]
        public async Task<IActionResult> UnBlockUser([FromBody] string user_id)
        {
            return Ok();
        }
    }
}