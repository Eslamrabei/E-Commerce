using Persistence.Identity;

namespace Persistence.Repositories
{
    public class RefreshTokenRepository(IdentityStoreDbContext _context) : IRefreshTokenRepository
    {
        public async Task<RefreshToken?> GetByTokenAsync(string Token)
        {
            return await _context.RefreshTokens.AsNoTrackingWithIdentityResolution().Include(u => u.User)
                .FirstOrDefaultAsync(t => t.Token == Token);
        }

        public async Task RemoveAllForUserAsync(string userId)
        {
            var tokens = _context.RefreshTokens.Where(u => u.UserId == userId);
            _context.RefreshTokens.RemoveRange(tokens);
            await _context.SaveChangesAsync();

        }

        public async Task RevokeAsync(RefreshToken refreshToken)
        {
            refreshToken.IsRevoked = true;
            _context.RefreshTokens.Update(refreshToken);
            await _context.SaveChangesAsync();

        }

        public async Task SaveAsync(RefreshToken refreshToken)
        {
            await _context.RefreshTokens.AddAsync(refreshToken);
            await _context.SaveChangesAsync();
        }
    }
}
