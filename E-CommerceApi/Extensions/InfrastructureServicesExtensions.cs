using Domain.Contracts;
using Domain.Entities.IdentityModule;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Persistence.Data;
using Persistence.Identity;
using Persistence.Repositories;
using Shared.Common;
using StackExchange.Redis;
using System.Text;

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
            // StoreDbContext
            services.AddScoped<IDataSeeding, DataSeeding>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            // Identity
            services.AddIdentity<User, IdentityRole>(opt =>
            {
                opt.User.RequireUniqueEmail = true;

            }).AddEntityFrameworkStores<IdentityStoreDbContext>();
            services.AddSingleton<IConnectionMultiplexer>((_) =>
            {
                return ConnectionMultiplexer.Connect(_configuration.GetConnectionString("RedisConnection")!);
            });

            services.AddScoped<IBasketRepository, BasketRepository>();
            services.AddScoped<ICacheRepository, CacheRepository>();
            services.ValidateJwt(_configuration);
            return services;
        }

        public static IServiceCollection ValidateJwt(this IServiceCollection services, IConfiguration _configuration)
        {
            // 1] resolve the JwtOptions
            var jwtOptions = _configuration.GetSection("JwtOptions").Get<JwtOptions>();

            // 2] Validate with AddAuthentication 
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = jwtOptions.Issuer,
                    ValidAudience = jwtOptions.Audience,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions.SecretKey))
                };
            });

            services.AddAuthorization();
            return services;

        }


    }
}
