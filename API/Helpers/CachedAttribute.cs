using System.Text;
using Microsoft.AspNetCore.Mvc.Filters;

namespace API.Helpers
{
    public class CachedAttribute : Attribute, IAsyncActionFilter
    {
        private readonly int _TimeToLiveInSeconds;
        public CachedAttribute(int TimeToLiveInSeconds)
        {
            _TimeToLiveInSeconds = TimeToLiveInSeconds;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var cacheService = context.HttpContext.RequestServices.GetRequiredService<IResponseCacheService>();

            // To build a cache key based on the query string parameters.
            var cacheKey = GenerateCacheKeyFromRequest(context.HttpContext.Request);

            // Get the data that the client requests from the cache
            var cacheResponse = await cacheService.GetCachedResponse(cacheKey);

            // If the data was in the cache then return it to the client.
            if (!string.IsNullOrEmpty(cacheResponse))
            {
                var contentResult = new ContentResult
                {
                    Content = cacheResponse,
                    ContentType = "application/json",
                    StatusCode = 200
                };
                context.Result = contentResult;

                return;
            }

            // If the data is not in the cache then move the request to the controller to get the data form DB.
            var executeContext = await next();

            // After getting the data from the DB, Cache it so any client ask for it find it in the cache.
            if (executeContext.Result is OkObjectResult okObjectResult)
            {
                await cacheService.CacheResponseAsync(cacheKey, okObjectResult.Value,
                    TimeSpan.FromSeconds(_TimeToLiveInSeconds));
            }
        }

        private string GenerateCacheKeyFromRequest(HttpRequest request)
        {
            var keyBuilder = new StringBuilder();

            keyBuilder.Append($"{request.Path}");

            // To iterate over the query string values to generate the key from them.
            foreach (var (key, value) in request.Query.OrderBy(x => x.Key))
            {
                keyBuilder.Append($"|{key}-{value}");
            }

            return keyBuilder.ToString();
        }
    }
}