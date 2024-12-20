using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Build.Logging;
using SocialMediaServer.DTOs.Request;
using SocialMediaServer.Services.Interfaces;

namespace SocialMediaServer.Controllers
{
    [ApiController]
    [Route("api/relationship")]
    [Authorize]
    public class RelationshipController(IRelationshipService relationshipService, IUserService userService) : ControllerBase
    {
        private readonly IRelationshipService _relationshipService = relationshipService;
        private readonly IUserService _userService = userService;

        [HttpGet("me/following")]
        public async Task<IActionResult> GetCurrentUserFollowing()
        {
            var userClaim = await _userService.GetCurrentUser(User);

            var list_following = await _relationshipService.GetUserFollowing(userClaim.Id.ToString());

            return Ok(list_following);
        }


        [HttpGet("{user_id}/following")]
        public async Task<IActionResult> GetUserFollowing([FromRoute] string user_id)
        {

            var list_following = await _relationshipService.GetUserFollowing(user_id);

            return Ok(list_following);
        }

        [HttpPost("follow/{user_id}")]
        public async Task<IActionResult> FollowUser([FromRoute] string user_id)
        {

            var senderClaims = await _userService.GetCurrentUser(User);

            if (senderClaims == null)
                return Unauthorized("User is not logged in.");

            if (user_id == null)
                return BadRequest("Receiver id is required.");

            await _relationshipService.FollowUser(senderClaims.Id.ToString(), user_id);

            return CreatedAtAction("FollowUser", new { user_id }, "Followed user successfully.");
        }

        [HttpPost("follow/{request_id}/accept")]
        public async Task<IActionResult> AcceptUserFollowRequest([FromRoute] string request_id)
        {
            var senderClaims = await _userService.GetCurrentUser(User);

            if (request_id == null)
                return BadRequest("Receiver id is required.");

            await _relationshipService.AcceptUserFollowRequest(senderClaims.Id.ToString(), request_id);

            return NoContent();
        }

        [HttpPost("follow/{request_id}/reject")]
        public async Task<IActionResult> RejectUserFollowRequest([FromRoute] string request_id)
        {
            var senderClaims = await _userService.GetCurrentUser(User);

            if (request_id == null)
                return BadRequest("Receiver id is required.");

            await _relationshipService.RejectUserFollowRequest(senderClaims.Id.ToString(), request_id);

            return NoContent();
        }

        [HttpPost("{user_id}/unfollow")]
        public async Task<IActionResult> UnFollowUser([FromRoute] string user_id)
        {

            var senderClaims = await _userService.GetCurrentUser(User);

            if (senderClaims == null)
                return Unauthorized("User is not logged in.");

            if (user_id == null)
                return BadRequest("Receiver id is required.");


            await _relationshipService.UnFollowUser(senderClaims.Id.ToString(), user_id);

            return NoContent();

        }

        [HttpGet("me/follower")]
        public async Task<IActionResult> GetCurrentUserFollower()
        {
            var userClaims = await _userService.GetCurrentUser(User);

            var list_following = await _relationshipService.GetUserFollower(userClaims.Id.ToString());

            return Ok(list_following);

        }

        [HttpGet("{user_id}/follower")]
        public async Task<IActionResult> GetUserFollower([FromRoute] string user_id)
        {
            var list_following = await _relationshipService.GetUserFollower(user_id);

            return Ok(list_following);
        }

        [HttpGet("me/block-list")]
        public async Task<IActionResult> GetCurrentUserBlockList()
        {
            var userClaim = await _userService.GetCurrentUser(User);

            var block_list = await _relationshipService.GetUserBlockList(userClaim.Id.ToString());

            return Ok(block_list);
        }

        [HttpPost("block")]
        public async Task<IActionResult> BlockUser([FromBody] CreateRelationshipDTO createRelationshipDTO)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var senderClaims = await _userService.GetCurrentUser(User);

            await _relationshipService.BlockUser(senderClaims.Id.ToString(), createRelationshipDTO.ReceiverId.ToString());

            return NoContent();
        }

        [HttpPost("unblock")]
        public async Task<IActionResult> UnBlockUser([FromBody] CreateRelationshipDTO createRelationshipDTO)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var senderClaims = await _userService.GetCurrentUser(User);

            if (senderClaims == null)
                return Unauthorized("User is not logged in.");

            await _relationshipService.UnBlockUser(senderClaims.Id.ToString(), createRelationshipDTO.ReceiverId.ToString());

            return NoContent();

        }

        [HttpGet("me/personal-messenger")]
        public async Task<IActionResult> GetCurrentUserPersonalMessenger()
        {
            var userClaims = await _userService.GetCurrentUser(User);

            var list_following = await _relationshipService.GetCurrentUserPersonalMessenger(userClaims.Id.ToString());

            return Ok(list_following);

        }

        [HttpGet("{user_id}/following/get-quantity")]
        public async Task<IActionResult> GetFollowingQuantity([FromRoute] string user_id)
        {
            var quantity = await _relationshipService.GetNumberOfFollowing(user_id);
            return Ok(new { quantity });
        }

        [HttpGet("{user_id}/follower/get-quantity")]
        public async Task<IActionResult> GetFollowerQuantity([FromRoute] string user_id)
        {
            var quantity = await _relationshipService.GetNumberOfFollower(user_id);

            return Ok(new { quantity });
        }
        [HttpGet("recommendation")]
        public async Task<IActionResult> GetRecommendation([FromQuery] RecommendationQueryDTO recommendationQueryDTO)
        {
            var userClaims = await _userService.GetCurrentUser(User);

            var list_recommendation = await _relationshipService.GetRecommendation(userClaims.Id, recommendationQueryDTO);

            return Ok(list_recommendation);
        }
    }
}