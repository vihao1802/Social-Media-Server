using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SocialMediaServer.DTOs.Request.GroupChat;
using SocialMediaServer.ExceptionHandling;
using SocialMediaServer.Services.Interfaces;
using System.Threading.Tasks;

namespace SocialMediaServer.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/groupchat/")]
    public class GroupChatController : ControllerBase
    {
        private readonly IGroupChatService _groupChatService;

        public GroupChatController(IGroupChatService groupChatService)
        {
            _groupChatService = groupChatService;
        }

        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAllGroupChats([FromQuery] GroupChatQueryDTO groupChatQueryDTO)
        {
            var groupChats = await _groupChatService.GetAllAsync(groupChatQueryDTO);
            return Ok(groupChats);
        }

        [HttpGet("GetAllByUser/{userId}")]
        public async Task<IActionResult> GetAllByUsers([FromQuery] GroupChatQueryDTO groupChatQueryDTO, string userId)
        {
            var groupChats = await _groupChatService.GetAllByUserAsync(groupChatQueryDTO,userId);
            return Ok(groupChats);
        }

        [HttpGet("GetById/{groupChatId}")]
        public async Task<IActionResult> GetGroupChatById(int groupChatId)
        {
            try
            {
                var groupChat = await _groupChatService.GetByIdAsync(groupChatId);
                return Ok(groupChat);
            }
            catch (AppError ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpPost("Create")]
        public async Task<IActionResult> CreateGroupChat([FromForm] GroupChatCreateDTO groupChatCreateDTO,IFormFile? mediaFile)
        {
            var createdGroupChat = await _groupChatService.CreateAsync(groupChatCreateDTO, mediaFile);
            return CreatedAtAction(nameof(GetGroupChatById), new { groupChatId = createdGroupChat.Id }, createdGroupChat);
        }

        [HttpPut("Update/{groupChatId}")]
        public async Task<IActionResult> UpdateGroupChat([FromForm] UpdateGrChatDTO groupChatUpdateDTO, int groupChatId)
        {
            try
            {
                var updatedGroupChat = await _groupChatService.UpdateAsync(groupChatUpdateDTO, groupChatId);
                return Ok(updatedGroupChat);
            }
            catch (AppError ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpPut("TransferAdmin/{groupChatId}")]
        public async Task<IActionResult> TransferAdmin(int groupChatId, [FromBody] TransferAdminDTO transferAdminDTO)
        {
            try
            {
                await _groupChatService.TransferAdminAsync(groupChatId, transferAdminDTO.NewAdminId);
                return Ok("Admin transferred successfully");
            }
            catch (AppError ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpDelete("Delete/{groupChatId}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteGroupChat(int groupChatId)
        {
            try
            {
                await _groupChatService.DeleteAsync(groupChatId);
                return NoContent();
            }
            catch (AppError ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpDelete("Disband/{groupChatId}")]
        public async Task<IActionResult> DisbandGroupChat(int groupChatId)
        {
            try
            {
                await _groupChatService.DeleteGrAsync(groupChatId);
                return NoContent();
            }
            catch (AppError ex)
            {
                return NotFound(ex.Message);
            }
        }
    }
}
