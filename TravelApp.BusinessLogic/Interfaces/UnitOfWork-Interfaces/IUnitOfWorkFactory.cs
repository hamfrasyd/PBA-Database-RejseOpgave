namespace TravelApp.BusinessLogic.Interfaces
{
    /// <summary>Creates iunit of work instances.</summary>
    public interface IUnitOfWorkFactory
    {
        /// <summary>Creates a Unit of Work for the selected database provider.</summary>
        /// <returns>A unit of work for the selected database provider.</returns>
        public IUnitOfWork Create();
    }
}
