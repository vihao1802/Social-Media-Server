using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SocialMediaServer.DTOs.Request.GroupChat;
using SocialMediaServer.DTOs.Request.GroupMess;
using SocialMediaServer.DTOs.Response;
using SocialMediaServer.ExceptionHandling;
using SocialMediaServer.Services.Interfaces;
using System.Threading.Tasks;

namespace SocialMediaServer.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/GroupMessenge")]
    public class GroupMessengeController : ControllerBase
    {
        private readonly IGroupMessengeService _groupMessengeService;
        private readonly IUserService _userService;

        public GroupMessengeController(IGroupMessengeService groupMessengeService, IUserService userService)
        {
            _groupMessengeService = groupMessengeService;
            _userService = userService;
        }

        [HttpPost]
        [Route("Create")]
        public async Task<IActionResult> CreateAsync([FromForm] GrMessCreateDTO grMessCreateDTO)
        {
            try
            {
                var result = await _groupMessengeService.CreateAsync(grMessCreateDTO);
                return Ok(result);
            }
            catch (AppError ex)
            {
                return StatusCode(ex.StatusCode, ex.Message);
            }
        }

        [HttpDelete]
        [Route("DeleteAllByGroupId/{groupId}")]
        public async Task<IActionResult> DeleteAllByGroupIdAsync(int groupId)
        {
            try
            {
                var success = await _groupMessengeService.DeleteAllByGroupIdAsync(groupId);
                if (success)
                    return NoContent();
                else
                    return NotFound("Group not found or no messages to delete");
            }
            catch (AppError ex)
            {
                return StatusCode(ex.StatusCode, ex.Message);
            }
        }

        [HttpDelete]
        [Route("Delete/{id}")]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            try
            {
                var success = await _groupMessengeService.DeleteAsync(id);
                if (success)
                    return NoContent();
                else
                    return NotFound("Message not found");
            }
            catch (AppError ex)
            {
                return StatusCode(ex.StatusCode, ex.Message);
            }
        }

        [HttpDelete]
        [Route("Recall/{id}")]
        public async Task<IActionResult> RecallAsync(int id)
        {
            try
            {
                var success = await _groupMessengeService.RecallAsync(id);
                if (success)
                    return NoContent();
                else
                    return NotFound("Message not found");
            }
            catch (AppError ex)
            {
                return StatusCode(ex.StatusCode, ex.Message);
            }
        }

        [HttpGet]
        [Route("GetAllByGroupId/{groupId}")]
        public async Task<IActionResult> GetAllByGroupIdAsync(int groupId, [FromQuery] GrMessQueryDTO grMessQueryDTO)
        {
            var userClaims = await _userService.GetCurrentUser(User);
            try
            {
                var result = await _groupMessengeService.GetAllByGroupIdAsync(groupId, grMessQueryDTO, userClaims.Id.ToString());
                return Ok(result);
            }
            catch (AppError ex)
            {
                return StatusCode(ex.StatusCode, ex.Message);
            }
        }

        [HttpGet]
        [Route("GetById/{id}")]
        public async Task<IActionResult> GetByIdAsync(int id)
        {
            var userClaims = await _userService.GetCurrentUser(User);

            try
            {
                var grMess = await _groupMessengeService.GetAllByGroupIdAsync(id, new GrMessQueryDTO(),userClaims.Id.ToString());
                if (grMess == null)
                    return NotFound("Message not found");
                return Ok(grMess);
            }
            catch (AppError ex)
            {
                return StatusCode(ex.StatusCode, ex.Message);
            }
        }
    }
}
