namespace TravelApp.BusinessLogic.Interfaces
{
    /// <summary>Provides repositories that share one persistence context and commit boundary.</summary>
    public interface IUnitOfWork : IAsyncDisposable
    {
        public IDestinationRepository Destinations { get; }

        public Task CommitAsync();
    }
}
