using System.Collections.Generic;
using System.Threading.Tasks;
using NinjaApp.Core.Models;

namespace NinjaApp.Core
{
    public interface IUserRepository
    {
         Task<IEnumerable<User>> GetUsersAsync();
         Task<User> GetUserByUserNameAsync(string userName);
         Task<User> UpdateUserAsync(User oldUser, User newUser);
    }
}