
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SocialMediaServer.DTOs.Request.MediaContent;
using SocialMediaServer.ExceptionHandling;
using SocialMediaServer.Services.Interfaces;
namespace SocialMediaServer.Controllers
{

    [ApiController]
    // [Authorize]
    [Route("api/comment/")]

    public class CommentController(
        ICommentService commentService) : ControllerBase
    {
        private readonly ICommentService _commentService = commentService;

        [HttpGet]
        public async Task<IActionResult> GetAllComment([FromQuery] CommentQueryDTO commentQueryDTO)
        {
            var comments = await _commentService.GetAllAsync(commentQueryDTO);
            return Ok(comments);
        }

        [HttpGet("post/{post_id}")]
        public async Task<IActionResult> GetAllByPostId(int post_id, [FromQuery] CommentQueryDTO commentQueryDTO)
        {
            var comments = await _commentService.GetAllByPostIdAsync(post_id, commentQueryDTO);
            return Ok(comments);
        }

        [HttpGet("user/{user_id}")]
        public async Task<IActionResult> GetAllByUserId(string user_id, [FromQuery] CommentQueryDTO commentQueryDTO)
        {
            var comments = await _commentService.GetAllByUserIdAsync(user_id, commentQueryDTO);
            return Ok(comments);
        }

        [HttpGet("{comment_id}")]
        public async Task<IActionResult> GetById(int comment_id)
        {
            try
            {
                var comment = await _commentService.GetByIdAsync(comment_id);
                return Ok(comment);
            }
            catch (AppError ex)
            {
                return NotFound(ex.Message);
            }
        }
          
        [HttpPost]
        public async Task<IActionResult> CreateAsync(
            [FromForm] CommentCreateDTO commentCreateDTO, IFormFile? mediaFile)
        {
            var createdComment = await _commentService.CreateAsync(
                commentCreateDTO, mediaFile);

            return CreatedAtAction(nameof(GetById), 
                new { comment_id = createdComment.Id }, createdComment);
        }

        [HttpPut("{comment_id}")]
        public async Task<IActionResult> UpdateAsync(
            [FromForm] CommentUpdateDTO commentUpdateDTO, int comment_id,
            IFormFile? mediaFile)
        {
            try
            {
                var updatedComment = await _commentService.UpdateAsync(
                    commentUpdateDTO, comment_id, mediaFile);
                return Ok(updatedComment);
            }
            catch (AppError ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpPatch("{comment_id}")]
        public async Task<IActionResult> PatchAsync(
            [FromForm] CommentPatchDTO commentPatchDTO, int comment_id,
            IFormFile? mediaFile)
        {
            try
            {
                var updatedComment = await _commentService.PatchAsync(
                    commentPatchDTO, comment_id, mediaFile);
                return Ok(updatedComment);
            }
            catch (AppError ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpDelete("{comment_id}")]
        public async Task<IActionResult> DeleteAsync(int comment_id)
        {
            try
            {
                await _commentService.DeleteAsync(comment_id);
                return NoContent();
            }
            catch (AppError ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpDelete("post/{post_id}")]
        [Authorize(Roles ="Admin")]
        public async Task<IActionResult> DeleteAllByPostIdAsync(int post_id)
        {
            try
            {
                await _commentService.DeleteAllByPostIdAsync(post_id);
                return NoContent();
            }
            catch (AppError ex)
            {
                return NotFound(ex.Message);
            }
        }
    }
}