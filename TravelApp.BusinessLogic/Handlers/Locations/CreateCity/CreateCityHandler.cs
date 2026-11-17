using TravelApp.BusinessLogic.Exceptions.SpecificCustomException;
using TravelApp.BusinessLogic.Handlers.Locations.Data;
using TravelApp.BusinessLogic.Interfaces;

namespace TravelApp.BusinessLogic.Handlers.Locations.CreateCity
{
    /// <summary>Handles the create city use case.</summary>
    public class CreateCityHandler
    {
        private readonly IUnitOfWorkFactory _unitOfWorkFactory;

        /// <summary>Initializes a new CreateCityHandler instance.</summary>
        /// <param name="unitOfWorkFactory">The unit of work factory.</param>
        public CreateCityHandler(IUnitOfWorkFactory unitOfWorkFactory)
        {
            _unitOfWorkFactory = unitOfWorkFactory;
        }

        /// <summary>Creates a city and returns the persisted result.</summary>
        /// <param name="request">The city to create.</param>
        /// <returns>The created city.</returns>
        /// <exception cref="TravelAppValidationException">Thrown when the name is missing or longer than 64 characters.</exception>
        /// <exception cref="TravelAppNotFoundException">Thrown when the country does not exist.</exception>
        public async Task<CreateCityResponse> CreateCityAsync(CreateCityRequest request)
        {
            await using IUnitOfWork unitOfWork = _unitOfWorkFactory.Create();

            bool countryExists = await unitOfWork.Locations.CountryExistsAsync(request.CountryId);
            if (countryExists is false)
            {
                throw new TravelAppNotFoundException($"Country {request.CountryId} was not found.");
            }

            CreateCityDto cityToCreate = new CreateCityDto
            {
                Name = request.Name.Trim(),
                CountryId = request.CountryId
            };

            CityDto? createdCity = await unitOfWork.Locations.CreateCityAsync(cityToCreate);

            if (createdCity is null)
            {
                createdCity = unitOfWork.Locations.GetCreatedCity();
            }

            await unitOfWork.CommitAsync();

            CreateCityResponse response = new CreateCityResponse
            {
                Id = createdCity.Id,
                Name = createdCity.Name,
                CountryId = createdCity.CountryId
            };
            return response;
        }
    }
}
