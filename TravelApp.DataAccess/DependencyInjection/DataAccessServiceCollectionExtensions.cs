using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TravelApp.BusinessLogic.DatabaseSelection;
using TravelApp.BusinessLogic.Interfaces;
using TravelApp.DataAccess.SQLRepositories.UnitOfWork;

namespace TravelApp.DataAccess.DependencyInjection
{
    /// <summary>Registers database configuration and the provider-neutral Unit of Work factory.</summary>
    public static class DataAccessServiceCollectionExtensions
    {
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
