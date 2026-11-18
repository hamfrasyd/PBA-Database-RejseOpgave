using TravelApp.BusinessLogic.Exceptions.SpecificCustomException;
using TravelApp.BusinessLogic.Interfaces;
using TravelApp.BusinessLogic.Models.Locations;

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
        /// <exception cref="TravelAppNotFoundException">Thrown when the country does not exist.</exception>
        public async Task<CreateCityResponse> CreateCityAsync(CreateCityRequest request)
        {
            await using IUnitOfWork unitOfWork = _unitOfWorkFactory.Create();

            bool countryExists = await unitOfWork.Locations.CountryExistsAsync(request.CountryId);
            if (countryExists is false)
            {
                throw new TravelAppNotFoundException($"Country with ID {request.CountryId} was not found.");
            }

            City city = new City
            {
                Name = request.Name.Trim()
            };

            await unitOfWork.Locations.CreateCityAsync(city, request.CountryId);

            await unitOfWork.CommitAsync();

            CreateCityResponse response = new CreateCityResponse
            {
                Id = city.Id,
                Name = city.Name,
                CountryId = request.CountryId
            };

            return response;
        }
    }
}
