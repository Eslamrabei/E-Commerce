using FluentValidation;
using Persistence.Validations;

namespace E_CommerceApi.Extensions
{
    public static class FluentValidationsDI
    {
        public static IServiceCollection EnsureValidations(this IServiceCollection services)
        {
            services.AddValidatorsFromAssemblyContaining<ValidationAssembly>();

            return services;
        }
    }
}
