using EKemit.Core.Services.Contract;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Drawing.Printing;
using System.Text;
using System.Xml.Linq;

namespace EKemit.APIs.Helpers
{
    public class CachedAttribute : Attribute, IAsyncActionFilter
    {
        private readonly int _timeToLiveInSeconds;

        public CachedAttribute(int timeToLiveInSeconds)
        {
            _timeToLiveInSeconds = timeToLiveInSeconds;
        }
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var responseCacheService = context.HttpContext.RequestServices.GetRequiredService<IResponseCacheService>();
            //Ask CLR Object From "ResponseCacheService" Explicitly

            var cacheKey = GenerateCacheKeyFromRequest(context.HttpContext.Request);
            var response = await responseCacheService.GetCachedResponseAsync(cacheKey);

            if (!string.IsNullOrEmpty(response))
            {
                var result = new ContentResult()
                {
                    Content = response,
                    ContentType = "application/json",
                    StatusCode = 200

                };

                context.Result = result;
                return;
            }

            //If Response Not Cached ?
            var executedActionContext = await next.Invoke(); // Will Execute the action(endpoint) itself 

            if (executedActionContext.Result is OkObjectResult okObjectResult && okObjectResult.Value is not null)
            {
                await responseCacheService.CacheResponseAsync(cacheKey, okObjectResult.Value, TimeSpan.FromSeconds(_timeToLiveInSeconds));
            }
        }

        private string GenerateCacheKeyFromRequest(HttpRequest request)
        {
            //{{BaseUrl}}/api/products?pageIndex=1&pageSize=5&sort=name

            var keyBuilder = new StringBuilder();
            keyBuilder.Append(request.Path); // /api/products

            //pageIndex = 1
            //pageSize = 5
            //sort = name

            foreach (var (key, value) in request.Query.OrderBy(x => x.Key))
            {
                keyBuilder.Append($"|{key}-{value}");
                // /api/products|pageIndex-1
                // /api/products|pageIndex-1|pageSize-5
                // /api/products|pageIndex-1|pageSize-5|sort-name
            }
            return keyBuilder.ToString();
        }
    }
}
