using TravelApp.BusinessLogic.Handlers.Locations.Data;
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
        /// <param name="continentId">The continent identifier to filter by, or <see langword="null"/> for all continents.</param>
        /// <returns>The matching countries.</returns>
        public async Task<List<GetCountriesResponse>> GetCountriesAsync(int? continentId)
        {
            await using IUnitOfWork unitOfWork = _unitOfWorkFactory.Create();

            List<CountryDto> countriesFromDb = await unitOfWork.Locations.GetCountriesAsync(continentId);

            List<GetCountriesResponse> countriesResponse = new List<GetCountriesResponse>();

            foreach (CountryDto country in countriesFromDb)
            {
                countriesResponse.Add(new GetCountriesResponse
                {
                    Id = country.Id,
                    ContinentId = country.ContinentId,
                    Name = country.Name
                });
            }

            return countriesResponse;
        }
    }
}
