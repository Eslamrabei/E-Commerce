using Domain.Entities.BasketModule;

namespace Domain.Contracts
{
    public interface IBasketRepository
    {
        Task<CustomerBasket?> GetBasketAsync(string id);
        Task<CustomerBasket?> CreateOrUpdateAsync(CustomerBasket basket, TimeSpan? time = null);
        Task<bool> DaleteAsync(string id);
    }
}
