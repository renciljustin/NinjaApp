using System.Threading.Tasks;
using NinjaApp.Core;

namespace NinjaApp.Persistence
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly NinjaDbContext _context;
        public UnitOfWork(NinjaDbContext context)
        {
            _context = context;
        }

        public async Task CompleteAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}