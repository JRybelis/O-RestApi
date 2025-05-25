using Microsoft.Net.Http.Headers;

namespace HotelListing.Net9.Middleware;

public class CacheMiddleware(RequestDelegate next)
{
    public async Task InvokeAsync(HttpContext context)
    {
        context.Response.GetTypedHeaders().CacheControl = new CacheControlHeaderValue()
        {
            Public = true,
            MaxAge = TimeSpan.FromSeconds(10)
        };
        context.Response.Headers[HeaderNames.Vary] = new[] { "Accept-Encoding" };
        
        await next(context);
    }
}