using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NinjaApp.Core;
using NinjaApp.Core.Models;

namespace NinjaApp.Persistence
{
    public class UserRepository : IUserRepository
    {
        private readonly NinjaDbContext _context;
        private readonly IUnitOfWork uow;

        public UserRepository(NinjaDbContext context, IUnitOfWork uow)
        {
            this.uow = uow;
            _context = context;
        }

        public async Task<User> GetUserByUserNameAsync(string userName)
        {
            return await _context.Users
                .Where(u => u.UserName == userName)
                .FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<User>> GetUsersAsync()
        {
            return await _context.Users
                .ToListAsync();
        }

        public async Task<User> UpdateUserAsync(User oldUser, User newUser)
        {
            oldUser.FirstName = newUser.FirstName;
            oldUser.LastName = newUser.LastName;
            oldUser.BirthDate = newUser.BirthDate;
            oldUser.Email = newUser.Email;

            await uow.CompleteAsync();

            return oldUser;
        }
    }
}