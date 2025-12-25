namespace Service.Implementations
{
    public class ServiceManager(IUnitOfWork _unitOfWork, IMapper _mapper, IBasketRepository _basket, UserManager<User> _userManager, IOptions<JwtOptions> _options, IConfiguration _configuration) /*: IServiceManager*/
    {
        //private readonly Lazy<IProductService> _productService = new(() => new ProductService(_unitOfWork, _mapper));
        //private readonly Lazy<IBasketService> _basketService = new(() => new BasketService(_basket, _mapper));
        //private readonly Lazy<IAuthenticationService> _authenticationService = new(() => new AuthenticationService(_userManager, _options, _mapper));
        //private readonly Lazy<IOrderService> _orderService = new(() => new OrderService(_mapper, _basket, _unitOfWork));
        //private readonly Lazy<IPaymentService> _paymentService = new(() => new PaymentService(_configuration, _basket, _unitOfWork, _mapper));

        //public IProductService ProductService => _productService.Value;
        //public IBasketService BasketService => _basketService.Value;
        //public IAuthenticationService AuthenticationService => _authenticationService.Value;
        //public IOrderService OrderService => _orderService.Value;

        //public IPaymentService PaymentService => _paymentService.Value;
    }
}
