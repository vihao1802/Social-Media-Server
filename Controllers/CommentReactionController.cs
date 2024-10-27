
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SocialMediaServer.DTOs.Request.MediaContent;
using SocialMediaServer.DTOs.Request.Post;
using SocialMediaServer.DTOs.Request.PostViewer;
using SocialMediaServer.ExceptionHandling;
using SocialMediaServer.Services.Interfaces;
namespace SocialMediaServer.Controllers
{

    [ApiController]
    [Authorize]
    [Route("api/commentReaction/")]

    public class CommentReactionController(
        ICommentReactionService commentReactionService) : ControllerBase
    {
        private readonly ICommentReactionService _commentReactionService = commentReactionService;

        [HttpGet("{cmt_id}")]
        public async Task<IActionResult> GetAllByCommentId(int cmt_id, [FromQuery] CommentReactionQueryDTO commentReactionQueryDTO)
        {
            var commentReactions = await _commentReactionService.GetAllByCommentIdAsync(cmt_id, commentReactionQueryDTO);
            return Ok(commentReactions);
        }
         
        [HttpPost]
        public async Task<IActionResult> CreateByCommentId(
            [FromBody] CommentReactionCreateDTO commentReactionCreateDTO)
        {
            var commentReaction = await _commentReactionService.CreateByCommentIdAsync(commentReactionCreateDTO);
            return CreatedAtAction(nameof(CreateByCommentId), commentReaction);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCommentReaction(int id)
        {
            try
            {
                await _commentReactionService.DeleteByIdAsync(id);
                return NoContent();
            }
            catch (AppError ex)
            {
                return NotFound(ex.Message);
            }
        }
    }
}