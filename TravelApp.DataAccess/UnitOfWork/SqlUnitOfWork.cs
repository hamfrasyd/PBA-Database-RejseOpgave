using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using TravelApp.BusinessLogic.Interfaces;
using TravelApp.DataAccess.SQLRepositories;
using TravelApp.DataAccess.SQLRepositories.EntityFramework;

namespace TravelApp.DataAccess.SQLRepositories.UnitOfWork
{
    /// <summary>Coordinates EF Core repositories over one DbContext and commit boundary.</summary>
    internal class SqlUnitOfWork : IUnitOfWork
    {
        private readonly TravelAppContext _context;
        private readonly ILogger<SqlUnitOfWork> _logger;

        public IDestinationRepository Destinations { get; }

        public SqlUnitOfWork(string connectionString, ILogger<SqlUnitOfWork> logger)
        {
            _logger = logger;

            DbContextOptions<TravelAppContext> options = new DbContextOptionsBuilder<TravelAppContext>().UseSqlServer(connectionString).Options;
            _context = new TravelAppContext(options);
            Destinations = new SqlDestinationRepository(_context);
        }

        /// <summary>Persists all pending EF Core changes as one atomic database save.</summary>
        public async Task CommitAsync()
        {
            try
            {
                int changedEntities = await _context.SaveChangesAsync();
                _logger.LogDebug("Committed {ChangedEntities} TravelDB entities.", changedEntities);
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, "Failed to commit TravelDB changes.");
                throw;
            }
        }

        public async ValueTask DisposeAsync()
        {
            await _context.DisposeAsync();
        }
    }
}
