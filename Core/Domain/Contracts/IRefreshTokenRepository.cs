using Domain.Entities.IdentityModule;

namespace Domain.Contracts
{
    public interface IRefreshTokenRepository
    {
        Task<RefreshToken?> GetByTokenAsync(string Token);
        Task RevokeAsync(RefreshToken refreshToken);
        Task RemoveAllForUserAsync(string userId);
        Task SaveAsync(RefreshToken refreshToken);
    }
}
