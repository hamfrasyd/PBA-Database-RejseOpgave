using TravelApp.BusinessLogic.DatabaseSelection;

namespace TravelApp.API.DatabaseSelection
{
    /// <summary>Selects a provider per HTTP request, ready for a future frontend database selector.</summary>
    public class DatabaseProviderMiddleware
    {
        private string ProviderHeaderName = HeaderName_For_DatabaseProvider.Key;
        private readonly RequestDelegate _next;
        private readonly IConfiguration _configuration;

        /// <summary>Initializes a new instance of the <see cref="DatabaseProviderMiddleware"/> class.</summary>
        /// <param name="next">The next middleware in the pipeline.</param>
        /// <param name="configuration">The application configuration used when the request does not select a provider.</param>
        public DatabaseProviderMiddleware(RequestDelegate next, IConfiguration configuration)
        {
            _next = next;
            _configuration = configuration;
        }

        /// <summary>Selects the database provider for the current HTTP request.</summary>
        /// <param name="context">The current HTTP context.</param>
        /// <param name="selectedDatabaseProvider">The provider selection stored for this request.</param>
        /// <returns>A task that completes when the request has been handled.</returns>
        public async Task InvokeAsync(HttpContext context, SelectedDatabaseProvider selectedDatabaseProvider)
        {
            string selectedProvider = context.Request.Headers[ProviderHeaderName].ToString();
            if (string.IsNullOrWhiteSpace(selectedProvider))
            {
                selectedProvider = _configuration["Database:Provider"] ?? "Sql"; // Default to SQL if not specified in the Headers value or configuration
            }

            switch (selectedProvider.Trim().ToLowerInvariant())
            {
                case "sql":
                case "mssql":
                    selectedDatabaseProvider.Provider = DatabaseProviderTypes.Sql;
                    break;

                case "mongo":
                case "mongodb":
                    selectedDatabaseProvider.Provider = DatabaseProviderTypes.Mongo;
                    break;

                case "graph":
                case "neo4j":
                    selectedDatabaseProvider.Provider = DatabaseProviderTypes.Graph;
                    break;

                default:
                    context.Response.StatusCode = StatusCodes.Status400BadRequest;
                    await context.Response.WriteAsJsonAsync(new { Error = $"Unknown database provider '{selectedProvider}'." });
                    return;
            }

            if (selectedDatabaseProvider.Provider != DatabaseProviderTypes.Sql)
            {
                context.Response.StatusCode = StatusCodes.Status501NotImplemented;
                await context.Response.WriteAsJsonAsync(new { Error = $"Provider '{selectedDatabaseProvider.Provider}' is not implemented. SQL is currently available." });
                return;
            }

            await _next(context);
        }
    }
}
