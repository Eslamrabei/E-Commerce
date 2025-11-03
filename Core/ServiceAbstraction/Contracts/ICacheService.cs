namespace ServiceAbstraction.Contracts
{
    public interface ICacheService
    {
        Task<string?> GetCacheAsync(string key);
        Task SetCacheAsync(string key, Object Value, TimeSpan TimeToLive);
    }
}
