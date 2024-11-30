using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SocialMediaServer.DTOs.Request.GroupMember;
using SocialMediaServer.ExceptionHandling;
using SocialMediaServer.Services.Interfaces;

namespace SocialMediaServer.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/GroupMember/")]
    public class GroupMemberController : ControllerBase
    {
        private readonly IGroupMemberService _grMemberService;
        public GroupMemberController(IGroupMemberService groupMemberService)
        {
            _grMemberService = groupMemberService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] GroupMemberQueryDTO GroupMemberQueryDTO)
        {
            var grMembers = await _grMemberService.GetAllAsync(GroupMemberQueryDTO);
            return Ok(grMembers);
        }

        [HttpGet("GetAllByGroup/{group_id}")]
        public async Task<IActionResult> GetAllByGroupId(int group_id, [FromQuery] GroupMemberQueryDTO GroupMemberQueryDTO)
        {
            var grMembers = await _grMemberService.GetAllByGroupIdAsync(group_id, GroupMemberQueryDTO);
            return Ok(grMembers);
        }

        [HttpGet("GetAllByUser/{user_id}")]
        public async Task<IActionResult> GetAllByUserId(string user_id, [FromQuery] GroupMemberQueryDTO GroupMemberQueryDTO)
        {
            var grMembers = await _grMemberService.GetAllByUserIdAsync(user_id, GroupMemberQueryDTO);
            return Ok(grMembers);
        }

        [HttpGet("GetById/{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var grMember = await _grMemberService.GetByIdAsync(id);
                return Ok(grMember);
            }
            catch (AppError ex)
            {
                return NotFound(ex.Message);
            }
        }
          
        [HttpPost("Create")]
        public async Task<IActionResult> CreateAsync(
            [FromForm] GroupMemberCreateDTO grMemberCreateDTO)
        {
            var createdGrMember = await _grMemberService.CreateAsync(grMemberCreateDTO);

            return CreatedAtAction(nameof(GetById), 
                new { id = createdGrMember.Id }, createdGrMember);
        }

        [HttpPut("Update/{id}")]
        public async Task<IActionResult> UpdateAsync(
            [FromForm] GroupMemberUpdateDTO grMemberUpdateDTO, int id)
        {
            try
            {
                var updatedGrMember = await _grMemberService.UpdateAsync(
                    grMemberUpdateDTO, id);
                return Ok(updatedGrMember);
            }
            catch (AppError ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            try
            {
                await _grMemberService.DeleteAsync(id);
                return NoContent();
            }
            catch (AppError ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpDelete("OutGroup/{memberId}")]
        public async Task<IActionResult> OutGroupAsync(int memberId)
        {
            try
            {
                await _grMemberService.OutGroupAsync(memberId);
                return NoContent();
            }
            catch (AppError ex)
            {
                return NotFound(ex.Message);
            }
        }

    }
}