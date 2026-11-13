using TravelApp.BusinessLogic.Models;
using TravelApp.BusinessLogic.Interfaces;

namespace TravelApp.BusinessLogic.Handlers.Destinations
{
    /// <summary>Handles the get countries use case.</summary>
    public class GetCountriesHandler
    {
        private readonly IUnitOfWorkFactory _unitOfWorkFactory;

        /// <summary>Initializes a new GetCountriesHandler instance.</summary>
        public GetCountriesHandler(IUnitOfWorkFactory unitOfWorkFactory)
        {
            _unitOfWorkFactory = unitOfWorkFactory;
        }

        /// <summary>Retrieves countries.</summary>
        public async Task<List<GetCountriesResponse>> GetCountriesAsync(int? continentId)
        {
            await using IUnitOfWork unitOfWork = _unitOfWorkFactory.Create();

            List<Country> countriesFromDb = await unitOfWork.Destinations.GetCountriesAsync(continentId);

            List<GetCountriesResponse> countriesResponse = new List<GetCountriesResponse>();

            foreach (Country country in countriesFromDb)
            {
                countriesResponse.Add(new GetCountriesResponse
                {
                    Id = country.Id,
                    ContinentId = country.Continent?.Id ?? throw new InvalidOperationException("Required Continent relationship was not loaded."),
                    Name = country.Name
                });
            }

            return countriesResponse;
        }
    }
}
