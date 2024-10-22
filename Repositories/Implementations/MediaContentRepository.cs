using Microsoft.EntityFrameworkCore;
using SocialMediaServer.Data;
using SocialMediaServer.Models;
using SocialMediaServer.Repositories.Interfaces;

namespace SocialMediaServer.Repositories.Implementations
{
    public class MediaContentRepository : IMediaContentRepository
    {
        private readonly ApplicationDBContext _dbContext;

        public MediaContentRepository(ApplicationDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        public Task<List<MediaContent>> GetAllMediaContentAsync()
        {
            var mediaContents = _dbContext.MediaContents.ToListAsync();
            return mediaContents;
        }

        public Task<List<MediaContent>> GetAllMediaContentByPostIdAsync(int postId)
        {
            var mediaContents = _dbContext.MediaContents
                .Where(mediaContent => mediaContent.PostId == postId)
                .ToListAsync();
            return mediaContents;
        }

        public async Task<MediaContent> GetByIdAsync(int id)
        {
            return await _dbContext.MediaContents.FirstOrDefaultAsync(p => p.Id == id) ?? throw new ArgumentNullException(nameof(id));
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
