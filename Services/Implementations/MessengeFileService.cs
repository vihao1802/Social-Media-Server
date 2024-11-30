using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using SocialMediaServer.ExceptionHandling;
using SocialMediaServer.Services.Interfaces;

namespace SocialMediaServer.Services.Implementations
{
    public class MessengeFileService: IMessengeFileService
    {
        private readonly Cloudinary _cloudinary;
        public MessengeFileService(Cloudinary cloudinary) {
            _cloudinary = cloudinary;
        }

        public async Task<List<string>>  UploadFileMessengerAsync(List<IFormFile> files, string folder)
        {
            var secureUrls = new List<string>();

            foreach (var file in files)
            {
                using var stream = file.OpenReadStream();
                var uploadParams = new RawUploadParams
                {
                    File = new FileDescription(file.FileName, stream),
                    UseFilename = true,
                    Folder = folder,
                };

                var uploadResult = await _cloudinary.UploadAsync(uploadParams);

                if (uploadResult.Error != null)
                {
                    throw new AppError(uploadResult.Error.Message);
                }

                secureUrls.Add(uploadResult.SecureUrl.ToString());
            }

            return secureUrls;
        }
    }
}