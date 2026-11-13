using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using TravelApp.BusinessLogic.Interfaces;
using TravelApp.BusinessLogic.Models;
using TravelApp.DataAccess.SQLRepositories;
using TravelApp.DataAccess.SQLRepositories.EntityFramework;

namespace TravelApp.DataAccess.SQLRepositories.UnitOfWork
{
    /// <summary>Coordinates EF Core repositories over one DbContext and commit boundary.</summary>
    public class SqlUnitOfWork : IUnitOfWork
    {
        private readonly TravelAppContext _context;
        private readonly ILogger<SqlUnitOfWork> _logger;

        public IDestinationRepository Destinations { get; }
        public IAchievementRepository Achievements { get; }
        public IUserRepository Users { get; }

        public SqlUnitOfWork(string connectionString, ILogger<SqlUnitOfWork> logger)
        {
            _logger = logger;

            DbContextOptions<TravelAppContext> options = new DbContextOptionsBuilder<TravelAppContext>().UseSqlServer(connectionString).Options;
            _context = new TravelAppContext(options);

            Destinations = new SqlDestinationRepository(_context);
            Achievements = new SqlAchievementRepository(_context);
            Users = new SqlUserRepository(_context);
        }

        /// <summary>Persists all pending EF Core changes as one atomic database save.</summary>
        public async Task CommitAsync()
        {
            try
            {
                int changedEntities = await _context.SaveChangesAsync();

                if (_context.Database.CurrentTransaction is not null)
                {
                    await _context.Database.CommitTransactionAsync();
                }

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
