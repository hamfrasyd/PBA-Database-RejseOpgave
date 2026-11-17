using TravelApp.BusinessLogic.DTOs.Locations;
using TravelApp.BusinessLogic.Models.Locations;

namespace TravelApp.BusinessLogic.Interfaces
{
    /// <summary>Defines database operations for geographic locations.</summary>
    public interface ILocationsRepository
    {
        #region Create - Cities

        /// <summary>Stages a city for insertion in the given country.</summary>
        /// <param name="city">The city to add.</param>
        /// <param name="countryId">The country identifier.</param>
        /// <returns>A task that completes when the city is staged. The generated identifier is available on <paramref name="city"/> after the unit of work commits.</returns>
        Task CreateCityAsync(City city, int countryId);

        #endregion

        #region Read - All Cities, Countries & Continents

        /// <summary>Retrieves cities with optional filtering and sorting.</summary>
        /// <param name="filter">The filter and sort options.</param>
        /// <returns>The matching cities.</returns>
        Task<List<CityDTO>> GetCitiesAsync(GetCitiesFilterDTO filter);

        /// <summary>Retrieves countries, optionally filtered by continent.</summary>
        /// <param name="continentId">The continent identifier to filter by, or <see langword="null"/> for all continents.</param>
        /// <returns>The matching countries.</returns>
        Task<List<CountryDTO>> GetCountriesAsync(int? continentId);

        /// <summary>Retrieves continents.</summary>
        /// <returns>The continents.</returns>
        Task<List<ContinentDTO>> GetContinentsAsync();

        #endregion

        #region Read - By-ID Cities & Stats

        /// <summary>Retrieves a city by ID.</summary>
        /// <param name="cityId">The city identifier.</param>
        /// <returns>The city, or <see langword="null"/> when it does not exist.</returns>
        Task<CityDTO?> GetCityAsync(int cityId);

        /// <summary>Retrieves a city by ID.</summary>
        /// <param name="cityId">The city identifier.</param>
        /// <returns>The city, or <see langword="null"/> when it does not exist.</returns>
        Task<City?> GetCityByIdAsync(int cityId);

        /// <summary>Retrieves statistics for a city.</summary>
        /// <param name="cityId">The city identifier.</param>
        /// <returns>The statistics, or <see langword="null"/> when the city does not exist.</returns>
        Task<CityStatsDTO?> GetCityStatsAsync(int cityId);

        #endregion

        #region Validation - Cities & Countries

        /// <summary>Checks whether a city exists.</summary>
        /// <param name="cityId">The city identifier.</param>
        /// <returns><see langword="true"/> when the city exists; otherwise, <see langword="false"/>.</returns>
        Task<bool> CityExistsAsync(int cityId);

        /// <summary>Checks whether a country exists.</summary>
        /// <param name="countryId">The country identifier.</param>
        /// <returns><see langword="true"/> when the country exists; otherwise, <see langword="false"/>.</returns>
        Task<bool> CountryExistsAsync(int countryId);

        #endregion
    }
}
