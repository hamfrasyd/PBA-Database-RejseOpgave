using TravelApp.BusinessLogic.Models;
using TravelApp.BusinessLogic.Interfaces;

namespace TravelApp.BusinessLogic.Handlers.Destinations
{
    /// <summary>Handles the get cities use case.</summary>
    public class GetCitiesHandler
    {
        private readonly IUnitOfWorkFactory _unitOfWorkFactory;

        /// <summary>Initializes a new GetCitiesHandler instance.</summary>
        public GetCitiesHandler(IUnitOfWorkFactory unitOfWorkFactory)
        {
            _unitOfWorkFactory = unitOfWorkFactory;
        }

        /// <summary>Retrieves cities.</summary>
        public async Task<List<GetCitiesResponse>> GetCitiesAsync(GetCitiesRequest request)
        {
            await using IUnitOfWork unitOfWork = _unitOfWorkFactory.Create();

            List<City> citiesFromDb = await unitOfWork.Destinations.GetCitiesAsync(request);

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
