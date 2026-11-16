using TravelApp.BusinessLogic.DTOs.Trips;
using TravelApp.BusinessLogic.Models.Trips;

namespace TravelApp.BusinessLogic.Interfaces
{
    /// <summary>Defines database operations for trips.</summary>
    public interface ITripRepository
    {
        #region Create - Trips

        /// <summary>Adds a trip for a user.</summary>
        /// <param name="trip">The trip to add.</param>
        /// <param name="userId">The user identifier.</param>
        /// <returns>A task that completes when the trip is added.</returns>
        Task AddForUserAsync(Trip trip, int userId);

        /// <summary>Adds a stop to a trip.</summary>
        /// <param name="stop">The stop to add.</param>
        void AddStop(TripStop stop);

        /// <summary>Adds an activity to a trip.</summary>
        /// <param name="activity">The trip activity to add.</param>
        void AddTripActivity(TripActivity activity);

        /// <summary>Adds a participant to a trip.</summary>
        /// <param name="participant">The participant to add.</param>
        void AddParticipant(TripParticipant participant);

        /// <summary>Adds an existing city to a trip.</summary>
        /// <param name="tripId">The trip identifier.</param>
        /// <param name="cityId">The city identifier.</param>
        /// <returns>A task that completes when the city is added.</returns>
        Task AddCityAsync(int tripId, int cityId);

        /// <summary>Adds an existing activity to a trip.</summary>
        /// <param name="tripId">The trip identifier.</param>
        /// <param name="activityId">The activity identifier.</param>
        /// <returns>A task that completes when the activity is added.</returns>
        Task AddActivityAsync(int tripId, int activityId);

        /// <summary>Adds an existing user as a trip participant.</summary>
        /// <param name="tripId">The trip identifier.</param>
        /// <param name="userId">The user identifier.</param>
        /// <returns>A task that completes when the participant is added.</returns>
        Task AddParticipantAsync(int tripId, int userId);

        #endregion

        #region Read - All

        /// <summary>Retrieves trips for a user.</summary>
        /// <param name="userId">The user identifier.</param>
        /// <returns>The user's trips.</returns>
        Task<List<Trip>> GetByUserAsync(int userId);

        /// <summary>Retrieves trip summaries for a user with optional country and continent filtering.</summary>
        /// <param name="userId">The user identifier.</param>
        /// <param name="countryId">The country identifier to filter by, or <see langword="null"/> for all countries.</param>
        /// <param name="continentId">The continent identifier to filter by, or <see langword="null"/> for all continents.</param>
        /// <returns>The matching trip summaries.</returns>
        Task<List<TripSummaryDTO>> GetSummariesByUserAsync(int userId, int? countryId, int? continentId);

        #endregion

        #region Read - By ID

        /// <summary>Retrieves a trip by ID.</summary>
        /// <param name="tripId">The trip identifier.</param>
        /// <returns>The trip, or <see langword="null"/> when it does not exist.</returns>
        Task<Trip?> GetByIdAsync(int tripId);

        /// <summary>Retrieves trip details by ID.</summary>
        /// <param name="tripId">The trip identifier.</param>
        /// <returns>The trip details, or <see langword="null"/> when the trip does not exist.</returns>
        Task<TripDetailsDTO?> GetDetailsAsync(int tripId);

        /// <summary>Retrieves cities, activities and participants associated with a trip.</summary>
        /// <param name="tripId">The trip identifier.</param>
        /// <returns>The related identifiers, or <see langword="null"/> when the trip does not exist.</returns>
        Task<TripRelationsDTO?> GetRelationsAsync(int tripId);

        /// <summary>Retrieves a stop for a specific trip and city.</summary>
        /// <param name="tripId">The trip identifier.</param>
        /// <param name="cityId">The city identifier.</param>
        /// <returns>The stop, or <see langword="null"/> when it does not exist.</returns>
        Task<TripStop?> GetStopAsync(int tripId, int cityId);

        /// <summary>Retrieves an activity associated with a trip.</summary>
        /// <param name="tripId">The trip identifier.</param>
        /// <param name="activityId">The activity identifier.</param>
        /// <returns>The trip activity, or <see langword="null"/> when it does not exist.</returns>
        Task<TripActivity?> GetTripActivityAsync(int tripId, int activityId);

        /// <summary>Retrieves a participant associated with a trip.</summary>
        /// <param name="tripId">The trip identifier.</param>
        /// <param name="userId">The user identifier.</param>
        /// <returns>The participant, or <see langword="null"/> when it does not exist.</returns>
        Task<TripParticipant?> GetParticipantAsync(int tripId, int userId);

        #endregion

        #region Update - Trips

        /// <summary>Updates a trip.</summary>
        /// <param name="trip">The trip to update.</param>
        void Update(Trip trip);

        #endregion

        #region Delete - Trips

        /// <summary>Deletes a trip.</summary>
        /// <param name="tripId">The trip identifier.</param>
        /// <returns>A task that completes when the trip is deleted.</returns>
        Task DeleteAsync(int tripId);

        /// <summary>Removes a stop from a trip.</summary>
        /// <param name="stop">The stop to remove.</param>
        void RemoveStop(TripStop stop);

        /// <summary>Removes an activity from a trip.</summary>
        /// <param name="activity">The trip activity to remove.</param>
        void RemoveTripActivity(TripActivity activity);

        /// <summary>Removes a participant from a trip.</summary>
        /// <param name="participant">The participant to remove.</param>
        void RemoveParticipant(TripParticipant participant);

        /// <summary>Removes a city from a trip.</summary>
        /// <param name="tripId">The trip identifier.</param>
        /// <param name="cityId">The city identifier.</param>
        /// <returns>A task that completes when the city is removed.</returns>
        Task RemoveCityAsync(int tripId, int cityId);

        /// <summary>Removes an activity from a trip.</summary>
        /// <param name="tripId">The trip identifier.</param>
        /// <param name="activityId">The activity identifier.</param>
        /// <returns>A task that completes when the activity is removed.</returns>
        Task RemoveActivityAsync(int tripId, int activityId);

        /// <summary>Removes a participant from a trip.</summary>
        /// <param name="tripId">The trip identifier.</param>
        /// <param name="userId">The user identifier.</param>
        /// <returns>A task that completes when the participant is removed.</returns>
        Task RemoveParticipantAsync(int tripId, int userId);

        #endregion

        #region Validation - Trip Access & Cities

        /// <summary>Retrieves trip access information for a user.</summary>
        /// <param name="tripId">The trip identifier.</param>
        /// <param name="userId">The user identifier.</param>
        /// <returns>The access information, or <see langword="null"/> when the trip does not exist.</returns>
        Task<TripAccessInfoDTO?> GetAccessInfoAsync(int tripId, int userId);

        /// <summary>Checks whether a city exists.</summary>
        /// <param name="cityId">The city identifier.</param>
        /// <returns><see langword="true"/> when the city exists; otherwise, <see langword="false"/>.</returns>
        Task<bool> CityExistsAsync(int cityId);

        #endregion
    }
}
