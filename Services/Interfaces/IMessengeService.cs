using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SocialMediaServer.DTOs.Request;
using SocialMediaServer.DTOs.Response;
using SocialMediaServer.Models;

namespace SocialMediaServer.Services.Interfaces
{
    public interface IMessengeService
    {
        Task<List<MessengeResponseDTO>> GetMessagesByRelationshipIdAsync(int relationshipId);
        Task SendMessengeAsync(string senderId, int relationshipId, int? replytoId ,string? content, List<string>? filesName);
        Task DeleteMessengeAsync(string senderId, int messengeId);
        Task<MessengeResponseDTO?> GetLatestMessageByRelationshipIdAsync(int relationshipId);
    }
}