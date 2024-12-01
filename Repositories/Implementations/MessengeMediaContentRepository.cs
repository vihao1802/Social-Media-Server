using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SocialMediaServer.Data;
using SocialMediaServer.Models;
using SocialMediaServer.Repositories.Interfaces;

namespace SocialMediaServer.Repositories.Implementations
{
    public class MessengeMediaContentRepository: IMessengeMediaContent
    {
        private readonly ApplicationDBContext _context;

        public MessengeMediaContentRepository(ApplicationDBContext context){
            _context = context;
        }

        public async Task<List<MessengeMediaContent>> createMessengeMediaContentAsync(List<MessengeMediaContent> messengeMediaContents){
            await _context.MessengeMediaContents.AddRangeAsync(messengeMediaContents);
            
            await _context.SaveChangesAsync();

            return messengeMediaContents;
        }

    }
}