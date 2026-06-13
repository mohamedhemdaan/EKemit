using EKemit.Core.Entities.Identity;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EKemit.Core.Services.Contract
{
    public interface IAuthService
    {
        //return token              //user whom will generate token for him  
        Task<string> CreateTokenAsync(AppUser user,UserManager<AppUser> userManager);
    }
}
