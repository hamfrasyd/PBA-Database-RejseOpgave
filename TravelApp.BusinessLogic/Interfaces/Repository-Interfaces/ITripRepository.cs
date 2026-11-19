using Neo4j.Driver;
using TravelApp.BusinessLogic.DTOs.Pagination;
using TravelApp.BusinessLogic.DTOs.Trips;
using TravelApp.BusinessLogic.Models.Trips;

namespace TravelApp.BusinessLogic.Interfaces
{
    /// <summary>Defines database operations for trips.</summary>
    public interface ITripRepository
    {
        #region Create
        /// <summary>Adds trip for user.</summary>
        Task CreateTripForUserAsync(Trip trip, int userId);
        #endregion

        #region Read All
        /// <summary>Retrieves paginated trip summaries across all users.</summary>
        Task<PagedResultDTO<TripSummaryDTO>> GetAllSummariesAsync(int? countryId, int? continentId, bool? isCompleted, int pageNumber, int pageSize);
        #endregion

        #region Read all for user
        /// <summary>Retrieves trip summaries for user with optional filtering by country and continent.</summary>
        Task<PagedResultDTO<TripSummaryDTO>> GetSummariesByUserAsync(int userId, int? countryId, int? continentId, int pageNumber, int pageSize);
        #endregion

        #region Read By Id
        /// <summary>Retrieves trip details by id.</summary>
        Task<TripDetailsDTO?> GetDetailsAsync(int tripId);
        #endregion

        #region Update
        /// <summary>Updates trip.</summary>
        Task UpdateAsync(int tripId, UpdateTripDTO update);
        #endregion

        #region Delete
        /// <summary>Deletes trip.</summary>
        Task DeleteAsync(int tripId);
        #endregion

        #region Stops in trip
        /// <summary>Adds city to a trip.</summary>
        Task AddCityToTripAsync(int tripId, int cityId);

        /// <summary>Removes City from trip</summary>
        Task RemoveCityFromTripAsync(int tripId, int cityId);
        #endregion

        #region Activities in trip
        /// <summary>Retrieves the activities associated with a specific trip.</summary>
        Task<List<TripActivityDTO>> GetActivitiesnTripAsync(int tripId);

        /// <summary>Adds activity to a trip.</summary>
        Task AddActivityToTripAsync(int tripId, int activityId);

        /// <summary>Removes activity from a trip.</summary>
        Task RemoveActivityFromTripAsync(int tripId, int activityId);
        #endregion

        #region Participants in trip
        /// <summary>Retrieves participants for a trip.</summary>
        Task<List<TripParticipantDTO>> GetParticipantsInTripAsync(int tripId);

        /// <summary>adds user as a participant to a trip.</summary>
        Task AddParticipantToTripAsync(int tripId, int userId);

        /// <summary>Removes user as a participant from a trip.</summary>
        Task RemoveParticipantFromTripAsync(int tripId, int userId);
        #endregion

        #region Access and relations
        /// <summary>Retrieves access information for a trip and user.</summary>
        Task<TripAccessInfoDTO?> GetAccessInfoAsync(int tripId, int userId);

        /// <summary>Retrieves trip relations for trip.</summary>
        Task<TripRelationsDTO?> GetRelationsAsync(int tripId);
        #endregion
    }
}
