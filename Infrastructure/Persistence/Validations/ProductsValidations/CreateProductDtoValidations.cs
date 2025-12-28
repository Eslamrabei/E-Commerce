using FluentValidation;
using Shared.Dtos.ProductDto;

namespace Persistence.Validations.ProductsValidations
{
    public class CreateProductDtoValidations : AbstractValidator<CreatedProductDto>
    {
        public CreateProductDtoValidations()
        {
            RuleFor(p => p.Name).NotEmpty().MinimumLength(3).MaximumLength(64);
            RuleFor(p => p.Description).NotEmpty().MinimumLength(5).MaximumLength(256);
            RuleFor(p => p.Price).GreaterThan(0).LessThan((decimal)Math.Pow(10, 5));
        }
    }
}
