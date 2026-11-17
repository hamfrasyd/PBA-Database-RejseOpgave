using TravelApp.BusinessLogic.Exceptions.SpecificCustomException;
using TravelApp.BusinessLogic.Interfaces;
using TravelApp.BusinessLogic.Models.Destinations;

namespace TravelApp.BusinessLogic.Handlers.Locations.CreateCity
{
    /// <summary>Handles the create city use case.</summary>
    public class CreateCityHandler
    {
        private readonly IUnitOfWorkFactory _unitOfWorkFactory;

        /// <summary>Initializes a new CreateCityHandler instance.</summary>
        public CreateCityHandler(IUnitOfWorkFactory unitOfWorkFactory)
        {
            _unitOfWorkFactory = unitOfWorkFactory;
        }

        /// <summary>Creates city.</summary>
        public async Task<int> CreateCityAsync(CreateCityRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.Name))
            {
                throw new TravelAppValidationException("City name is required.");
            }

            await using IUnitOfWork unitOfWork = _unitOfWorkFactory.Create();

            Country? countryFromDb = await unitOfWork.Locations.GetCountryByIdAsync(request.CountryId);

            if (countryFromDb is null)
            {
                throw new TravelAppValidationException("The parent destination does not exist.");
            }

            City createdCity = new City
            {
                Name = request.Name.Trim(),
                Country = countryFromDb
            };

            await unitOfWork.Locations.AddCityAsync(createdCity);

            await unitOfWork.CommitAsync();

            return createdCity.Id;
        }
    }
}
