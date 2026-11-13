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

            City? cityInDb = await unitOfWork.Destinations.GetCityByIdAsync(request.CityId);
            if (cityInDb is null)
            {
                throw new TravelAppNotFoundException($"City {request.CityId} was not found.");
            }

            Country? countryInDb = await unitOfWork.Destinations.GetCountryByIdAsync(request.CountryId);
            if (countryInDb is null)
            {
                throw new TravelAppNotFoundException($"Country {request.CountryId} was not found.");
            }

            cityInDb.Country = countryInDb;
            cityInDb.Name = request.Name.Trim();

            await unitOfWork.Destinations.UpdateCityAsync(cityInDb, request);

            await unitOfWork.CommitAsync();
        }
    }
}
