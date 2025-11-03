namespace ServiceAbstraction.Contracts
{
    public interface IBasketService
    {
        Task<BasketDto?> GetBasketAsync(string id);
        Task<BasketDto?> CreateOrUpdateAsync(BasketDto basketDto);
        Task<bool> DeleteBasketAsync(string id);
    }
}
