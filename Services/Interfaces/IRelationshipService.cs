using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SocialMediaServer.Models;

namespace SocialMediaServer.Services.Interfaces
{
    public interface IRelationshipService
    {

        Task<List<User>> GetFollowing(Guid user_id);
        Task<List<User>> GetCurrentUserFollowing();
        Task<List<User>> GetFollowers(Guid user_id);
    }
}