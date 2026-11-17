using TravelApp.BusinessLogic.DTOs.Trips;
using TravelApp.BusinessLogic.Models.Trips;

namespace TravelApp.BusinessLogic.Interfaces
{
    /// <summary>Defines database operations for trips.</summary>
    public interface ITripRepository
    {
        /// <summary>Retrieves trip by id.</summary>
        Task<Trip?> GetByIdAsync(int tripId);

        /// <summary>Retrieves trips for user.</summary>
        Task<List<Trip>> GetByUserAsync(int userId);

        /// <summary>Retrieves trip details by id.</summary>
        Task<TripDetailsDTO?> GetDetailsAsync(int tripId);

        /// <summary>Retrieves trip summaries for user with optional filtering by country and continent.</summary>
        Task<List<TripSummaryDTO>> GetSummariesByUserAsync(int userId, int? countryId, int? continentId);

        /// <summary>Adds trip for user.</summary>
        Task AddForUserAsync(Trip trip, int userId);

        /// <summary>Updates trip.</summary>
        void Update(Trip trip);

        Task<TripAccessInfoDTO?> GetAccessInfoAsync(int tripId, int userId);

        /// <summary>Deletes trip.</summary>
        Task DeleteAsync(int tripId);

        /// <summary>Checks if city exists.</summary>
        Task<bool> CityExistsAsync(int cityId);

        /// <summary>Retrieves stop for trip and city.</summary>
        Task<TripStop?> GetStopAsync(int tripId, int cityId);

        /// <summary>Retrieves activity for trip.</summary>
        Task<TripActivity?> GetTripActivityAsync(
            int tripId, int activityId);

        /// <summary>Retrieves participant for trip.</summary>
        Task<TripParticipant?> GetParticipantAsync(
            int tripId, int userId);

        /// <summary>Adds stop to trip.</summary>
        void AddStop(TripStop stop);

        /// <summary>Adds activity to trip.</summary>
        void AddTripActivity(TripActivity activity);

        /// <summary>Adds participant to trip.</summary>
        void AddParticipant(TripParticipant participant);

        /// <summary>Removes stop from trip.</summary>
        void RemoveStop(TripStop stop);

        /// <summary>Removes activity from trip.</summary>
        void RemoveTripActivity(TripActivity activity);

        /// <summary>Removes participant from trip.</summary>
        void RemoveParticipant(TripParticipant participant);






        Task<TripRelationsDTO?> GetRelationsAsync(int tripId);

        Task AddCityAsync(int tripId, int cityId);
        Task RemoveCityAsync(int tripId, int cityId);

        Task AddActivityAsync(int tripId, int activityId);
        Task RemoveActivityAsync(int tripId, int activityId);

        Task AddParticipantAsync(int tripId, int userId);
        Task RemoveParticipantAsync(int tripId, int userId);
    }
}
