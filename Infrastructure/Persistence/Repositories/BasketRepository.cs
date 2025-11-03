using Domain.Entities.BasketModule;
using StackExchange.Redis;

namespace Persistence.Repositories
{
    public class BasketRepository(IConnectionMultiplexer _connectionMultiplexer) : IBasketRepository
    {
        private readonly IDatabase _database = _connectionMultiplexer.GetDatabase();
        public async Task<CustomerBasket?> CreateOrUpdateAsync(CustomerBasket basket, TimeSpan? time = null)
        {
            var SetCustomer = JsonSerializer.Serialize(basket);
            var result = await _database.StringSetAsync(basket.Id, SetCustomer, time ?? TimeSpan.FromDays(20));
            return result ? await GetBasketAsync(basket.Id) : null;

        }

        public async Task<bool> DaleteAsync(string id)
        => await _database.KeyDeleteAsync(id);

        public async Task<CustomerBasket?> GetBasketAsync(string id)
        {
            var GetBasket = await _database.StringGetAsync(id);
            if (GetBasket.IsNullOrEmpty) return null;
            return JsonSerializer.Deserialize<CustomerBasket>(GetBasket!);

        }

    }
}
