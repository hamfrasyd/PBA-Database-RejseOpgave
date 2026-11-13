using Microsoft.EntityFrameworkCore;
using TravelApp.BusinessLogic.Handlers.Destinations;
using TravelApp.BusinessLogic.Interfaces;
using TravelApp.BusinessLogic.Models;
using TravelApp.DataAccess.SQLRepositories.EntityFramework;

namespace TravelApp.DataAccess.SQLRepositories
{
    internal class SqlDestinationRepository : IDestinationRepository
    {
        private readonly TravelAppContext _context;

        public SqlDestinationRepository(TravelAppContext context)
        {
            _context = context;
        }

        public async Task<List<Continent>> GetContinentsAsync()
        {
            var result = await _context.ContinentEntities.AsNoTracking().OrderBy(item => item.Name).ToListAsync();
            return result;
        }

        public async Task<List<Country>> GetCountriesAsync(int? continentId)
        {
            IQueryable<Country> countries = _context.CountryEntities.AsNoTracking().Include(country => country.Continent);

            if (continentId.HasValue)
            {
                countries = countries.Where(country => country.Continent!.Id == continentId.Value);
            }

            List<Country> result = await countries.OrderBy(country => country.Name).ToListAsync();
            return result;
        }

        public async Task<List<City>> GetCitiesAsync(GetCitiesRequest request)
        {
            IQueryable<City> cities = _context.CityEntities.AsNoTracking().Include(city => city.Country);
            if (request.CountryId.HasValue)
            {
                cities = cities.Where(city => city.Country!.Id == request.CountryId.Value);
            }

            if (string.IsNullOrWhiteSpace(request.Search) is false)
            {
                string search = request.Search.Trim();
                cities = cities.Where(city => city.Name.Contains(search));
            }

            cities = request.Descending ? cities.OrderByDescending(city => city.Name).ThenBy(city => city.Id) : cities.OrderBy(city => city.Name).ThenBy(city => city.Id);
            List<City> result = await cities.ToListAsync();
            return result;
        }
    }
}
