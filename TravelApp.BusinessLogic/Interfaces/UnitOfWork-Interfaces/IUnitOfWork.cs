namespace TravelApp.BusinessLogic.Interfaces
{
    /// <summary>Provides repositories that share one persistence context and commit boundary.</summary>
    public interface IUnitOfWork : IAsyncDisposable
    {
        /// <summary>Provides access to the destinations repository.</summary>
        public IDestinationRepository Destinations { get; }

        /// <summary>Provides access to the achievements repository.</summary>
        public IAchievementRepository Achievements { get; }

        /// <summary>Saves pending changes and commits the transaction.</summary>
        public Task CommitAsync();
    }
}
