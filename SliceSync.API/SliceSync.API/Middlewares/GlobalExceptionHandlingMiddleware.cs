using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Data.SqlClient;
using System.Threading.Tasks;

namespace SliceSync.API.Middlewares
{
    // This middleware is used to handle errors (exceptions)
    // that happen anywhere in the application.
    public class GlobalExceptionHandlingMiddleware
    {
        // _next stores the next middleware in the pipeline
        private readonly RequestDelegate _next;

        // _logger is used to save error details in logs
        private readonly ILogger<GlobalExceptionHandlingMiddleware> _logger;

        // Constructor
        // It receives the next middleware and logger object
        public GlobalExceptionHandlingMiddleware(
            RequestDelegate next,
            ILogger<GlobalExceptionHandlingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        // This method runs whenever a request comes to the application
        public async Task Invoke(HttpContext httpContext)
        {
            try
            {
                // Pass the request to the next middleware
                await _next(httpContext);
            }

            // This catch block handles InvalidOperationException
            catch (InvalidOperationException ex)
            {
                // Check if the error is related to database connection
                if (ex.InnerException?.Message.StartsWith("Cannot open database") ?? false)
                {
                    // Set HTTP status code as 500 (Internal Server Error)
                    httpContext.Response.StatusCode = 500;

                    // Send custom error message to the client
                    await httpContext.Response.WriteAsync("Invalid DB Name !!");
                }
            }

            // This catch block handles all other exceptions
            catch (Exception ex)
            {
                // Check if there is an inner exception
                if (ex.InnerException != null)
                {
                    // Save inner exception type and message in logs
                    _logger.LogError(
                        "{ExceptionType} {ExceptionMessage}",
                        ex.InnerException.GetType().ToString(),
                        ex.InnerException.Message);
                }
                else
                {
                    // Save normal exception type and message in logs
                    _logger.LogError(
                        "{ExceptionType} {ExceptionMessage}",
                        ex.GetType().ToString(),
                        ex.Message);
                }

                // Set status code as 500
                httpContext.Response.StatusCode = 500;

                // Send exception message to the client
                await httpContext.Response.WriteAsync(ex.Message);
            }
            }
    }

    // Extension class used to register middleware easily
    public static class GlobalExceptionHandlingMiddlewareExtensions
    {
        // Custom extension method
        // This helps us use:
        // app.UseGlobalExceptionHandlingMiddleware();
        public static IApplicationBuilder UseGlobalExceptionHandlingMiddleware(
            this IApplicationBuilder builder)
        {
            // Add middleware into request pipeline
            return builder.UseMiddleware<GlobalExceptionHandlingMiddleware>();
        }
    }
}