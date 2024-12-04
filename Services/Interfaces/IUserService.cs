using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using SocialMediaServer.DTOs.Request;
using SocialMediaServer.DTOs.Request.User;
using SocialMediaServer.DTOs.Response;
using SocialMediaServer.Models;
using SocialMediaServer.Utils;

namespace SocialMediaServer.Services.Interfaces
{
    public interface IUserService
    {
        Task<List<UserResponseDTO?>> GetAllUsers();

        Task<UserResponseDTO?> GetUserById(string id);
        Task<UserResponseDTO> GetCurrentUser(ClaimsPrincipal principal);

        Task<UserResponseDTO?> GetUserByUsername(string username);
        Task<UserResponseDTO?> GetUserByEmail(string email);

        Task<PaginatedResult<UserResponseDTO>> SearchForUser(UserQueryDTO userQueryDTO);

        Task UpdateUserInformation(string userId, UpdateUserDTO updateUserDTO);

        Task<IdentityResult> LockUser(string id);
        Task<IdentityResult> UnLockUser(string id);

        Task UpdateUserAvatar(string userId, IFormFile file);
    }
}