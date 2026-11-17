using TravelApp.BusinessLogic.Exceptions.SpecificCustomException;
using TravelApp.BusinessLogic.Interfaces;

namespace TravelApp.BusinessLogic.Handlers.Locations.DeleteCity
{
    /// <summary>Handles the delete city use case.</summary>
    public class DeleteCityHandler
    {
        private readonly IUnitOfWorkFactory _unitOfWorkFactory;

        /// <summary>Initializes a new DeleteCityHandler instance.</summary>
        /// <param name="unitOfWorkFactory">The unit of work factory.</param>
        public DeleteCityHandler(IUnitOfWorkFactory unitOfWorkFactory)
        {
            _unitOfWorkFactory = unitOfWorkFactory;
        }

        /// <summary>Deletes a city and returns its identifier.</summary>
        /// <param name="cityId">The city identifier.</param>
        /// <returns>The identifier of the deleted city.</returns>
        /// <exception cref="TravelAppNotFoundException">Thrown when the city does not exist.</exception>
        public async Task<int> DeleteCityAsync(int cityId)
        {
            await using IUnitOfWork unitOfWork = _unitOfWorkFactory.Create();

            bool wasDeleted = await unitOfWork.Locations.DeleteCityAsync(cityId);
            if (wasDeleted is false)
            {
                throw new TravelAppNotFoundException($"City {cityId} was not found.");
            }

            await unitOfWork.CommitAsync();

            return cityId;
        }
    }
}
