using TravelApp.BusinessLogic.Models;
using TravelApp.BusinessLogic.Exceptions.SpecificCustomException;
using TravelApp.BusinessLogic.Interfaces;

namespace TravelApp.BusinessLogic.Handlers.Destinations
{
    /// <summary>Handles the delete city use case.</summary>
    public class DeleteCityHandler
    {
        private readonly IUnitOfWorkFactory _unitOfWorkFactory;

        /// <summary>Initializes a new DeleteCityHandler instance.</summary>
        public DeleteCityHandler(IUnitOfWorkFactory unitOfWorkFactory)
        {
            _unitOfWorkFactory = unitOfWorkFactory;
        }

        /// <summary>Deletes city.</summary>
        public async Task DeleteCityAsync(int resourceId)
        {
            await using IUnitOfWork unitOfWork = _unitOfWorkFactory.Create();

            bool wasDeleted = await unitOfWork.Destinations.DeleteCityAsync(resourceId);

            if (wasDeleted is false)
            {
                throw new TravelAppNotFoundException($"City {resourceId} was not found.");
            }

            await unitOfWork.CommitAsync();
        }
    }
}
