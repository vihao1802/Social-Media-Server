
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SocialMediaServer.DTOs.Request.MediaContent;
using SocialMediaServer.ExceptionHandling;
using SocialMediaServer.Services.Interfaces;
namespace SocialMediaServer.Controllers
{

    [ApiController]
    [Authorize]
    [Route("api/mediaContent/")]

    public class MediaContentController(
        IMediaContentService mediaContentService) : ControllerBase
    {
        private readonly IMediaContentService _mediaContentService = mediaContentService;

        [HttpGet]
        public async Task<IActionResult> GetAllMediaContent([FromQuery] MediaContentQueryDTO mediaContentQueryDTO)
        {
            var posts = await _mediaContentService.GetAllAsync(mediaContentQueryDTO);
            return Ok(posts);
        }

        [HttpGet("post/{post_id}")]
        public async Task<IActionResult> GetAllByPostId(int post_id, [FromQuery] MediaContentQueryDTO mediaContentQueryDTO)
        {
            var posts = await _mediaContentService.GetAllByPostIdAsync(post_id, mediaContentQueryDTO);
            return Ok(posts);
        }

        [HttpGet("{mediaContent_id}")]
        public async Task<IActionResult> GetById(int mediaContent_id)
        {
            try
            {
                var post = await _mediaContentService.GetByIdAsync(mediaContent_id);
                return Ok(post);
            }
            catch (AppError ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateAsync(
            [FromForm] MediaContentCreateDTO mediaContentCreateDTO, IFormFile mediaFile)
        {
            var createdMediaContent = await _mediaContentService.CreateAsync(
                mediaContentCreateDTO, mediaFile);

            return CreatedAtAction(nameof(GetById),
                new { mediaContent_id = createdMediaContent.Id }, createdMediaContent);
        }

        [HttpPut("{mediaContent_id}")]
        public async Task<IActionResult> UpdateAsync(
            [FromForm] MediaContentUpdateDTO mediaContentUpdateDTO, int mediaContent_id,
            IFormFile mediaFile)
        {
            try
            {
                var updatedMediaContent = await _mediaContentService.UpdateAsync(
                    mediaContentUpdateDTO, mediaContent_id, mediaFile);
                return Ok(updatedMediaContent);
            }
            catch (AppError ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpPatch("{mediaContent_id}")]
        public async Task<IActionResult> PatchAsync(
            [FromForm] MediaContentPatchDTO mediaContentPatchDTO, int mediaContent_id,
            IFormFile? mediaFile)
        {
            try
            {
                var updatedMediaContent = await _mediaContentService.PatchAsync(
                    mediaContentPatchDTO, mediaContent_id, mediaFile);
                return Ok(updatedMediaContent);
            }
            catch (AppError ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpDelete("{mediaContent_id}")]
        public async Task<IActionResult> DeleteAsync(int mediaContent_id)
        {
            try
            {
                await _mediaContentService.DeleteAsync(mediaContent_id);
                return NoContent();
            }
            catch (AppError ex)
            {
                return NotFound(ex.Message);
            }
        }
    }
}