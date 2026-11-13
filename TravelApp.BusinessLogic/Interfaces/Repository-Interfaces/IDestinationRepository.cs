using TravelApp.BusinessLogic.Handlers.Destinations;
using TravelApp.BusinessLogic.Models;

namespace TravelApp.BusinessLogic.Interfaces
{
    /// <summary>Defines database operations for destinations.</summary>
    public interface IDestinationRepository
    {
        /// <summary>Retrieves continents.</summary>
        public Task<List<Continent>> GetContinentsAsync();
        /// <summary>Retrieves countries.</summary>
        public Task<List<Country>> GetCountriesAsync(int? continentId);
        /// <summary>Retrieves cities.</summary>
        public Task<List<City>> GetCitiesAsync(GetCitiesRequest request);

    }
}
