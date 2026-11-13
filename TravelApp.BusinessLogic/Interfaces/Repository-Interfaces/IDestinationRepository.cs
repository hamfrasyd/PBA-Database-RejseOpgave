using TravelApp.BusinessLogic.Models;

namespace TravelApp.BusinessLogic.Interfaces
{
    public interface IDestinationRepository
    {
        public Task<List<Continent>> GetContinentsAsync();
        public Task<List<Country>> GetCountriesAsync(int? continentId);

    }
}
