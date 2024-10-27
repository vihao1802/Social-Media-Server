using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using SocialMediaServer.Data;
using SocialMediaServer.DTOs.Request.MediaContent;
using SocialMediaServer.DTOs.Request.Post;
using SocialMediaServer.ExceptionHandling;
using SocialMediaServer.Models;
using SocialMediaServer.Repositories.Interfaces;
using SocialMediaServer.Utils;

namespace SocialMediaServer.Repositories.Implementations
{
    public class MediaContentRepository : IMediaContentRepository
    {
        private readonly ApplicationDBContext _dbContext;

        public MediaContentRepository(ApplicationDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<PaginatedResult<MediaContent>> GetAllMediaContentAsync(MediaContentQueryDTO mediaContentQueryDTO)
        {
            var filterParams = new Dictionary<string, object?>
            {
                {"Id", mediaContentQueryDTO.Id},
                {"Media_type", mediaContentQueryDTO.Media_type },
                {"Media_Url", mediaContentQueryDTO.Media_Url },
                {"PostId", mediaContentQueryDTO.PostId }
            };

            var mediaContents = _dbContext.MediaContents;
            foreach (var mediaContent in mediaContents)
            {
                mediaContent.Post = _dbContext.Posts.FirstOrDefault(p => p.Id == mediaContent.PostId);
            }

            var mediaContentsQuery = mediaContents
                .ApplyIncludes(mediaContentQueryDTO.Includes)
                .ApplyFilters(filterParams)
                .ApplySorting(mediaContentQueryDTO.Sort)
                .ApplyPaginationAsync(mediaContentQueryDTO.Page, mediaContentQueryDTO.PageSize);

            return await mediaContentsQuery;
        }

        public async Task<PaginatedResult<MediaContent>> GetAllMediaContentByPostIdAsync(int postId, MediaContentQueryDTO mediaContentQueryDTO)
        {
            var filterParams = new Dictionary<string, object?>
            {
                {"Id", mediaContentQueryDTO.Id},
                {"Media_type", mediaContentQueryDTO.Media_type },
                {"Media_Url", mediaContentQueryDTO.Media_Url },
                {"PostId", mediaContentQueryDTO.PostId }
            };

            var mediaContents = _dbContext.MediaContents
                .Where(mediaContent => mediaContent.PostId == postId);
                
            foreach (var mediaContent in mediaContents)
            {
                mediaContent.Post = _dbContext.Posts.FirstOrDefault(p => p.Id == mediaContent.PostId);
            }

            var mediaContentsQuery = mediaContents
                .ApplyIncludes(mediaContentQueryDTO.Includes)
                .ApplyFilters(filterParams)
                .ApplySorting(mediaContentQueryDTO.Sort)
                .ApplyPaginationAsync(mediaContentQueryDTO.Page, mediaContentQueryDTO.PageSize);

            return await mediaContentsQuery;
        }

        public async Task<MediaContent> GetByIdAsync(int id)
        {
            var mediaContent = await _dbContext.MediaContents.FirstOrDefaultAsync(p => p.Id == id)
                ?? throw new AppError("Media content not found", 404);

            mediaContent.Post = await _dbContext.Posts.FirstOrDefaultAsync(p => p.Id == mediaContent.PostId);
            
            return mediaContent;
        }

        public async Task<MediaContent> CreateAsync(MediaContent mediaContent)
        {
            _dbContext.MediaContents.Add(mediaContent);
            await _dbContext.SaveChangesAsync();
            return mediaContent;
        }

        public async Task<MediaContent> UpdateAsync(MediaContent mediaContent)
        {
            _dbContext.MediaContents.Update(mediaContent);
            await _dbContext.SaveChangesAsync();
            return mediaContent;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var mdeiaContent = await _dbContext.MediaContents.FirstOrDefaultAsync(p => p.Id == id);
            if (mdeiaContent == null)
            {
                return false;
            }
            _dbContext.MediaContents.Remove(mdeiaContent);
            await _dbContext.SaveChangesAsync();
            return true;
        }
    }
}
