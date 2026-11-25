
using DevFlowAPI.Exceptions.Auth;
using Microsoft.AspNetCore.Mvc;

namespace DevFlowAPI.Middleware;

internal sealed class GlobalExceptionHandlerMiddleware(
    RequestDelegate next,
    ILogger<GlobalExceptionHandlerMiddleware> logger)
{
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await next(context);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Unhandled exception occurred");

            var statusCode = ex switch
            {
                InvalidCredentialsException =>
                    StatusCodes.Status401Unauthorized,

                EmailAlreadyRegisteredException =>
                    StatusCodes.Status409Conflict,

                _ => StatusCodes.Status500InternalServerError
            };

            context.Response.StatusCode = statusCode;

            await context.Response.WriteAsJsonAsync(
                new ProblemDetails
                {
                    Status = statusCode,
                    Title = statusCode switch
                    {
                        401 => "Unauthorized",
                        409 => "Conflict",
                        _ => "Internal Server Error"
                    },
                    Detail = statusCode == 500
                        ? "An unexpected error occurred."
                        : ex.Message
                });
        }
    }
}
