
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SocialMediaServer.DTOs.Request.Post;
using SocialMediaServer.DTOs.Request.PostViewer;
using SocialMediaServer.ExceptionHandling;
using SocialMediaServer.Services.Interfaces;
namespace SocialMediaServer.Controllers
{

    [ApiController]
    [Authorize]
    [Route("api/postViewer/")]

    public class PostViewerController(
        IPostViewerService postViewerService) : ControllerBase
    {
        private readonly IPostViewerService _postViewerService = postViewerService;

        [HttpGet("{post_id}")]
        public async Task<IActionResult> GetAllByPostId(int post_id)
        {
            var posts = await _postViewerService.GetAllByPostIdAsync(post_id);
            return Ok(posts);
        }
         
        [HttpPost]
        public async Task<IActionResult> CreateByPostId(
            [FromBody] PostViewerCreateDTO postViewerCreateDTO)
        {
            var postViewer = await _postViewerService.CreateByPostIdAsync(postViewerCreateDTO);
            return CreatedAtAction(nameof(CreateByPostId), postViewer);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePost(int id)
        {
            try
            {
                await _postViewerService.DeleteByIdAsync(id);
                return NoContent();
            }
            catch (AppError ex)
            {
                return NotFound(ex.Message);
            }
        }
    }
}