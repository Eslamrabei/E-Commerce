namespace ServiceAbstraction.Contracts
{
    public interface IServiceManager
    {
        public IProductService ProductService { get; }
        public IBasketService BasketService { get; }
        public IAuthenticationService AuthenticationService { get; }
        public IOrderService OrderService { get; }
        public IPaymentService PaymentService { get; }
        public ICacheService CacheService { get; }
        public IRefreshTokenServices RefreshTokenServices { get; }
    }
}
