using Microsoft.EntityFrameworkCore;
using TravelApp.BusinessLogic.Handlers.Destinations;
using TravelApp.BusinessLogic.Interfaces;
using TravelApp.BusinessLogic.Models;
using TravelApp.DataAccess.SQLRepositories.EntityFramework;

namespace TravelApp.DataAccess.SQLRepositories
{
    public class SqlDestinationRepository : IDestinationRepository
    {
        private readonly TravelAppContext _context;

        /// <summary>Initializes a new SqlDestinationRepository instance.</summary>
        public SqlDestinationRepository(TravelAppContext context)
        {
            _context = context;
        }

        /// <summary>Retrieves continents.</summary>
        public async Task<List<Continent>> GetContinentsAsync()
        {
            var result = await _context.ContinentEntities.AsNoTracking().OrderBy(item => item.Name).ToListAsync();
            return result;
        }

        /// <summary>Retrieves countries.</summary>
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

        /// <summary>Retrieves cities.</summary>
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

        /// <summary>Retrieves city by id.</summary>
        public async Task<City?> GetCityByIdAsync(int cityId)
        {
            City? matchingCity = await _context.CityEntities.Include(city => city.Country).FirstOrDefaultAsync(city => city.Id == cityId);
            return matchingCity;
        }

        /// <summary>Retrieves country by id.</summary>
        public async Task<Country?> GetCountryByIdAsync(int countryId)
        {
            Country? country = await _context.CountryEntities.FindAsync(countryId);
            return country;
        }

        /// <summary>Runs the country exists async operation.</summary>
        public async Task<bool> CountryExistsAsync(int countryId)
        {
            bool countryExists = await _context.CountryEntities.AnyAsync(country => country.Id == countryId);
            return countryExists;
        }

        /// <summary>Retrieves activity by id.</summary>
        public async Task<Activity?> GetActivityByIdAsync(int activityId)
        {
            Activity? matchingActivity = await _context.ActivityEntities.Include(activity => activity.City).FirstOrDefaultAsync(activity => activity.Id == activityId);
            return matchingActivity;
        }

        /// <summary>Retrieves activities for city.</summary>
        public async Task<List<Activity>> GetActivitiesForCityAsync(int cityId)
        {
            IQueryable<Activity> activities = _context.ActivityEntities
                .AsNoTracking()
                .Include(activity => activity.City)
                .Where(activity => activity.City!.Id == cityId)
                .OrderBy(activity => activity.Name)
                .ThenBy(activity => activity.Id);

            List<Activity> result = await activities.ToListAsync();
            return result;
        }

        /// <summary>Runs the activity belongs to city async operation.</summary>
        public async Task<bool> ActivityBelongsToCityAsync(int activityId, int cityId)
        {
            bool activityBelongsToCity = await _context.ActivityEntities.AnyAsync(activity => activity.Id == activityId && activity.City!.Id == cityId);
            return activityBelongsToCity;
        }

        /// <summary>Retrieves stats.</summary>
        public async Task<GetDestinationStatsResponse?> GetStatsAsync(int cityId)
        {
            GetDestinationStatsResponse? destinationStats = await _context.DestinationStatistics.AsNoTracking().FirstOrDefaultAsync(item => item.CityId == cityId);
            return destinationStats;
        }

        /// <summary>Adds city.</summary>
        public async Task AddCityAsync(City city)
        {
            await _context.CityEntities.AddAsync(city);
        }

        /// <summary>Adds activity.</summary>
        public async Task AddActivityAsync(Activity activity)
        {
            await _context.ActivityEntities.AddAsync(activity);
        }

        /// <summary>Updates city.</summary>
        public Task UpdateCityAsync(City city)
        {
            // The handler updated an entity already tracked by this Unit of Work.
            return Task.CompletedTask;
        }

        /// <summary>Updates activity.</summary>
        public Task UpdateActivityAsync(Activity activity)
        {
            // The handler updated an entity already tracked by this Unit of Work.
            return Task.CompletedTask;
        }

        /// <summary>Deletes city.</summary>
        public async Task<bool> DeleteCityAsync(int cityId)
        {
            if (_context.Database.CurrentTransaction is null)
            {
                await _context.Database.BeginTransactionAsync();
            }

            int deletedRows = await _context.CityEntities.Where(city => city.Id == cityId).ExecuteDeleteAsync();

            bool wasDeleted = deletedRows > 0;
            return wasDeleted;
        }

        /// <summary>Deletes activity.</summary>
        public async Task<bool> DeleteActivityAsync(int activityId)
        {
            if (_context.Database.CurrentTransaction is null)
            {
                await _context.Database.BeginTransactionAsync();
            }

            int deletedRows = await _context.ActivityEntities.Where(activity => activity.Id == activityId).ExecuteDeleteAsync();

            bool wasDeleted = deletedRows > 0;
            return wasDeleted;
        }
    }
}
