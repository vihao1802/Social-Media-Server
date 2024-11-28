using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using SocialMediaServer.DTOs.Response;
using SocialMediaServer.Models;

namespace SocialMediaServer.Services.Interfaces
{
    public interface IRelationshipService
    {

        Task<List<FollowingResponseDTO>> GetUserFollowing(string user_id);
        Task<List<PersonalMessengerResponseDTO>?> GetCurrentUserPersonalMessenger(string user_id);
        Task FollowUser(string sender_id, string receiver_id);
        Task UnFollowUser(string sender_id, string receiver_id);

        Task<List<FollowerResponseDTO>?> GetUserFollower(string user_id);
        Task<List<FollowingResponseDTO>?> GetUserBlockList(string user_id);
        Task BlockUser(string sender_id, string receiver_id);
        Task UnBlockUser(string sender_id, string receiver_id);


    }
}