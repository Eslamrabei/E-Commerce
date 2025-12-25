namespace Service.Implementations
{
    public class BasketService(IBasketRepository _basket, IMapper _mapper) : IBasketService
    {
        public async Task<BasketDto?> CreateOrUpdateAsync(BasketDto basketDto)
        {
            var basket = _mapper.Map<CustomerBasket>(basketDto);
            var result = await _basket.CreateOrUpdateAsync(basket);
            return result is null ? throw new GenericNotFoundException<CustomerBasket, int>(basket.Id, "BasketId") : await GetBasketAsync(basket.Id);
        }

        public async Task<bool> DeleteBasketAsync(string id)
        => await _basket.DaleteAsync(id);
        public async Task<BasketDto?> GetBasketAsync(string id)
        {
            var CustomerResult = await _basket.GetBasketAsync(id);
            return CustomerResult is null ? throw new GenericNotFoundException<CustomerBasket, int>(id, "BasketId") : _mapper.Map<BasketDto>(CustomerResult);
        }
    }
}
