using FluentValidation;
using Shared.Dtos.AiSearch;

namespace Persistence.Validations.ProductsValidations
{
    public class UpdateProductDtoValidation : AbstractValidator<UpdateProductDto>
    {
        public UpdateProductDtoValidation()
        {
            RuleFor(p => p.Id).GreaterThan(0).LessThan((int)Math.Pow(10, 3));
            RuleFor(p => p.Name).NotEmpty().MinimumLength(3).MaximumLength(64);
            RuleFor(p => p.Description).NotEmpty().MinimumLength(5).MaximumLength(256);
            RuleFor(p => p.Price).GreaterThan(0).LessThan((decimal)Math.Pow(10, 5));
        }
    }
}
