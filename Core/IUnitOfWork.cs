using System.Threading.Tasks;

namespace NinjaApp.Core
{
    public interface IUnitOfWork
    {
         Task CompleteAsync();
    }
}