namespace SocialMediaServer.Services.Interfaces
{
    public interface IMediaService
    {
        Task DeleteMediaAsync(string url, string folder);
        Task<string> UploadMediaAsync(IFormFile file, string folder);
    }
}
