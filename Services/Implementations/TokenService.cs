using System;
using System.Collections.Generic;
using System.Drawing.Text;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Tokens;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages;
using SocialMediaServer.DTOs.Request;
using SocialMediaServer.Models;
using SocialMediaServer.Services.Interfaces;

namespace SocialMediaServer.Services.Implementations
{
    public class TokenService : ITokenService
    {
        private readonly IConfiguration _config;
        private readonly SymmetricSecurityKey _key;
        public TokenService(IConfiguration config)
        {
            _config = config;
            _key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(_config["JWT:SigningKey"]));
        }
        public string CreateToken(LoginDTO loginDto)
        {
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Email, loginDto.Email),
            };
            var creds = new SigningCredentials(_key, SecurityAlgorithms.HmacSha512Signature);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddDays(30),
                SigningCredentials = creds,
                Issuer = _config["JWT:Issuer"],
                Audience = _config["JWT:Audience"]
            };

            var tokenHanlder = new JwtSecurityTokenHandler();

            var token = tokenHanlder.CreateToken(tokenDescriptor);

            return tokenHanlder.WriteToken(token);

        }
    }
}