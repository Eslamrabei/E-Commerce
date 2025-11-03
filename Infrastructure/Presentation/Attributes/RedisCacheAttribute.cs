using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using System.Text;

namespace Presentation.Attributes
{
    public class RedisCacheAttribute(int durationInSecondes = 120) : ActionFilterAttribute
    {
        public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var cacheService = context.HttpContext.RequestServices.GetRequiredService<IServiceManager>().CacheService;
            string key = GenerateKey(context.HttpContext.Request);
            var result = await cacheService.GetCacheAsync(key);
            if (result != null)
            {
                context.Result = new ContentResult()
                {
                    Content = result,
                    ContentType = "application/json",
                    StatusCode = StatusCodes.Status200OK
                };
                return;
            }
            var resultContent = await next.Invoke();
            if (resultContent.Result is ObjectResult objectResult)
            {
                await cacheService.SetCacheAsync(key, objectResult, TimeSpan.FromSeconds(durationInSecondes));
            }

        }

        private string GenerateKey(HttpRequest request)
        {
            var key = new StringBuilder();
            key.Append(request.Path);
            foreach (var item in request.Query.OrderBy(q => q.Key))
            {
                key.Append($"/{item.Key}-{item.Value}");
            }
            return key.ToString();
        }
    }
}
