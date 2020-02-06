using System.Threading.Tasks;
using NinjaApp.Core.Models;

namespace NinjaApp.Core
{
    public interface IRefreshTokenRepository
    {
        RefreshToken CreateRefreshToken(string userId);
        Task<RefreshToken> FindByValueAsync(string value);
        void Refresh(RefreshToken refreshToken);
        void RevokeToken(RefreshToken refreshToken);
    }
}