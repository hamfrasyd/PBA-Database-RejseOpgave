using TravelApp.BusinessLogic.Models;
using TravelApp.BusinessLogic.Exceptions.SpecificCustomException;
using TravelApp.BusinessLogic.Interfaces;

namespace TravelApp.BusinessLogic.Handlers.Destinations
{
    /// <summary>Handles the update city use case.</summary>
    public class UpdateCityHandler
    {
        private readonly IUnitOfWorkFactory _unitOfWorkFactory;

        /// <summary>Initializes a new UpdateCityHandler instance.</summary>
        public UpdateCityHandler(IUnitOfWorkFactory unitOfWorkFactory)
        {
            _unitOfWorkFactory = unitOfWorkFactory;
        }

        /// <summary>Updates city.</summary>
        public async Task UpdateCityAsync(UpdateCityRequest request)
        {
            await using IUnitOfWork unitOfWork = _unitOfWorkFactory.Create();

            if (string.IsNullOrWhiteSpace(request.Name))
            {
                throw new TravelAppValidationException("City name is required.");
            }

            if (request.Country is null)
            {
                throw new TravelAppValidationException("Country is required.");
            }

            bool dbOperationSuccessful = await unitOfWork.Destinations.UpdateCityAsync(request);
            if (dbOperationSuccessful is false)
            {
                throw new TravelAppNotFoundException($"City {request.CityId} was not found.");
            }

            await unitOfWork.CommitAsync();
        }
    }
}
