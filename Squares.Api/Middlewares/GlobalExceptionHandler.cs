using Microsoft.AspNetCore.Diagnostics;

namespace Squares.Api.Middlewares;

internal sealed class GlobalExceptionHandler(IProblemDetailsService problemDetailsService ,ILogger<GlobalExceptionHandler> logger) 
    : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken ct)
    {
		logger.LogError(exception, "An unhandled exception occurred while processing the request.");
		httpContext.Response.StatusCode = StatusCodes.Status500InternalServerError;
        await problemDetailsService.WriteAsync(new ProblemDetailsContext
        {
            HttpContext = httpContext,
            Exception = exception,
            ProblemDetails =
            {
                Title = "An error occurred",
                Detail = exception.Message,
                Type = exception.GetType().Name
            }
        });

        return true;
    }
}
