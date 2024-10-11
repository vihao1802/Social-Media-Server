using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using SocialMediaServer.Models;

namespace SocialMediaServer.Repositories.Interfaces
{
    public interface IUserRepository
    {
        Task<List<User?>> GetAllUsers();
        Task<User?> GetUserById(Guid id);
        Task<User?> GetUserByUsername(string username);
        Task<IdentityResult> UpdateUserInformation(User user);
        Task<IdentityResult> LockUser(User user);
        Task<IdentityResult> UnLockUser(User user);

    }
}