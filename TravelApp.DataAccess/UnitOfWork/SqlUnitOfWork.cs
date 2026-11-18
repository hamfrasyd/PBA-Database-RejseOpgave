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

        /// <inheritdoc/>
        public IAchievementRepository Achievements { get; }
        /// <inheritdoc/>
        public IActivitiesRepository Activities { get; }
        /// <inheritdoc/>
        public ILocationsRepository Locations { get; }
        /// <inheritdoc/>
        public ITripRepository Trips { get; }
        /// <inheritdoc/>
        public IUserRepository Users { get; }
        /// <inheritdoc/>
        public IPostRepository Posts { get; }

        /// <inheritdoc/>
        public IFriendshipRepository Friendships { get; }
        /// <inheritdoc/>
        public IInviteRepository Invitations { get; }

        /// <summary>
        /// Initializes a new instance of the SQL unit of work with the specified database connection and logger.
        /// </summary>
        /// <param name="connectionString">The connection string used to configure the SQL Server data context.</param>
        /// <param name="logger">The logger used to record unit-of-work activity and diagnostics.</param>
        public SqlUnitOfWork(string connectionString, ILogger<SqlUnitOfWork> logger)
        {
            _logger = logger;

            DbContextOptions<TravelAppContext> options = new DbContextOptionsBuilder<TravelAppContext>().UseSqlServer(connectionString).Options;
            _context = new TravelAppContext(options);

            Achievements = new SqlAchievementRepository(_context);
            Activities = new SqlActivitiesRepository(_context);
            Locations = new SqlLocationsRepository(_context);
            Trips = new SqlTripRepository(_context);
            Users = new SqlUserRepository(_context);
            Posts = new SqlPostRepository(_context);
            Friendships = new SqlFriendshipRepository(_context);
            Invitations = new SqlInviteRepository(_context);
        }

        /// <summary>Persists all pending EF Core changes as one atomic database save.</summary>
        /// <returns>A task that completes when the changes are saved.</returns>
        /// <exception cref="Exception">Rethrown when saving the changes fails.</exception>
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

        /// <summary>Releases the database context.</summary>
        /// <returns>A task that completes when the context is disposed.</returns>
        public async ValueTask DisposeAsync()
        {
            await _context.DisposeAsync();
        }
    }
}
