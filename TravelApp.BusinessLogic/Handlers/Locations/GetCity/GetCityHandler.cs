using TravelApp.BusinessLogic.Exceptions.SpecificCustomException;
using TravelApp.BusinessLogic.Handlers.Locations.Data;
using TravelApp.BusinessLogic.Interfaces;

namespace TravelApp.BusinessLogic.Handlers.Locations.GetCity
{
    /// <summary>Handles the get city use case.</summary>
    public class GetCityHandler
    {
        private readonly IUnitOfWorkFactory _unitOfWorkFactory;

        /// <summary>Initializes a new GetCityHandler instance.</summary>
        /// <param name="unitOfWorkFactory">The unit of work factory.</param>
        public GetCityHandler(IUnitOfWorkFactory unitOfWorkFactory)
        {
            _unitOfWorkFactory = unitOfWorkFactory;
        }

        /// <summary>Retrieves a city.</summary>
        /// <param name="cityId">The city identifier.</param>
        /// <returns>The city.</returns>
        /// <exception cref="TravelAppNotFoundException">Thrown when the city does not exist.</exception>
        public async Task<GetCityResponse> GetCityAsync(int cityId)
        {
            await using IUnitOfWork unitOfWork = _unitOfWorkFactory.Create();

            CityDTO? cityFromDb = await unitOfWork.Locations.GetCityAsync(cityId);
            if (cityFromDb is null)
            {
                throw new TravelAppNotFoundException($"City {cityId} was not found.");
            }

            GetCityResponse response = new GetCityResponse
            {
                Id = cityFromDb.Id,
                CountryId = cityFromDb.CountryId,
                Name = cityFromDb.Name
            };

            return response;
        }
    }
}
