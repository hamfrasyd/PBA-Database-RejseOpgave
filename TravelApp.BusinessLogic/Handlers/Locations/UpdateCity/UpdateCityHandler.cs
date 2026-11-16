using TravelApp.BusinessLogic.Exceptions.SpecificCustomException;
using TravelApp.BusinessLogic.Interfaces;

namespace TravelApp.BusinessLogic.Handlers.Locations.UpdateCity
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
        public async Task UpdateNameAndCountryAsync(int cityId, UpdateCityRequest request)
        {
            await using IUnitOfWork unitOfWork = _unitOfWorkFactory.Create();

            if (string.IsNullOrWhiteSpace(request.Name))
            {
                throw new TravelAppValidationException("City name is required.");
            }

            City? cityInDb = await unitOfWork.Destinations.GetCityByIdAsync(request.CityId);
            if (cityInDb is null)
            {
                throw new TravelAppNotFoundException($"City {request.CityId} was not found.");
            }

            bool operationSuccessful = await unitOfWork.Locations.UpdateCityAsync(cityId, request);
            if (operationSuccessful is false)
            {
                throw new TravelAppNotFoundException($"Country {request.CountryId} was not found.");
                throw new TravelAppNotFoundException($"City with ID: {cityId} was not found.");
            }

            cityInDb.Country = countryInDb;
            cityInDb.Name = request.Name.Trim();

            await unitOfWork.Destinations.UpdateCityAsync(cityInDb, request);

            await unitOfWork.CommitAsync();
        }
    }
}
