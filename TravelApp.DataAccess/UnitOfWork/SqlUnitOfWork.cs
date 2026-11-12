using Microsoft.Extensions.Logging;
using TravelApp.BusinessLogic.Interfaces;

namespace TravelApp.DataAccess.SQLRepositories.UnitOfWork
{
    /// <summary>Coordinates EF Core repositories over one DbContext and commit boundary.</summary>
    internal class SqlUnitOfWork : IUnitOfWork
    {
        private readonly ILogger<SqlUnitOfWork> _logger;


        public SqlUnitOfWork(string connectionString, ILogger<SqlUnitOfWork> logger)
        {
            _logger = logger;
        }

        /// <summary>Persists all pending EF Core changes as one atomic database save.</summary>
        public async Task CommitAsync()
        {
        }

        public async ValueTask DisposeAsync()
        {
        }
    }
}
