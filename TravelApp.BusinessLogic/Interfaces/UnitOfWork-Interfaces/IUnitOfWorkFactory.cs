namespace TravelApp.BusinessLogic.Interfaces
{
    /// <summary>Creates iunit of work instances.</summary>
    public interface IUnitOfWorkFactory
    {
        /// <summary>Creates a Unit of Work for the selected database provider.</summary>
        public IUnitOfWork Create();
    }
}
