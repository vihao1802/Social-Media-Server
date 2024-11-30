using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SocialMediaServer.DTOs.Request;
using SocialMediaServer.DTOs.Response;
using SocialMediaServer.Models;

namespace SocialMediaServer.Services.Interfaces
{
    public interface ITokenService
    {
        string CreateToken(UserResponseDTO userResponseDTO);
    }
}