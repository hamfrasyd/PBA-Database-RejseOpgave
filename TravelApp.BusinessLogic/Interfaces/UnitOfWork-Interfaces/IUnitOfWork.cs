using TravelApp.BusinessLogic.Interfaces.Repository_Interfaces;

namespace TravelApp.BusinessLogic.Interfaces
{
    /// <summary>Provides repositories that share one persistence context and commit boundary.</summary>
    public interface IUnitOfWork : IAsyncDisposable
    {
        /// <summary>Provides access to the achievements repository.</summary>
        public IAchievementRepository Achievements { get; }

        /// <summary>Provides access to the destinations repository.</summary>
        public IActivitiesRepository Activities { get; }

        /// <summary>Provides access to the locations repository.</summary>
        public ILocationsRepository Locations { get; }

        /// <summary>Provides access to the Trip repository</summary>
        public ITripRepository Trips { get; }

        /// <summary>Provides access to the users repository.</summary>
        public IUserRepository Users { get; }

        /// <summary>Provides access to the posts repository.</summary>
        public IPostRepository Posts { get; }

        /// <summary>Provides access to the groups repository.</summary>
        public IGroupRepository Groups { get; }

        /// <summary>Provides access to the reviews repository.</summary>
        public IReviewRepository Reviews { get; }

        /// <summary>Saves pending changes and commits the transaction.</summary>
        /// <returns>A task that completes when the changes are saved.</returns>
        public Task CommitAsync();
    }
}
