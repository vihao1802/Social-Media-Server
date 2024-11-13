using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SocialMediaServer.Services.Interfaces
{
    public interface IMessengeFileService
    {
        Task<List<string>> UploadFileMessengerAsync(List<IFormFile> files, string folder);
    }
}