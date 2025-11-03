using Microsoft.OpenApi.Models;

namespace E_CommerceApi.Extensions
{
    public static class WebApplicationExtensions
    {
        public static IServiceCollection AddWebApiService(this IServiceCollection services, IConfiguration _configuration)
        {
            services.AddControllers();
            services.AddCors(options =>
            {
                options.AddPolicy("AngularPolicy", builder =>
                {
                    builder.AllowAnyHeader().AllowAnyMethod()
                    .WithOrigins(_configuration["URLS:FrontUrl"]);
                });
            });
            services.ConfigurationSwagger();


            return services;
        }

        public static IServiceCollection ConfigurationSwagger(this IServiceCollection services)
        {
            services.AddEndpointsApiExplorer();

            services.AddSwaggerGen(opt =>
            {
                opt.SwaggerDoc("v1", new OpenApiInfo { Title = "E-Commerce_API", Version = "v1" });
                opt.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Description = "Please enter token",
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    BearerFormat = "JWT",
                    Scheme = "bearer"
                });
                opt.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type=ReferenceType.SecurityScheme,
                                Id="Bearer"
                            }
                        },
                        new string[]{}
                    }
                });
            });

            return services;
        }
    }
}
