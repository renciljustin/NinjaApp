using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NinjaApp.Core;
using NinjaApp.Core.Models;

namespace NinjaApp.Persistence {
    public class UserRepository : IUserRepository {
        private readonly NinjaDbContext _context;

        public UserRepository (NinjaDbContext context) {
            _context = context;
        }
        
        public async Task<User> GetUserByUserNameAsync (string userName) {
            return await _context.Users
                .Where(u => u.UserName == userName)
                .FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<User>> GetUsersAsync () {
            return await _context.Users
                .ToListAsync();
        }
    }
}