using TravelApp.BusinessLogic.DTOs.Locations;
using TravelApp.BusinessLogic.Interfaces;

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
        public async Task<List<GetCitiesResponse>> GetCitiesAsync(GetCitiesRequest request)
        {
            await using IUnitOfWork unitOfWork = _unitOfWorkFactory.Create();

            GetCitiesFilterDTO filter = new GetCitiesFilterDTO
            {
                CountryId = request.CountryId,
                Search = request.Search,
                Descending = request.Descending
            };

            List<CityDTO> citiesFromDb = await unitOfWork.Locations.GetCitiesAsync(filter);

            List<GetCitiesResponse> cities = new List<GetCitiesResponse>();
            foreach (CityDTO city in citiesFromDb)
            {
                cities.Add(new GetCitiesResponse
                {
                    Id = city.Id,
                    CountryId = city.CountryId,
                    Name = city.Name
                });
            }

            return cities;
        }
    }
}
