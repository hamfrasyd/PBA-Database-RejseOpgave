using Microsoft.Extensions.Logging;
using TravelApp.BusinessLogic.DatabaseSelection;
using TravelApp.BusinessLogic.Interfaces;

namespace TravelApp.DataAccess.UnitOfWork
{

    /// <summary>Creates the selected provider's Unit of Work without exposing SQL types to business logic.</summary>
    public class UnitOfWorkFactory : IUnitOfWorkFactory
    {
        private readonly DatabaseConnectionStrings _connectionStrings;
        private readonly SelectedDatabaseProvider _providerSelection;
        private readonly ILoggerFactory _loggerFactory;

        /// <summary>Initializes a new instance of the <see cref="UnitOfWorkFactory"/> class.</summary>
        /// <param name="connectionStrings">The configured database connection strings.</param>
        /// <param name="providerSelection">The selected database provider.</param>
        /// <param name="loggerFactory">The logger factory used by the created unit of work.</param>
        public UnitOfWorkFactory(
            DatabaseConnectionStrings connectionStrings,
            SelectedDatabaseProvider providerSelection,
            ILoggerFactory loggerFactory)
        {
            _connectionStrings = connectionStrings;
            _providerSelection = providerSelection;
            _loggerFactory = loggerFactory;
        }

        /// <summary>Creates a unit of work for the selected database provider.</summary>
        /// <returns>A unit of work for the selected database provider.</returns>
        /// <exception cref="NotSupportedException">Thrown when the selected provider is not implemented.</exception>
        /// <exception cref="InvalidOperationException">Thrown when the selected provider is unknown.</exception>
        public IUnitOfWork Create()
        {
            switch (_providerSelection.Provider)
            {
                case DatabaseProviderTypes.Sql:
                    return new SqlUnitOfWork(_connectionStrings.Sql, _loggerFactory.CreateLogger<SqlUnitOfWork>());

                case DatabaseProviderTypes.Mongo:
                    throw new NotSupportedException($"Database provider '{_providerSelection.Provider}' is not implemented yet.");

                case DatabaseProviderTypes.Graph:
                    throw new NotSupportedException($"Database provider '{_providerSelection.Provider}' is not implemented yet.");

                default:
                    throw new InvalidOperationException("Unknown database provider.");
            }
        }
    }
}
