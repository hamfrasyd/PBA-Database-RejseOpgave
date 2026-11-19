using TravelApp.BusinessLogic.DTOs.Activities;
using TravelApp.BusinessLogic.Models.Activities;

namespace TravelApp.BusinessLogic.Interfaces
{
    /// <summary>Defines database operations for activities.</summary>
    public interface IActivitiesRepository
    {
        #region Create - Activities

        /// <summary>Stages an activity for insertion in the given city.</summary>
        /// <param name="activity">The activity to add.</param>
        /// <param name="cityId">The city identifier.</param>
        /// <returns>A task that completes when the activity is staged. The generated identifier is available on <paramref name="activity"/> after the unit of work commits.</returns>
        Task CreateActivityAsync(Activity activity, int cityId);

        #endregion

        #region Read - All Activities

        /// <summary>Retrieves activities, optionally filtered by city.</summary>
        /// <param name="cityId">The city identifier to filter by, or null for all cities.</param>
        /// <returns>The matching activities.</returns>
        Task<List<ActivityDTO>> GetActivitiesAsync(int? cityId);

        #endregion

        #region Read - By-ID Activities

        /// <summary>Retrieves an activity by ID.</summary>
        /// <param name="activityId">The activity identifier.</param>
        /// <returns>The activity, or null when it does not exist.</returns>
        Task<ActivityDTO?> GetActivityByIdAsync(int activityId);

        #endregion

        #region Update - Activities

        /// <summary>Updates an activity.</summary>
        /// <param name="activityToUpdate">The activity values to persist.</param>
        /// <returns>true when the activity exists and is marked for update; otherwise, false.</returns>
        Task<bool> UpdateActivityAsync(UpdateActivityDTO activityToUpdate);

        #endregion

        #region Delete - Activities

        /// <summary>Deletes an activity.</summary>
        /// <param name="activityId">The activity identifier.</param>
        /// <returns>A task that completes when the activity is marked for deletion.</returns>
        Task DeleteAsync(int activityId);

        #endregion
    }
}