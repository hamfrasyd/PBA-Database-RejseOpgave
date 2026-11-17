using TravelApp.BusinessLogic.Handlers.Locations.GetCities;
using TravelApp.BusinessLogic.Handlers.Locations.GetDestinationStats;
using TravelApp.BusinessLogic.Handlers.Locations.UpdateCity;
using TravelApp.BusinessLogic.Models.Destinations;

namespace TravelApp.BusinessLogic.Interfaces
{
    /// <summary>Defines database operations for geographic locations.</summary>
    public interface ILocationsRepository
    {
        #region Create - Cities

        /// <summary>Adds a city.</summary>
        /// <param name="city">The city to add.</param>
        /// <returns>A task that completes when the city is added.</returns>
        Task AddCityAsync(City city);

        #endregion

        #region Read - All Cities, Countries & Continents

        /// <summary>Retrieves cities with optional filtering and sorting.</summary>
        /// <param name="request">The filter and sort options.</param>
        /// <returns>The matching cities.</returns>
        Task<List<City>> GetCitiesAsync(GetCitiesRequest request);

        /// <summary>Retrieves countries, optionally filtered by continent.</summary>
        /// <param name="continentId">The continent identifier to filter by, or <see langword="null"/> for all continents.</param>
        /// <returns>The matching countries.</returns>
        Task<List<Country>> GetCountriesAsync(int? continentId);

        /// <summary>Retrieves continents.</summary>
        /// <returns>The continents.</returns>
        Task<List<Continent>> GetContinentsAsync();

        #endregion

        #region Read - By-ID Cities, Stats-for-a-city & Countries

        /// <summary>Retrieves a city by ID.</summary>
        /// <param name="cityId">The city identifier.</param>
        /// <returns>The city, or <see langword="null"/> when it does not exist.</returns>
        Task<City?> GetCityByIdAsync(int cityId);

        /// <summary>Retrieves statistics for a city.</summary>
        /// <param name="cityId">The city identifier.</param>
        /// <returns>The statistics, or <see langword="null"/> when the city does not exist.</returns>
        Task<GetCityStatsResponse?> GetStatsAsync(int cityId);

        /// <summary>Retrieves a country by ID.</summary>
        /// <param name="countryId">The country identifier.</param>
        /// <returns>The country, or <see langword="null"/> when it does not exist.</returns>
        Task<Country?> GetCountryByIdAsync(int countryId);

        #endregion

        #region Update - Cities

        /// <summary>Updates a city.</summary>
        /// <param name="cityId">The city identifier.</param>
        /// <param name="request">The updated city details.</param>
        /// <returns><see langword="true"/> when the city was updated; otherwise, <see langword="false"/>.</returns>
        Task<bool> UpdateCityAsync(int cityId, UpdateCityRequest request);

        #endregion

        #region Delete - Cities

        /// <summary>Deletes a city.</summary>
        /// <param name="cityId">The city identifier.</param>
        /// <returns><see langword="true"/> when the city was deleted; otherwise, <see langword="false"/>.</returns>
        Task<bool> DeleteCityAsync(int cityId);

        #endregion

        #region Validation - Countries

        /// <summary>Checks whether a country exists.</summary>
        /// <param name="countryId">The country identifier.</param>
        /// <returns><see langword="true"/> when the country exists; otherwise, <see langword="false"/>.</returns>
        Task<bool> CountryExistsAsync(int countryId);

        #endregion
    }
}
