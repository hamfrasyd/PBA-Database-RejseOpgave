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

            City? entityFromDb = await unitOfWork.Destinations.GetCityByIdAsync(request.CityId);

            if (entityFromDb is null)
            {
                throw new TravelAppNotFoundException($"City {request.CityId} was not found.");
            }

            if (string.IsNullOrWhiteSpace(request.Name))
            {
                throw new TravelAppValidationException("City name is required.");
            }

            Country? countryFromDb = await unitOfWork.Destinations.GetCountryByIdAsync(request.CountryId);

            if (countryFromDb is null)
            {
                throw new TravelAppValidationException("The parent destination does not exist.");
            }

            entityFromDb.Name = request.Name.Trim();
            entityFromDb.Country = countryFromDb;

            await unitOfWork.Destinations.UpdateCityAsync(entityFromDb);

            await unitOfWork.CommitAsync();
        }
    }
}
