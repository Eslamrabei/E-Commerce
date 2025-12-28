using Persistence.Implementations;

namespace E_CommerceApi.Extensions
{
    public static class InfrastructureServicesExtensions
    {
        public static IServiceCollection AddInfrastrucureService(this IServiceCollection services, IConfiguration _configuration)
        {
            services.AddDbContext<StoreDbContext>(options =>
            {
                options.UseSqlServer(_configuration.GetConnectionString("DefaultConnection"));
            });
            services.AddDbContext<IdentityStoreDbContext>(options =>
            {
                options.UseSqlServer(_configuration.GetConnectionString("IdentityConnection"));
            });
            services.AddSingleton<IConnectionMultiplexer>((_) =>
            {
                return ConnectionMultiplexer.Connect(_configuration.GetConnectionString("RedisConnection")!);
            });




            // StoreDbContext
            services.AddScoped<IDataSeeding, DataSeeding>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            // Identity
            services.AddIdentity<User, IdentityRole>(opt =>
            {
                opt.User.RequireUniqueEmail = true;

            }).AddEntityFrameworkStores<IdentityStoreDbContext>()
            .AddDefaultTokenProviders();
            services.AddSingleton<IConnectionMultiplexer>((_) =>
            {
                return ConnectionMultiplexer.Connect(_configuration.GetConnectionString("RedisConnection")!);
            });

            services.AddHttpClient<IEmbeddingService, OllamaEmbeddingService>();
            services.AddHttpClient<IOllamaService, OllamaService>();
            services.AddHttpClient<IGroqService, GroqService>();
            services.AddSingleton<IVectorService, VectorDbService>();
            services.AddScoped<JwtOptions>();
            services.AddScoped<IProductRepository, ProductRepository>();
            services.AddScoped<IBasketRepository, BasketRepository>();
            services.AddScoped<ICacheRepository, CacheRepository>();
            services.AddScoped<IRefreshTokenRepository, RefreshTokenRepository>();
            services.Configure<JwtOptions>(_configuration.GetSection("JwtOptions"));
            services.ValidateJwt(_configuration);


            return services;
        }

        public static IServiceCollection ValidateJwt(this IServiceCollection services, IConfiguration _configuration)
        {
            var jwtOptions = _configuration.GetSection("JwtOptions").Get<JwtOptions>();

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.SaveToken = true;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidIssuer = jwtOptions.Issuer,
                    ValidateAudience = true,
                    ValidAudience = jwtOptions.Audience,
                    ValidateLifetime = false,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions.SecretKey))
                };
            });
            services.AddAuthorization();

            return services;

        }


    }
}
