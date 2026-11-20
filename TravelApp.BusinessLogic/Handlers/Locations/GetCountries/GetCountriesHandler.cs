using TravelApp.BusinessLogic.DTOs.Locations;
using TravelApp.BusinessLogic.Interfaces;

namespace TravelApp.BusinessLogic.Handlers.Locations.GetCountries
{
    /// <summary>Handles the get countries use case.</summary>
    public class GetCountriesHandler
    {
        private readonly IUnitOfWorkFactory _unitOfWorkFactory;

        /// <summary>Initializes a new GetCountriesHandler instance.</summary>
        /// <param name="unitOfWorkFactory">The unit of work factory.</param>
        public GetCountriesHandler(IUnitOfWorkFactory unitOfWorkFactory)
        {
            _unitOfWorkFactory = unitOfWorkFactory;
        }

        /// <summary>Retrieves countries.</summary>
        /// <param name="continentId">The continent identifier to filter by, or null for all continents.</param>
        /// <returns>The matching countries.</returns>
        public async Task<List<GetCountriesResponse>> GetCountriesAsync(int? continentId)
        {
            await using IUnitOfWork unitOfWork = _unitOfWorkFactory.Create();

            List<CountryDTO> countriesFromDb = await unitOfWork.Locations.GetCountriesAsync(continentId);

            List<GetCountriesResponse> countriesResponse = countriesFromDb.Select(country => new GetCountriesResponse
            {
                Id = country.Id,
                ContinentId = country.ContinentId,
                Name = country.Name
            }).ToList();

            return countriesResponse;
        }
    }
}
