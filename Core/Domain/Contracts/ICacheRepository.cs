namespace Domain.Contracts
{
    public interface ICacheRepository
    {
        Task<string?> GetCacheAsync(string key);
        Task SetCacheAsync(string key, Object Value, TimeSpan TimeToLive);
    }
}
