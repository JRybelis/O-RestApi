using System.Net;
using HotelListing.Net9.API.Core.Exceptions;
using HotelListing.Net9.API.Core.Models.Middleware;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace HotelListing.Net9.API.Core.Middleware;

public class ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
{
    /// <summary>
    /// The nested try-catch provides graceful degradation:
    /// Level 1: Try to send detailed JSON error
    /// Level 2: If JSON serialization fails, send simple text error from HandleExceptionAsync
    /// Level 3: If everything fails, at least the 500 status code is set
    /// </summary>
    /// <param name="httpContext">http request context</param>
    public async Task InvokeAsync(HttpContext httpContext)
    {
        try
        {
            await next(httpContext);
        }
        catch (Exception e)
        {
            logger.LogError(e, "Something went wrong while processing the {0}", httpContext.Request.Path);
            await HandleExceptionAsync(httpContext, e);
        }
    }

    private async Task HandleExceptionAsync(HttpContext httpContext, Exception exception)
    {
        // Prevent multiple responses
        if (httpContext.Response.HasStarted) return; // prevents the error handler from crashing

        try
        {
            httpContext.Response.Clear(); // Clears partial JSON responses
            httpContext.Response.ContentType = "application/json";
            
            var statusCode = HttpStatusCode.InternalServerError;

            var errorDetails = new ErrorDetails
            {
                ErrorType = "Failure",
                ErrorMessage = exception.Message
            };
            
            switch (exception)
            {
                case NotFoundException:
                    statusCode = HttpStatusCode.NotFound;
                    errorDetails.ErrorType = "NotFound";
                    break;
            }
            
            var response = JsonConvert.SerializeObject(errorDetails);
            httpContext.Response.StatusCode = (int)statusCode;
            
            await httpContext.Response.WriteAsync(response);
        }
        catch (Exception writeException)
        {
            logger.LogError(writeException, "Failed to write detailed error response.");
            // At least, try to set a minimal error response
            try
            {
                httpContext.Response.StatusCode = 500;
                await httpContext.Response.WriteAsync("Internal Server Error");
            }
            catch(Exception fallbackException)
            {
                // Suppress, nothing else can be done at this point.
                // Last resort log the fact that even the fallback failed
                logger.LogCritical(fallbackException, "Exception middleware failed completely: {0} for {1}",
                    fallbackException.Message);
            }
        }
    }
}