using ApiKeyAuth.Models;

namespace ApiKeyAuth.Middleware;

public class ApiKeyMiddleware(RequestDelegate next, IApiKeyValidation apiKeyValidation)
{
    private const string ApiKeyHeader = "X-API-KEY";

    public async Task InvokeAsync(HttpContext context)
    {
        if (!context.Request.Headers.TryGetValue(ApiKeyHeader, out var extractedApiKey))
        {
            context.Response.StatusCode = 401;
            await context.Response.WriteAsync("API Key missing");
            return;
        }

        if (!apiKeyValidation.IsValidApiKey(extractedApiKey!))
        {
            context.Response.StatusCode = 401;
            await context.Response.WriteAsync("Invalid API Key");
            return;
        }

        await next(context);
    }
}