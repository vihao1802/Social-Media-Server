using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SocialMediaServer.Repositories.Interfaces
{
    public interface IRoleRepository
    {
        Task CreateRole(string roleName);

    }
}