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
        public async Task<CityDTO?> CreateCityAsync(CreateCityDTO cityToCreate)
        {
            _addedCity = new City
            {
                Name = cityToCreate.Name
            };

            await _context.CityEntities.AddAsync(_addedCity);
            _context.Entry(_addedCity).Property("CountryId").CurrentValue = cityToCreate.CountryId;

            // cowboy, skal bruge cecilias måde at Create på
            int countryId = _context.Entry(_addedCity).Property<int>("CountryId").CurrentValue;

            if (_addedCity.Id <= 0)
            {
                return null;
            }

            CityDTO createdCity = new CityDTO
            {
                Id = _addedCity.Id,
                Name = _addedCity.Name,
                CountryId = countryId
            };

            return createdCity;
        }

        /// <inheritdoc />
        public CityDTO GetCreatedCity()
        {
            if (_addedCity is null)
            {
                throw new InvalidOperationException("CreateCityAsync must be called before GetCreatedCity.");
            }

            int countryId = _context.Entry(_addedCity).Property<int>("CountryId").CurrentValue;

            CityDTO createdCity = new CityDTO
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
        public async Task<List<CityDTO>> GetCitiesAsync(GetCitiesFilterDTO filter)
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

            List<CityDTO> result = await orderedCities
                                        .Select(city => new CityDTO
                                        {
                                            Id = city.Id,
                                            Name = city.Name,
                                            CountryId = EF.Property<int>(city, "CountryId")
                                        })
                                        .ToListAsync();

            return result;
        }

        /// <inheritdoc />
        public async Task<List<CountryDTO>> GetCountriesAsync(int? continentId)
        {
            IQueryable<Country> countries = _context.CountryEntities.AsNoTracking();

            if (continentId.HasValue)
            {
                int selectedContinentId = continentId.Value;
                countries = countries.Where(country => EF.Property<int>(country, "ContinentId") == selectedContinentId);
            }

            List<CountryDTO> result = await countries
                .OrderBy(country => country.Name)
                .Select(country => new CountryDTO
                {
                    Id = country.Id,
                    Name = country.Name,
                    ContinentId = EF.Property<int>(country, "ContinentId")
                })
                .ToListAsync();

            return result;
        }

        /// <inheritdoc />
        public async Task<List<ContinentDTO>> GetContinentsAsync()
        {
            List<ContinentDTO> result = await _context.ContinentEntities
                .AsNoTracking()
                .OrderBy(continent => continent.Name)
                .Select(continent => new ContinentDTO
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
        public async Task<CityDTO?> GetCityAsync(int cityId)
        {
            CityDTO? cityData = await _context.CityEntities
                .AsNoTracking()
                .Where(city => city.Id == cityId)
                .Select(city => new CityDTO
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
        public async Task<CityStatsDTO?> GetCityStatsAsync(int cityId)
        {
            CityStatsDTO? cityStats = await _context.DestinationStatistics
                .AsNoTracking()
                .FirstOrDefaultAsync(item => item.CityId == cityId);

            return cityStats;
        }

        #endregion

        #region Validation - Cities & Countries

        /// <inheritdoc />
        public async Task<bool> CityExistsAsync(int cityId)
        {
            bool cityExists = await _context.CityEntities.AnyAsync(city => city.Id == cityId);

            return cityExists;
        }

        /// <inheritdoc />
        public async Task<bool> CountryExistsAsync(int countryId)
        {
            bool countryExists = await _context.CountryEntities.AnyAsync(country => country.Id == countryId);

            return countryExists;
        }

        #endregion
    }
}
