using TravelApp.BusinessLogic.Exceptions.SpecificCustomException;
using TravelApp.BusinessLogic.Handlers.Locations.Data;
using TravelApp.BusinessLogic.Interfaces;

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

        /// <summary>Updates a city's name and country and returns the persisted result.</summary>
        /// <param name="cityId">The city identifier.</param>
        /// <param name="request">The updated city details.</param>
        /// <returns>The updated city.</returns>
        /// <exception cref="TravelAppValidationException">Thrown when the city name is missing or longer than 64 characters.</exception>
        /// <exception cref="TravelAppNotFoundException">Thrown when the city or country does not exist.</exception>
        public async Task<UpdateCityResponse> UpdateCityAsync(int cityId, UpdateCityRequest request)
        {
            string cityName = request.Name.Trim();

            bool cityNameIsMissing = string.IsNullOrWhiteSpace(cityName);
            if (cityNameIsMissing)
            {
                throw new TravelAppValidationException("City name is required.");
            }

            bool cityNameIsTooLong = cityName.Length > 64;
            if (cityNameIsTooLong)
            {
                throw new TravelAppValidationException("City name must be 64 characters or fewer.");
            }

            await using IUnitOfWork unitOfWork = _unitOfWorkFactory.Create();

            CityDto? existingCityData = await unitOfWork.Locations.GetCityAsync(cityId);
            bool cityNotFound = existingCityData is null;
            if (cityNotFound)
            {
                throw new TravelAppNotFoundException($"City {cityId} was not found.");
            }

            bool countryExists = await unitOfWork.Locations.CountryExistsAsync(request.CountryId);
            if (countryExists is false)
            {
                throw new TravelAppNotFoundException($"Country {request.CountryId} was not found.");
            }

            UpdateCityData cityToUpdate = new UpdateCityData
            {
                Id = cityId,
                Name = cityName,
                CountryId = request.CountryId
            };

            UpdateCityData? updatedCityData = await unitOfWork.Locations.UpdateCityAsync(cityToUpdate);
            if (updatedCityData is null)
            {
                throw new TravelAppNotFoundException($"City {cityId} was not found.");
            }

            await unitOfWork.CommitAsync();

            UpdateCityResponse response = new UpdateCityResponse
            {
                Id = updatedCityData.Id,
                Name = updatedCityData.Name,
                CountryId = updatedCityData.CountryId
            };

            return response;
        }
    }
}
