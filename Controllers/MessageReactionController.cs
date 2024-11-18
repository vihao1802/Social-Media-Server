using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SocialMediaServer.DTOs.Request.MessageReaction;
using SocialMediaServer.ExceptionHandling;
using SocialMediaServer.Models;
using SocialMediaServer.Services;
using System.Threading.Tasks;

namespace SocialMediaServer.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/[controller]")]
    public class MessageReactionController : ControllerBase
    {
        private readonly MessageReactionService _messageReactionService;

        public MessageReactionController(MessageReactionService messageReactionService)
        {
            _messageReactionService = messageReactionService;
        }

        [HttpGet]
        public async Task<ActionResult> GetAllReactions([FromQuery] MessageReactionQueryDTO queryDTO)
        {
            var reactions = await _messageReactionService.GetAllReactionsAsync(queryDTO);
            return Ok(reactions);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> GetReactionById(int id)
        {
            try
            {
                var reaction = await _messageReactionService.GetByIdAsync(id);
                return Ok(reaction);
            }
            catch (AppError ex)
            {
                return StatusCode(ex.StatusCode, ex.Message);
            }
        }

        [HttpPost("Create")]
        public async Task<ActionResult> CreateReaction([FromForm] MessageReactionCreateDTO messageReaction)
        {
            var createdReaction = await _messageReactionService.CreateAsync(messageReaction);
            return CreatedAtAction(nameof(GetReactionById), new { id = createdReaction.Id }, createdReaction);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteReaction(int id)
        {
            var isDeleted = await _messageReactionService.DeleteAsync(id);
            if (!isDeleted)
            {
                return NotFound("MessageReaction not found.");
            }
            return NoContent();
        }

        [HttpDelete("groupMessage/{groupMessageId}")]
        public async Task<ActionResult> DeleteReactionsByGroupMessageId(int groupMessageId)
        {
            var isDeleted = await _messageReactionService.DeleteAllByGroupMessageIdAsync(groupMessageId);
            if (!isDeleted)
            {
                return NotFound("No reactions found for the given GroupMessageId.");
            }
            return NoContent();
        }
    }
}
