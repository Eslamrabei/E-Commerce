namespace Service.Implementations
{
    public class ServiceManagerWithDelegateFactory(Func<IProductService> _productFactory, Func<IBasketService> _basketFactory,
        Func<IAuthenticationService> _authenticationFactory, Func<IOrderService> _orderFactory,
        Func<IPaymentService> _paymentFactory, Func<ICacheService> _cacheFactory)
        : IServiceManager
    {

        public IProductService ProductService => _productFactory.Invoke();

        public IBasketService BasketService => _basketFactory.Invoke();

        public IAuthenticationService AuthenticationService => _authenticationFactory.Invoke();

        public IOrderService OrderService => _orderFactory.Invoke();

        public IPaymentService PaymentService => _paymentFactory.Invoke();

        public ICacheService CacheService => _cacheFactory.Invoke();
    }
}
