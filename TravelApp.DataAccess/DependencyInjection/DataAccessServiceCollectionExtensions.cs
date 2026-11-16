using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TravelApp.BusinessLogic.DatabaseSelection;
using TravelApp.BusinessLogic.Interfaces;
using TravelApp.DataAccess.UnitOfWork;

namespace TravelApp.DataAccess.DependencyInjection
{
    /// <summary>Registers database configuration and the provider-neutral Unit of Work factory.</summary>
    public static class DataAccessServiceCollectionExtensions
    {
        /// <summary>Registers data access services.</summary>
        /// <param name="services">The service collection to register data access in.</param>
        /// <param name="configuration">The application configuration that contains the connection strings.</param>
        /// <returns>The same service collection, so registration can be chained.</returns>
        /// <exception cref="InvalidOperationException">Thrown when the SQL connection string is missing or empty.</exception>
        public static IServiceCollection AddDataAccess(this IServiceCollection services, IConfiguration configuration)
        {
            // Register Services
            RegisterDatabaseSelection(services, configuration);
            RegisterUnitOfWorkFactory(services);
            return services;
        }
        private static void RegisterDatabaseSelection(IServiceCollection services, IConfiguration configuration)
        {
            // Get ConnectionString from configuration
            string sqlConnection = configuration["ConnectionStrings:TravelDb"] ?? throw new InvalidOperationException("ConnectionStrings:TravelDb is required.");

            if (string.IsNullOrWhiteSpace(sqlConnection))
            {
                throw new InvalidOperationException("ConnectionStrings:TravelDb cannot be empty.");
            }

            services.AddSingleton(new DatabaseConnectionStrings
            {
                Sql = sqlConnection,
                Mongo = configuration["ConnectionStrings:MongoDb"], // Not implemented yet
                Neo4j = configuration["ConnectionStrings:Neo4j"] // Not implemented yet
            });
        }

        private static void RegisterUnitOfWorkFactory(IServiceCollection services)
        {
            services.AddScoped<IUnitOfWorkFactory, UnitOfWorkFactory>();
        }
    }
}
