using Microsoft.AspNetCore.Mvc;
using Shared.ErrorModels;

namespace E_CommerceApi.Factories
{
    public class ApiResponseFactory
    {

        public static IActionResult CustomeValidationApiResponse(ActionContext context)
        {
            var errors = context.ModelState.Where(error => error.Value?.Errors.Any() == true)
                .Select(err => new ValidationError()
                {
                    Field = err.Key,
                    Errors = err.Value?.Errors.Select(e => e.ErrorMessage) ?? []
                });

            var response = new ValidationErrorResponse()
            {
                StatusCode = StatusCodes.Status400BadRequest,
                Message = $"One or more validation happend",
                ValidationErrors = errors
            };

            return new BadRequestObjectResult(response);
        }

    }
}
