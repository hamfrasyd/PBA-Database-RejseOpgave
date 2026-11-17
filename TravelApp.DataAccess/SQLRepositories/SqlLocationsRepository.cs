using Microsoft.EntityFrameworkCore;
using TravelApp.BusinessLogic.Handlers.Locations.Data;
using TravelApp.BusinessLogic.Interfaces;
using TravelApp.BusinessLogic.Models.Destinations;
using TravelApp.DataAccess.SQLRepositories.EntityFramework;

namespace TravelApp.DataAccess.SQLRepositories
{
    /// <summary>Implements database operations for cities, countries, and continents using Entity Framework Core.</summary>
    public class SqlLocationsRepository : ILocationsRepository
    {
        private readonly TravelAppContext _context;
        private City? _addedCity;

        /// <summary>Initializes a new instance of the <see cref="SqlLocationsRepository"/> class.</summary>
        /// <param name="context">The Entity Framework Core context connected to SQL Server.</param>
        public SqlLocationsRepository(TravelAppContext context)
        {
            _context = context;
        }

        #region Create - Cities

        /// <inheritdoc />
        public async Task<CityDto?> CreateCityAsync(CreateCityDto cityToCreate)
        {
            _addedCity = new City
            {
                Name = cityToCreate.Name
            };

            await _context.CityEntities.AddAsync(_addedCity);
            _context.Entry(_addedCity).Property("CountryId").CurrentValue = cityToCreate.CountryId;
            
            int countryId = _context.Entry(_addedCity).Property<int>("CountryId").CurrentValue;

            if (_addedCity.Id <= 0)
            {
                return null;
            }

            CityDto createdCity = new CityDto
            {
                Id = _addedCity.Id,
                Name = _addedCity.Name,
                CountryId = countryId
            };

            return createdCity;
        }

        /// <inheritdoc />
        public CityDto GetCreatedCity()
        {
            if (_addedCity is null)
            {
                throw new InvalidOperationException("CreateCityAsync must be called before GetCreatedCity.");
            }

            int countryId = _context.Entry(_addedCity).Property<int>("CountryId").CurrentValue;

            CityDto createdCity = new CityDto
            {
                Id = _addedCity.Id,
                Name = _addedCity.Name,
                CountryId = countryId
            };

            return createdCity;
        }

        #endregion

        #region Read - All Cities, Countries & Continents

        /// <inheritdoc />
        public async Task<List<CityDto>> GetCitiesAsync(GetCitiesFilterData filter)
        {
            IQueryable<City> cities = _context.CityEntities.AsNoTracking();

            if (filter.CountryId.HasValue)
            {
                int countryId = filter.CountryId.Value;
                cities = cities.Where(city => EF.Property<int>(city, "CountryId") == countryId);
            }

            if (string.IsNullOrWhiteSpace(filter.Search) is false)
            {
                string search = filter.Search.Trim();
                cities = cities.Where(city => city.Name.Contains(search));
            }

            IOrderedQueryable<City> orderedCities = filter.Descending
                ? cities.OrderByDescending(city => city.Name).ThenBy(city => city.Id)
                : cities.OrderBy(city => city.Name).ThenBy(city => city.Id);

            List<CityDto> result = await orderedCities
                .Select(city => new CityDto
                {
                    Id = city.Id,
                    Name = city.Name,
                    CountryId = EF.Property<int>(city, "CountryId")
                })
                .ToListAsync();

            return result;
        }

        /// <inheritdoc />
        public async Task<List<CountryData>> GetCountriesAsync(int? continentId)
        {
            IQueryable<Country> countries = _context.CountryEntities.AsNoTracking();

            if (continentId.HasValue)
            {
                int selectedContinentId = continentId.Value;
                countries = countries.Where(country => EF.Property<int>(country, "ContinentId") == selectedContinentId);
            }

            List<CountryData> result = await countries
                .OrderBy(country => country.Name)
                .Select(country => new CountryData
                {
                    Id = country.Id,
                    Name = country.Name,
                    ContinentId = EF.Property<int>(country, "ContinentId")
                })
                .ToListAsync();

            return result;
        }

        /// <inheritdoc />
        public async Task<List<ContinentData>> GetContinentsAsync()
        {
            List<ContinentData> result = await _context.ContinentEntities
                .AsNoTracking()
                .OrderBy(continent => continent.Name)
                .Select(continent => new ContinentData
                {
                    Id = continent.Id,
                    Name = continent.Name
                })
                .ToListAsync();

            return result;
        }

        #endregion

        #region Read - By-ID Cities & Stats

        /// <inheritdoc />
        public async Task<CityDto?> GetCityAsync(int cityId)
        {
            CityDto? cityData = await _context.CityEntities
                .AsNoTracking()
                .Where(city => city.Id == cityId)
                .Select(city => new CityDto
                {
                    Id = city.Id,
                    Name = city.Name,
                    CountryId = EF.Property<int>(city, "CountryId")
                })
                .FirstOrDefaultAsync();

            return cityData;
        }

        /// <inheritdoc />
        public async Task<City?> GetCityByIdAsync(int cityId)
        {
            City? matchingCity = await _context.CityEntities
                .Include(city => city.Country)
                .FirstOrDefaultAsync(city => city.Id == cityId);

            return matchingCity;
        }

        /// <inheritdoc />
        public async Task<CityStatsData?> GetCityStatsAsync(int cityId)
        {
            CityStatsData? cityStats = await _context.DestinationStatistics
                .AsNoTracking()
                .FirstOrDefaultAsync(item => item.CityId == cityId);

            return cityStats;
        }

        #endregion

        #region Update - Cities

        /// <inheritdoc />
        public async Task<UpdateCityData?> UpdateCityAsync(UpdateCityData cityToUpdate)
        {
            City? cityInDb = await _context.CityEntities
                .FirstOrDefaultAsync(city => city.Id == cityToUpdate.Id);

            if (cityInDb is null)
            {
                return null;
            }

            cityInDb.Name = cityToUpdate.Name;
            _context.Entry(cityInDb).Property("CountryId").CurrentValue = cityToUpdate.CountryId;

            UpdateCityData updatedCityData = new UpdateCityData
            {
                Id = cityInDb.Id,
                Name = cityInDb.Name,
                CountryId = cityToUpdate.CountryId
            };

            return updatedCityData;
        }

        #endregion

        #region Delete - Cities

        /// <inheritdoc />
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

        /// <inheritdoc />
        public async Task<bool> CountryExistsAsync(int countryId)
        {
            bool countryExists = await _context.CountryEntities.AnyAsync(country => country.Id == countryId);

            return countryExists;
        }

        #endregion
    }
}
