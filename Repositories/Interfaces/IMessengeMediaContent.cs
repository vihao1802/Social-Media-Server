using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SocialMediaServer.Models;

namespace SocialMediaServer.Repositories.Interfaces
{
    public interface IMessengeMediaContent
    {
        public Task<List<MessengeMediaContent>>  createMessengeMediaContentAsync(List<MessengeMediaContent> messengeMediaContent);
    }
}