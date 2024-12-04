using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using SocialMediaServer.ExceptionHandling;
using SocialMediaServer.Services.Interfaces;

namespace SocialMediaServer.Services.Implementations
{
    public class MediaService : IMediaService
    {
        private readonly Cloudinary _cloudinary;
        public MediaService(Cloudinary cloudinary)
        {
            _cloudinary = cloudinary;
        }

        public async Task DeleteMediaAsync(string url, string folder)
        {
            if (!string.IsNullOrEmpty(url))
            {
                var publicId = GetPublicIdFromUrl(url);
                var deletionParams = new DeletionParams(folder + "/" + publicId);
                var deletionResult = await _cloudinary.DestroyAsync(deletionParams);

                if (deletionResult.Result != "ok")
                {
                    throw new AppError("Failed to delete the old image.");
                }
            }
        }

        public async Task<string> UploadMediaAsync(IFormFile file, string folder)
        {
            using var stream = file.OpenReadStream();

            if (file.ContentType.StartsWith("image/"))
            {
                var uploadParams = new ImageUploadParams
                {
                    File = new FileDescription(file.FileName, stream),
                    Folder = folder,
                };
                var uploadResult = await _cloudinary.UploadLargeAsync(uploadParams);
                if (uploadResult.Error != null)
                {
                    throw new AppError(uploadResult.Error.Message);
                }

                return uploadResult.SecureUrl.ToString();
            }
            else if (file.ContentType.StartsWith("video/"))
            {
                var uploadParams = new VideoUploadParams
                {
                    File = new FileDescription(file.FileName, stream),
                    Folder = folder,
                };
                var uploadResult = await _cloudinary.UploadLargeAsync(uploadParams);
                if (uploadResult.Error != null)
                {
                    throw new AppError(uploadResult.Error.Message);
                }

                return uploadResult.SecureUrl.ToString();
            }
            else
            {
                throw new AppError("Invalid file type.");
            }
        }

        private static string GetPublicIdFromUrl(string url)
        {
            var uri = new Uri(url);
            var segments = uri.Segments;
            var publicIdWithExtension = segments[^1];
            var publicId = Path.GetFileNameWithoutExtension(publicIdWithExtension);
            return publicId;
        }
    }
}
