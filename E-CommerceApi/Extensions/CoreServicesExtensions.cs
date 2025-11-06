namespace E_CommerceApi.Extensions
{
    public static class CoreServicesExtensions
    {
        public static IServiceCollection AddCoreService(this IServiceCollection services, IConfiguration _configuration)
        {
            services.AddScoped<IServiceManager, ServiceManagerWithDelegateFactory>();

            services.AddAutoMapper(val => { }, typeof(AssemblyReferenc).Assembly);

            services.Configure<ApiBehaviorOptions>(options =>
            {
                options.InvalidModelStateResponseFactory = ApiResponseFactory.CustomeValidationApiResponse;
            });

            services.AddScoped<IProductService, ProductService>();
            services.AddScoped<IBasketService, BasketService>();
            services.AddScoped<IAuthenticationService, AuthenticationService>();
            services.AddScoped<IOrderService, OrderService>();
            services.AddScoped<IPaymentService, PaymentService>();
            services.AddScoped<ICacheService, CacheService>();
            services.AddScoped<IRefreshTokenServices, RefreshTokenServices>();


            services.AddScoped<Func<IProductService>>(provider => () => provider.GetRequiredService<IProductService>());
            services.AddScoped<Func<IBasketService>>(provider => () => provider.GetRequiredService<IBasketService>());
            services.AddScoped<Func<IAuthenticationService>>(provider => () => provider.GetRequiredService<IAuthenticationService>());
            services.AddScoped<Func<IOrderService>>(provider => () => provider.GetRequiredService<IOrderService>());
            services.AddScoped<Func<IPaymentService>>(provider => () => provider.GetRequiredService<IPaymentService>());
            services.AddScoped<Func<ICacheService>>(provider => () => provider.GetRequiredService<ICacheService>());
            services.AddScoped<Func<IRefreshTokenServices>>(provider => () => provider.GetRequiredService<IRefreshTokenServices>());


            return services;
        }
    }
}
