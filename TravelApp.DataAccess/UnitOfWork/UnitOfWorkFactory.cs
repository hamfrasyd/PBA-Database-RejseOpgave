using Microsoft.Extensions.Logging;
using TravelApp.BusinessLogic.DatabaseSelection;
using TravelApp.BusinessLogic.Interfaces;

namespace TravelApp.DataAccess.SQLRepositories.UnitOfWork
{

    /// <summary>Creates the selected provider's Unit of Work without exposing SQL types to business logic.</summary>
    public class UnitOfWorkFactory : IUnitOfWorkFactory
    {
        private readonly DatabaseConnectionStrings _connectionStrings;
        private readonly SelectedDatabaseProvider _providerSelection;
        private readonly ILoggerFactory _loggerFactory;

        public UnitOfWorkFactory(DatabaseConnectionStrings connectionStrings, SelectedDatabaseProvider providerSelection, ILoggerFactory loggerFactory)
        {
            _connectionStrings = connectionStrings;
            _providerSelection = providerSelection;
            _loggerFactory = loggerFactory;
        }

        public IUnitOfWork Create()
        {
            switch (_providerSelection.Provider)
            {
                case DatabaseProviderTypes.Sql:
                    return new SqlUnitOfWork(_connectionStrings.Sql, _loggerFactory.CreateLogger<SqlUnitOfWork>());
                    throw new NotSupportedException($"Database provider '{_providerSelection.Provider}' is not implemented yet.");

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
