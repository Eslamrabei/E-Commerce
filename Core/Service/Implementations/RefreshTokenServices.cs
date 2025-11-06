namespace Service.Implementations
{
    public class RefreshTokenServices(IRefreshTokenRepository _refreshToken, IMapper _mapper) : IRefreshTokenServices
    {
        public async Task<RefreshTokenDto?> GetByTokenAsync(string Token)
        {
            var GetToken = await _refreshToken.GetByTokenAsync(Token);
            return _mapper.Map<RefreshTokenDto>(GetToken);
        }
        public async Task RevokeAsync(RefreshTokenDto refreshToken)
        => await _refreshToken.RevokeAsync(_mapper.Map<RefreshToken>(refreshToken));

        public async Task RemoveAllForUserAsync(string userId)
        {
            await _refreshToken.RemoveAllForUserAsync(userId);
        }


        public async Task SaveAsync(RefreshTokenDto refreshToken)
        {
            await _refreshToken.SaveAsync(_mapper.Map<RefreshToken>(refreshToken));
        }
    }
}
