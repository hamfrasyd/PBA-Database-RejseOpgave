
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
        Task AddCityAsync(City city);

        #endregion

        #region Read - Continents, Countries, Cities, Stats

        /// <summary>Retrieves continents.</summary>
        Task<List<Continent>> GetContinentsAsync();

        /// <summary>Retrieves countries, optionally filtered by continent.</summary>
        Task<List<Country>> GetCountriesAsync(int? continentId);

        /// <summary>Retrieves countries by ID.</summary>
        Task<Country?> GetCountryByIdAsync(int countryId);

        /// <summary>Retrieves cities with optional filtering and sorting.</summary>
        Task<List<City>> GetCitiesAsync(GetCitiesRequest request);

        /// <summary>Retrieves a city by ID.</summary>
        Task<City?> GetCityByIdAsync(int cityId);

        /// <summary>Retrieves statistics for a city.</summary>
        Task<GetCityStatsResponse?> GetStatsAsync(int cityId);

        #endregion

        #region Update - Cities

        /// <summary>Updates a city.</summary>
        Task<bool> UpdateCityAsync(int cityId, UpdateCityRequest request);

        #endregion

        #region Delete - Cities

        /// <summary>Deletes a city.</summary>
        Task<bool> DeleteCityAsync(int cityId);

        #endregion

        #region Validation - Countries

        /// <summary>Checks whether a country exists.</summary>
        Task<bool> CountryExistsAsync(int countryId);

        #endregion
    }
}
