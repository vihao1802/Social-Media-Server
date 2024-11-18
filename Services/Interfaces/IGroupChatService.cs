using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using SocialMediaServer.DTOs.Request;
using SocialMediaServer.DTOs.Request.GroupChat;
using SocialMediaServer.DTOs.Response;
using SocialMediaServer.Models;
using SocialMediaServer.Utils;

namespace SocialMediaServer.Services.Interfaces
{
    public interface IGroupChatService
    {
        Task<List<GroupChatResponseDTO>> SearchForNames(string search_string);
        Task<GroupChatResponseDTO> GetByIdAsync(int id);
        Task<PaginatedResult<GroupChatResponseDTO>> GetAllAsync(GroupChatQueryDTO grChatQueryDTO);
        Task<PaginatedResult<GroupChatResponseDTO>> GetAllByUserAsync(GroupChatQueryDTO grChatQueryDTO, string userId);
        Task<GroupChatResponseDTO> CreateAsync(GroupChatCreateDTO postCreateDTO, IFormFile? mediaFile);
        Task<GroupChatResponseDTO> UpdateAsync(UpdateGrChatDTO updateDto, int id);
        Task<bool> DeleteAsync(int id);
        Task<bool> DeleteGrAsync(int id);
        Task TransferAdminAsync(int groupChatId, string newAdminId);
    }
}