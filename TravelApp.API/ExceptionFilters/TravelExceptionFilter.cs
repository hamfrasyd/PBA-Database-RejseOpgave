using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;
using TravelApp.BusinessLogic.Exceptions.SpecificCustomException;

namespace TravelApp.BusinessLogic.Exceptions.Filters
{
    /// <summary>
    /// Centralized exception filter that converts exceptions thrown during
    /// ASP.NET Core controller execution into structured HTTP error responses.
    /// Maps exceptions to appropriate HTTP status codes and logs database
    /// failures and unexpected errors.
    /// </summary>
    public class TravelExceptionFilter : IExceptionFilter
    {
        private readonly ILogger<TravelExceptionFilter> _logger;

        public TravelExceptionFilter(ILogger<TravelExceptionFilter> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// Handles an exception by identifying its type and mapping it to an
        /// appropriate HTTP status code and error message.
        /// Creates a structured JSON error response and marks the exception as handled.
        /// Logs database failures and unexpected exceptions.
        /// </summary>
        /// <param name="context">
        /// The exception context provided by ASP.NET Core, containing the thrown
        /// exception and allowing the HTTP response to be configured.
        /// </param>
        /// <remarks>
        /// Does not return a value directly. Instead, updates
        /// <see cref="ExceptionContext.Result"/> with an HTTP error response
        /// and sets <see cref="ExceptionContext.ExceptionHandled"/> to true.
        /// </remarks>
        public void OnException(ExceptionContext context)
        {
            Exception exception = context.Exception;
            int statusCode;
            string errorMessage;

            switch (exception)
            {
                // Catch Custom TravelApp exceptions and map them to appropriate HTTP status codes
                case TravelAppValidationException validationException:
                    statusCode = StatusCodes.Status400BadRequest;
                    errorMessage = validationException.Message;
                    break;

                case TravelAppNotFoundException notFoundException:
                    statusCode = StatusCodes.Status404NotFound;
                    errorMessage = notFoundException.Message;
                    break;

                case TravelAppConflictException conflictException:
                    statusCode = StatusCodes.Status409Conflict;
                    errorMessage = conflictException.Message;
                    break;

                case TravelAppForbiddenException forbiddenException:
                    statusCode = StatusCodes.Status403Forbidden;
                    errorMessage = forbiddenException.Message;
                    break;

                // Catch AspNetCore built-in exceptions and map them to appropriate HTTP status codes
                case UnauthorizedAccessException:
                    statusCode = StatusCodes.Status401Unauthorized;
                    errorMessage = "Authentication is required.";
                    break;

                // Catch Entity Framework exceptions and map them to appropriate HTTP status codes
                case DbUpdateConcurrencyException concurrencyException:
                    _logger.LogWarning(concurrencyException, "Concurrency conflict while updating the database.");
                    statusCode = StatusCodes.Status409Conflict;
                    errorMessage = "The data was modified or deleted by another operation.";
                    break;

                case DbUpdateException updateException:
                    _logger.LogError( updateException, "Entity Framework Core failed to save database changes.");
                    statusCode = StatusCodes.Status500InternalServerError;
                    errorMessage = "An error occurred while saving database changes.";
                    break;

                // Unexpected exceptions
                default:
                    _logger.LogError(exception, "Unexpected error while processing a TravelApp request.");
                    statusCode = StatusCodes.Status500InternalServerError;
                    errorMessage = "An unexpected server error occurred.";
                    break;
            }

            // ErrorResponse is our custom class to display error msgs, in json format.
            // Could also send to ObjetResult the errorMessage string, but it's better to have a structured response.
            ErrorResponse errorResponse = new ErrorResponse
            {
                Error = errorMessage
            };

            // ObjectResult is an ASP.NET Core result type that allows you to return an object as the HTTP response body.
            ObjectResult result = new ObjectResult(errorResponse)
            {
                StatusCode = statusCode
            };

            // context.Result allows you to specify what HTTP response ASP.NET Core should return to the client.
            context.Result = result;

            // Tells ASP.NET Core that the exception has been handled, so the exception filter has finished processing it.
            context.ExceptionHandled = true;
        }
    }
}
