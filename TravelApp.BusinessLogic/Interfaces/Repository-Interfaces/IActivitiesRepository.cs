
using TravelApp.BusinessLogic.Handlers.Activities.UpdateActivity;
using TravelApp.BusinessLogic.Models.Destinations;

namespace TravelApp.BusinessLogic.Interfaces
{
    /// <summary>Defines database operations for activities.</summary>
    public interface IActivitiesRepository
    {
        #region Create - Activities

        /// <summary>Adds an activity.</summary>
        Task AddActivityAsync(Activity activity);

        #endregion

        #region Read - Activities

        /// <summary>Retrieves activities, optionally filtered by city.</summary>
        Task<List<Activity>> GetActivitiesAsync(int? cityId);

        /// <summary>Retrieves an activity by ID.</summary>
        Task<Activity?> GetActivityByIdAsync(int activityId);

        #endregion

        #region Update - Activities

        /// <summary>Updates an activity.</summary>
        Task UpdateActivityAsync(int activityId, UpdateActivityRequest request);

        #endregion

        #region Delete - Activities

        /// <summary>Deletes an activity.</summary>
        Task<bool> DeleteActivityAsync(int activityId);

        #endregion

        #region Validation - Activities

        /// <summary>Checks whether an activity belongs to a city.</summary>
        Task<bool> ActivityBelongsToCityAsync(int activityId, int cityId);

        #endregion
    }
}
