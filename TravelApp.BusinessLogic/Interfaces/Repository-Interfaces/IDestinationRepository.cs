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
        /// <summary>Retrieves city by id.</summary>
        public Task<City?> GetCityByIdAsync(int cityId);
        /// <summary>Runs the country exists async operation.</summary>
        public Task<bool> CountryExistsAsync(int countryId);
        /// <summary>Retrieves country by id.</summary>
        public Task<Country?> GetCountryByIdAsync(int countryId);
        /// <summary>Retrieves activity by id.</summary>
        public Task<Activity?> GetActivityByIdAsync(int activityId);
        /// <summary>Retrieves activities for city.</summary>
        public Task<List<Activity>> GetActivitiesForCityAsync(int cityId);
        /// <summary>Runs the activity belongs to city async operation.</summary>
        public Task<bool> ActivityBelongsToCityAsync(int activityId, int cityId);
        /// <summary>Retrieves stats.</summary>
        public Task<GetDestinationStatsResponse?> GetStatsAsync(int cityId);
        /// <summary>Adds city.</summary>
        public Task AddCityAsync(City city);
        /// <summary>Adds activity.</summary>
        public Task AddActivityAsync(Activity activity);
        /// <summary>Updates city.</summary>
        public Task UpdateCityAsync(City city);
        /// <summary>Updates activity.</summary>
        public Task UpdateActivityAsync(Activity activity);
        /// <summary>Deletes city.</summary>
        public Task<bool> DeleteCityAsync(int cityId);
        /// <summary>Deletes activity.</summary>
        public Task<bool> DeleteActivityAsync(int activityId);
    }
}
