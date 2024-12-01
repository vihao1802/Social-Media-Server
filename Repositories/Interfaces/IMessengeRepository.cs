using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SocialMediaServer.Models;

namespace SocialMediaServer.Repositories.Interfaces
{
    public interface IMessengeRepository
    {
        Task<List<Messenge>> GetMessagesByRelationshipIdAsync(int relationshipId);
        Task<Messenge> SendMessengeAsync(Messenge message);
        Task DeleteMessengeAsync(Messenge message);
        Task<Messenge> GetMessengeAsyncById(int messengeId);
        Task<Messenge> GetLatestMessageByRelationshipIdAsync(int relationshipId);
    }
}