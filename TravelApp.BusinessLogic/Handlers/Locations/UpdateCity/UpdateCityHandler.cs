using TravelApp.BusinessLogic.Exceptions.SpecificCustomException;
using TravelApp.BusinessLogic.Interfaces;
using TravelApp.BusinessLogic.Models.Destinations;

namespace TravelApp.BusinessLogic.Handlers.Locations.UpdateCity
{
    /// <summary>Handles the update city use case.</summary>
    public class UpdateCityHandler
    {
        private readonly IUnitOfWorkFactory _unitOfWorkFactory;

        /// <summary>Initializes a new UpdateCityHandler instance.</summary>
        /// <param name="unitOfWorkFactory">The unit of work factory.</param>
        public UpdateCityHandler(IUnitOfWorkFactory unitOfWorkFactory)
        {
            _unitOfWorkFactory = unitOfWorkFactory;
        }

        /// <summary>Updates city.</summary>
        /// <param name="cityId">The city identifier.</param>
        /// <param name="request">The updated city details.</param>
        /// <exception cref="TravelAppValidationException">Thrown when the city name is missing.</exception>
        /// <exception cref="TravelAppNotFoundException">Thrown when the city or country does not exist.</exception>
        public async Task UpdateNameAndCountryAsync(int cityId, UpdateCityRequest request)
        {
            await using IUnitOfWork unitOfWork = _unitOfWorkFactory.Create();

            if (string.IsNullOrWhiteSpace(request.Name))
            {
                throw new TravelAppValidationException("City name is required.");
            }

            City? cityInDb = await unitOfWork.Locations.GetCityByIdAsync(cityId);
            if (cityInDb is null)
            {
                throw new TravelAppNotFoundException($"City {cityId} was not found.");
            }

            bool operationSuccessful = await unitOfWork.Locations.UpdateCityAsync(cityId, request);
            if (operationSuccessful is false)
            {
                throw new TravelAppNotFoundException($"Country {request.CountryId} was not found.");
                throw new TravelAppNotFoundException($"City with ID: {cityId} was not found.");
            }

            cityInDb.Country = cityInDb.Country;
            cityInDb.Name = request.Name.Trim();

            await unitOfWork.Locations.UpdateCityAsync(cityId, request);

            await unitOfWork.CommitAsync();
        }
    }
}
