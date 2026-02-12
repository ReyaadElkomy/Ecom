using System.Net;
using Ecom.Api.Helpers;
using Ecom.Api.Mapping;
using Microsoft.Extensions.Caching.Memory;

namespace Ecom.Api.Middlewares;

public class ExceptionsMiddleware
{
    private readonly RequestDelegate _next;
    private readonly IHostEnvironment _enviroment;
    private readonly IMemoryCache _cache;
    private readonly TimeSpan _rateLimitWindow = TimeSpan.FromSeconds(30);
    public ExceptionsMiddleware(RequestDelegate next, IHostEnvironment enviroment, IMemoryCache cache)
    {
        _next = next;
        _enviroment = enviroment;
        _cache = cache;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            //if(IsRequestAllowed(context) == false)
            //{
            //    context.Response.StatusCode = (int)HttpStatusCode.TooManyRequests;
            //    context.Response.ContentType = "application/json";
            //    var response = new ApiExceptions((int)HttpStatusCode.TooManyRequests, "Too many requests. Please try again later.");
            //    var json = System.Text.Json.JsonSerializer.Serialize(response);
            //    await context.Response.WriteAsync(json);
            //}

            ApplaySecurity(context);
            await _next(context);
        }
        catch (Exception ex)
        {
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            context.Response.ContentType = "application/json";
            var response = _enviroment.IsDevelopment() 
                ? new ApiExceptions((int)HttpStatusCode.InternalServerError, ex.Message, ex.StackTrace)
                : new ApiExceptions((int)HttpStatusCode.InternalServerError, ex.Message);

            var json = System.Text.Json.JsonSerializer.Serialize(response);

            await context.Response.WriteAsync(json);
        }
    }

    private void ApplaySecurity(HttpContext context)
    {
        context.Response.Headers["X-Content-Type-Options"] = "nosniff";
        context.Response.Headers["X-Frame-Options"] = "DENY";
        context.Response.Headers["X-XSS-Protection"] = "1; mode=block";
    }

    //private bool IsRequestAllowed(HttpContext context)
    //{
    //    var ipAddress = context.Connection.RemoteIpAddress?.ToString() ?? "unknown";
    //    var cacheKey = $"RequestCount_{ipAddress}";

    //    var requestData = _cache.GetOrCreate(cacheKey, entry =>
    //    {
    //        entry.AbsoluteExpirationRelativeToNow = _rateLimitWindow;
    //        return 0;
    //    });

    //    requestData++;

    //    if (requestData > 8)
    //        return false;

    //    _cache.Set(cacheKey, requestData, _rateLimitWindow);

    //    return true;
    //}


}
