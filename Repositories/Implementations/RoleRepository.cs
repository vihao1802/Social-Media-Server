using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using SocialMediaServer.Repositories.Interfaces;

namespace SocialMediaServer.Repositories.Implementations
{
    public class RoleRepository(RoleManager<IdentityRole> roleManager) : IRoleRepository
    {
        private readonly RoleManager<IdentityRole> _roleManager = roleManager;
        public Task CreateRole(string roleName)
        {
            throw new NotImplementedException();
        }
    }
}