using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NinjaApp.Core;
using NinjaApp.Core.Models;

namespace NinjaApp.Persistence
{
    public class RefreshTokenRepository : IRefreshTokenRepository
    {
        private readonly NinjaDbContext _context;

        public RefreshTokenRepository(NinjaDbContext context)
        {
            _context = context;
        }

        public RefreshToken  CreateRefreshToken(string userId)
        {
            var refreshToken = new RefreshToken
            {
                Id = Guid.NewGuid().ToString(),
                UserId = userId,
                Value = Guid.NewGuid().ToString(),
                ExpirationDate = DateTime.UtcNow.AddHours(12),
                TotalRefresh = 0,
                Revoked = false,
                CreationTime = DateTime.UtcNow
            };

            _context.RefreshTokens.Add(refreshToken);

            return refreshToken;
        }

        public async Task<RefreshToken> FindByValueAsync(string value)
        {
            return await _context.RefreshTokens.SingleOrDefaultAsync(rt => !rt.Revoked && rt.Value == value);
        }

        public void Refresh(RefreshToken refreshToken)
        {
            refreshToken.ExpirationDate = DateTime.UtcNow.AddHours(12);
            refreshToken.TotalRefresh++;
            refreshToken.LastModified = DateTime.UtcNow;
        }

        public void RevokeToken(RefreshToken refreshToken)
        {
            refreshToken.Revoked = true;
            refreshToken.LastModified = DateTime.UtcNow;
        }
    }
}