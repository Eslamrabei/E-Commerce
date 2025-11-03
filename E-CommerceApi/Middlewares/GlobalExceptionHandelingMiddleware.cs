using Domain.Exceptions;
using Shared.ErrorDetails;

namespace E_CommerceApi.Middlewares
{
    public class GlobalExceptionHandelingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<GlobalExceptionHandelingMiddleware> _logger;

        public GlobalExceptionHandelingMiddleware(RequestDelegate next, ILogger<GlobalExceptionHandelingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
                if (context.Response.StatusCode == StatusCodes.Status404NotFound)
                    await HandelExceptionApiAsync(context);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An expected error occured");
                await HandelExceptionAsync(context, ex);
            }

        }

        private static async Task HandelExceptionApiAsync(HttpContext context)
        {
            context.Response.ContentType = "application/json";
            var response = new ErrorDetails()
            {
                StatusCode = StatusCodes.Status404NotFound,
                ErrorMessage = $"The end point with url: {context.Request.Path} is not found "
            };
            await context.Response.WriteAsJsonAsync(response);
        }

        private static async Task HandelExceptionAsync(HttpContext context, Exception ex)
        {
            context.Response.ContentType = "application/json";

            var response = new ErrorDetails
            {
                ErrorMessage = ex.Message

            };

            context.Response.StatusCode = ex switch
            {
                NotFoundExceptionHandeling => StatusCodes.Status404NotFound,
                UnauthorizeException => StatusCodes.Status401Unauthorized,
                ValidationException validationException => HandelValidationException(validationException, response),
                _ => StatusCodes.Status500InternalServerError
            };
            response.StatusCode = context.Response.StatusCode;
            await context.Response.WriteAsJsonAsync(response);
        }

        private static int HandelValidationException(ValidationException validationException, ErrorDetails response)
        {
            response.Errors = validationException.Errors;
            return StatusCodes.Status400BadRequest;
        }
    }
}
