using Domain.Exceptions;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace UsersAPI
{
    public class GlobalExceptionHandler : IExceptionHandler
    {
        ILogger<GlobalExceptionHandler> logger;

        public GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger)
        {
            this.logger = logger;
        }

        public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
        {
            logger.LogError(exception, "An exception occurred: {Message}", exception.Message);

            var (statusCode, title) = exception switch
            {
                KeyNotFoundException => ((int)HttpStatusCode.NotFound, "Resource not found"),
                ArgumentException => ((int)HttpStatusCode.BadRequest, "Invalid request"),
                InvalidOperationException => ((int)HttpStatusCode.BadRequest, "Invalid operation"),
                UserNotFoundException => ((int)HttpStatusCode.NotFound,"User was not found"),
                _ => ((int)HttpStatusCode.InternalServerError, "An unexpected error occurred")
            };

            await httpContext.Response.WriteAsJsonAsync(new ProblemDetails
            {
                Status = statusCode,
                Type = "https://tools.ietf.org/html/rfc7231#section-6.5.1",
                Title = title,
                Detail = exception.Message,
                Instance = httpContext.Request.Path
            }, cancellationToken);

            return true;
        }
    }
}
