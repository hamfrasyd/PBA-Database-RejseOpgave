
using Microsoft.EntityFrameworkCore;
using TravelApp.BusinessLogic.Handlers.Locations.GetCities;
using TravelApp.BusinessLogic.Handlers.Locations.GetDestinationStats;
using TravelApp.BusinessLogic.Handlers.Locations.UpdateCity;
using TravelApp.BusinessLogic.Interfaces;
using TravelApp.BusinessLogic.Models.Destinations;
using TravelApp.DataAccess.SQLRepositories.EntityFramework;

namespace TravelApp.DataAccess.SQLRepositories
{
    public class SqlLocationsRepository : ILocationsRepository
    {
        private readonly TravelAppContext _context;

        public SqlLocationsRepository(TravelAppContext context)
        {
            _context = context;
        }

        #region Create - Cities

        public async Task AddCityAsync(City city)
        {
            await _context.CityEntities.AddAsync(city);
        }

        #endregion

        #region Read - Continents, Countries, Cities, Stats

        public async Task<List<Continent>> GetContinentsAsync()
        {
            List<Continent> result = await _context.ContinentEntities
                .AsNoTracking()
                .OrderBy(continent => continent.Name)
                .ToListAsync();

            return result;
        }

        public async Task<List<Country>> GetCountriesAsync(int? continentId)
        {
            IQueryable<Country> countries = _context.CountryEntities
                .AsNoTracking()
                .Include(country => country.Continent);

            if (continentId.HasValue)
            {
                countries = countries.Where(country => country.Continent!.Id == continentId.Value);
            }

            List<Country> result = await countries
                .OrderBy(country => country.Name)
                .ToListAsync();

            return result;
        }

        public async Task<Country?> GetCountryByIdAsync(int countryId)
        {
            Country? country = await _context.CountryEntities.FindAsync(countryId);

            return country;
        }

        public async Task<List<City>> GetCitiesAsync(GetCitiesRequest request)
        {
            IQueryable<City> cities = _context.CityEntities
                .AsNoTracking()
                .Include(city => city.Country);

            if (request.CountryId.HasValue)
            {
                cities = cities.Where(city => city.Country!.Id == request.CountryId.Value);
            }

            if (string.IsNullOrWhiteSpace(request.Search) is false)
            {
                string search = request.Search.Trim();

                cities = cities.Where(city => city.Name.Contains(search));
            }

            cities = request.Descending
                ? cities.OrderByDescending(city => city.Name).ThenBy(city => city.Id)
                : cities.OrderBy(city => city.Name).ThenBy(city => city.Id);

            List<City> result = await cities.ToListAsync();

            return result;
        }

        public async Task<City?> GetCityByIdAsync(int cityId)
        {
            City? matchingCity = await _context.CityEntities
                .Include(city => city.Country)
                .FirstOrDefaultAsync(city => city.Id == cityId);

            return matchingCity;
        }

        public async Task<GetCityStatsResponse?> GetStatsAsync(int cityId)
        {
            GetCityStatsResponse? destinationStats = await _context.DestinationStatistics
                .AsNoTracking()
                .FirstOrDefaultAsync(item => item.CityId == cityId);

            return destinationStats;
        }

        #endregion

        #region Update - Cities

        public async Task<bool> UpdateCityAsync(int cityId, UpdateCityRequest request)
        {
            City? cityInDb = await _context.CityEntities
                .FirstOrDefaultAsync(city => city.Id == cityId);

            if (cityInDb is null)
            {
                return false;
            }

            cityInDb.Name = request.Name.Trim();
            cityInDb.Country = request.Country;

            return true;
        }

        #endregion

        #region Delete - Cities

        public async Task<bool> DeleteCityAsync(int cityId)
        {
            if (_context.Database.CurrentTransaction is null)
            {
                await _context.Database.BeginTransactionAsync();
            }

            int deletedRows = await _context.CityEntities
                .Where(city => city.Id == cityId)
                .ExecuteDeleteAsync();

            bool wasDeleted = deletedRows > 0;

            return wasDeleted;
        }

        #endregion

        #region Validation - Countries

        public async Task<bool> CountryExistsAsync(int countryId)
        {
            bool countryExists = await _context.CountryEntities
                .AnyAsync(country => country.Id == countryId);

            return countryExists;
        }

        #endregion
    }
}
