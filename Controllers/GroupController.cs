using Microsoft.AspNetCore.Mvc;
using SocialMediaServer.DTOs.Request.GroupChat;
using SocialMediaServer.DTOs.Request.GroupMess;
using SocialMediaServer.ExceptionHandling;
using SocialMediaServer.Services.Interfaces;

namespace SocialMediaServer.Controllers
{
    [ApiController]
    // [Authorize]
    [Route("api/Group/")]
    public class GroupController(IGroupService groupService, IGroupMemberService grMemberService) : ControllerBase
    {
        private readonly IGroupService _groupService = groupService;
        private readonly IGroupMemberService _grMemberService = grMemberService;

        [HttpGet("{group_id}")]
        public async Task<IActionResult> GetById(int group_id)
        {
            try
            {
                var group = await _groupService.GetByIdAsync(group_id);
                return Ok(group);
            }
            catch (AppError ex)
            {
                return NotFound(ex.Message);
            }
        }
        
        [HttpPost("Create")]
        public async Task<IActionResult> CreateAsync(
            [FromForm] GroupCreateDTO groupCreateDTO)
        {
            var createdGroup = await _groupService.CreateAsync(
                groupCreateDTO);

            return CreatedAtAction(nameof(GetById), 
                new { group_id = createdGroup.Id }, createdGroup);
        }

        [HttpPut("{group_id}")]
        public async Task<IActionResult> UpdateAsync(
            [FromForm] GroupUpdateDTO groupUpdateDTO, int group_id,
            IFormFile? mediaFile)
        {
            try
            {
                var updatedGroup = await _groupService.UpdateAsync(
                    groupUpdateDTO, group_id, mediaFile);
                return Ok(updatedGroup);
            }
            catch (AppError ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpDelete("EndGroup/{id}")]
        public async Task<IActionResult> EndGroup(int id){
            try
            {
                await _groupService.EndGroup(id);
                return NoContent();
            }
            catch (AppError ex)
            {
                return NotFound(ex.Message);
            }
        }

        //chua xong
        [HttpPost("AddMember/{gr_id}/{user_id}")]
        public async Task<IActionResult> addMember(int gr_id, string user_id)
        {
            var createdGroup = await _grMemberService.CreateAsync(gr_id,user_id);

            return CreatedAtAction(nameof(_grMemberService.GetByIdAsync), 
                new { group_id = createdGroup.Id }, createdGroup);
        }

        [HttpDelete("DeleteMember/{member_id}")]
        public async Task<IActionResult> DeleteMember(int member_id){
            try
            {
                await _grMemberService.DeleteAsync(member_id);
                return NoContent();
            }
            catch (AppError ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpGet("GetAllMessenge/{group_id}")]
        public async Task<IActionResult> GetAllMessenge(int group_id, GrMessQueryDTO grMessQueryDTO){
             try
            {
                var listMessenge = await _groupService.GetAllByGroupIdAsync(group_id, grMessQueryDTO);
                return Ok(listMessenge);
            }
            catch (AppError ex)
            {
                return NotFound(ex.Message);
            }
        }

        
    }
}