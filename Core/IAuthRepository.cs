using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using NinjaApp.Core.Models;

namespace NinjaApp.Core
{
    public interface IAuthRepository
    {
         Task<IdentityResult> AddToRoleAsync(User user, string role);
         Task<SignInResult> CheckPasswordAsync(User user, string password);
         Task<IdentityResult> CreateUserAsync(User user, string password);
         Task<User> FindByIdAsync(string id);
         Task<User> FindByUserNameAsync(string userName);
         Task<IEnumerable<string>> GetRolesAsync(User user);
    }
}