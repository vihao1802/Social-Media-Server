
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SocialMediaServer.DTOs.Request.Post;
using SocialMediaServer.ExceptionHandling;
using SocialMediaServer.Services.Interfaces;
namespace SocialMediaServer.Controllers
{

    [ApiController]
    // [Authorize]
    [Route("api/post/")]

    public class PostController(IPostService postService) : ControllerBase
    {
        private readonly IPostService _postService = postService;

        [HttpGet]
        public async Task<IActionResult> GetAllPost([FromQuery] PostQueryDTO postQueryDTO)
        {
            var posts = await _postService.GetAllAsync(postQueryDTO);
            return Ok(posts);
        }

        [HttpGet("me")]
        public async Task<IActionResult> GetAllPostByMe([FromQuery] PostQueryDTO postQueryDTO)
        {
            var posts = await _postService.GetAllByMeAsync(postQueryDTO);
            return Ok(posts);
        }

        [HttpGet("user/{user_id}")]
        public async Task<IActionResult> GetAllPostByUserId(string user_id, [FromQuery] PostQueryDTO postQueryDTO)
        {
            var posts = await _postService.GetAllByUserIdAsync(user_id, postQueryDTO);
            return Ok(posts);
        }

        [HttpGet("{post_id}")]
        public async Task<IActionResult> GetPostById(int post_id)
        {
            try
            {
                var post = await _postService.GetByIdAsync(post_id);
                return Ok(post);
            }
            catch (AppError ex)
            {
                return NotFound(ex.Message);
            }
        }
        
        [HttpPost]
        public async Task<IActionResult> CreatePost(
            [FromBody] PostCreateDTO postCreateDTO)
        {
            var createdPost = await _postService.CreateAsync(postCreateDTO);

            return CreatedAtAction(nameof(GetPostById), 
                new { post_id = createdPost.Id }, createdPost);
        }

        [HttpPut("{post_id}")]
        public async Task<IActionResult> UpdatePost(
            [FromBody] PostUpdateDTO post, int post_id)
        {
            try
            {
                var updatedPost = await _postService.UpdateAsync(post, post_id);
                return Ok(updatedPost);
            }
            catch (AppError ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpDelete("{post_id}")]
        public async Task<IActionResult> DeletePost(int post_id)
        {
            try
            {
                await _postService.DeleteAsync(post_id);
                return NoContent();
            }
            catch (AppError ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpPatch("{post_id}")]
        public async Task<IActionResult> PatchPost(
            [FromBody] PostPatchDTO post, int post_id)
        {
            try
            {
                var updatedPost = await _postService.PatchAsync(post, post_id);
                return Ok(updatedPost);
            }
            catch (AppError ex)
            {
                return NotFound(ex.Message);
            }
        }
    }
}