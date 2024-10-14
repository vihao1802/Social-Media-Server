using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using SocialMediaServer.DTOs.Request;
using SocialMediaServer.DTOs.Response;
using SocialMediaServer.Models;

namespace SocialMediaServer.Services.Interfaces
{
    public interface IUserService
    {
        Task<List<UserResponseDTO?>> GetAllUsers();

        Task<UserResponseDTO?> GetUserById(string id);
        Task<UserResponseDTO?> GetUserByUsername(string username);
        Task<UserResponseDTO?> GetUserByEmail(string email);


        Task<List<UserResponseDTO>> SearchForUsers(string search_string);

        Task<IdentityResult?> UpdateUserInformation(UpdateUserDTO updateUserDTO);

        Task<IdentityResult> LockUser(string id);
        Task<IdentityResult> UnLockUser(string id);
    }
}