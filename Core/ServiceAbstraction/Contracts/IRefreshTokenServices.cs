namespace ServiceAbstraction.Contracts
{
    public interface IRefreshTokenServices
    {
        Task<RefreshTokenDto?> GetByTokenAsync(string Token);
        Task RevokeAsync(RefreshTokenDto refreshToken);
        Task SaveAsync(RefreshTokenDto refreshToken);
        Task RemoveAllForUserAsync(string userId);
    }
}
