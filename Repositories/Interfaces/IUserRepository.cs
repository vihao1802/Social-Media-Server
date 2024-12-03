using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using SocialMediaServer.DTOs.Request.User;
using SocialMediaServer.Models;
using SocialMediaServer.Utils;

namespace SocialMediaServer.Repositories.Interfaces
{
    public interface IUserRepository
    {
        Task<List<User?>> GetAllUsers();
        Task<PaginatedResult<User>> SearchForUser(UserQueryDTO userQueryDTO);

        Task<User?> GetUserById(string id);
        Task<User?> GetUserByClaimPrincipal(ClaimsPrincipal claimsPrincipal);
        Task<User?> GetUserByUsername(string username);
        Task<IdentityResult> UpdateUserInformation(User user);
        Task<IdentityResult> LockUser(User user);
        Task<IdentityResult> UnLockUser(User user);
        Task<IList<string>> GetUsersRoles(User user);


    }
}