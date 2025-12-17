using System.Text.Json;
using Microsoft.AspNetCore.Diagnostics;
using NotifyMe.Api.Configurations;

namespace NotifyMe.Api.Middlewares;

public class ApiExceptionHandler(ILogger<ApiExceptionHandler> logger) : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext,
        Exception exception,
        CancellationToken cancellationToken)
    {
        logger.LogError(exception, "Unhandled exception");

        var pd = ApiProblemDetails.FromException(exception);

        httpContext.Response.StatusCode = pd.Status ?? StatusCodes.Status500InternalServerError;
        httpContext.Response.ContentType = "application/problem+json";

        // Optional: add traceId without “extensions complexity”
        pd.Instance = httpContext.TraceIdentifier;

        await httpContext.Response.WriteAsync(
            JsonSerializer.Serialize(pd),
            cancellationToken);

        return true;
    }
}