using TravelApp.BusinessLogic.Interfaces;
using TravelApp.BusinessLogic.Models.Destinations;

namespace TravelApp.BusinessLogic.Handlers.Locations.GetCities
{
    /// <summary>Handles the get cities use case.</summary>
    public class GetCitiesHandler
    {
        private readonly IUnitOfWorkFactory _unitOfWorkFactory;

        /// <summary>Initializes a new GetCitiesHandler instance.</summary>
        /// <param name="unitOfWorkFactory">The unit of work factory.</param>
        public GetCitiesHandler(IUnitOfWorkFactory unitOfWorkFactory)
        {
            _unitOfWorkFactory = unitOfWorkFactory;
        }

        /// <summary>Retrieves cities.</summary>
        /// <param name="request">The filter and sort options.</param>
        /// <returns>The matching cities.</returns>
        /// <exception cref="InvalidOperationException">Thrown when a city's country was not loaded.</exception>
        public async Task<List<GetCitiesResponse>> GetCitiesAsync(GetCitiesRequest request)
        {
            await using IUnitOfWork unitOfWork = _unitOfWorkFactory.Create();

            List<City> citiesFromDb = await unitOfWork.Locations.GetCitiesAsync(request);

            List<GetCitiesResponse> cities = new List<GetCitiesResponse>();

            foreach (City city in citiesFromDb)
            {
                cities.Add(new GetCitiesResponse
                {
                    Id = city.Id,
                    CountryId = city.Country?.Id ?? throw new InvalidOperationException("Required Country relationship was not loaded."),
                    Name = city.Name
                });
            }

            return cities;
        }
    }
}
