using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using TravelApp.BusinessLogic.Interfaces;
using TravelApp.DataAccess.SQLRepositories;
using TravelApp.DataAccess.SQLRepositories.EntityFramework;

namespace TravelApp.DataAccess.UnitOfWork
{
    /// <summary>Coordinates EF Core repositories over one DbContext and commit boundary.</summary>
    public class SqlUnitOfWork : IUnitOfWork
    {
        private readonly TravelAppContext _context;
        private readonly ILogger<SqlUnitOfWork> _logger;

        public IActivitiesRepository Activities { get; }
        public ILocationsRepository Locations { get; }
        public IAchievementRepository Achievements { get; }
        public IUserRepository Users { get; }
        public ITripRepository Trips { get; }

        public SqlUnitOfWork(string connectionString, ILogger<SqlUnitOfWork> logger)
        {
            _logger = logger;

            DbContextOptions<TravelAppContext> options = new DbContextOptionsBuilder<TravelAppContext>().UseSqlServer(connectionString).Options;
            _context = new TravelAppContext(options);

            Activities = new SqlActivitiesRepository(_context);
            Locations = new SqlLocationsRepository(_context);
            Achievements = new SqlAchievementRepository(_context);
            Users = new SqlUserRepository(_context);
            Trips = new SqlTripRepository(_context);
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
