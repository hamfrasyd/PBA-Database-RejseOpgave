using TravelApp.BusinessLogic.Handlers.Activities.UpdateActivity;
using TravelApp.BusinessLogic.Models.Destinations;

namespace TravelApp.BusinessLogic.Interfaces
{
    /// <summary>Defines database operations for activities.</summary>
    public interface IActivitiesRepository
    {
        #region Create - Activities

        /// <summary>Adds an activity.</summary>
        /// <param name="activity">The activity to add.</param>
        /// <returns>A task that completes when the activity is added.</returns>
        Task AddActivityAsync(Activity activity);

        #endregion

        #region Read - All Activities

        /// <summary>Retrieves activities, optionally filtered by city.</summary>
        /// <param name="cityId">The city identifier to filter by, or <see langword="null"/> for all cities.</param>
        /// <returns>The matching activities.</returns>
        Task<List<Activity>> GetActivitiesAsync(int? cityId);

        #endregion

        #region Read - By-ID Activities

        /// <summary>Retrieves an activity by ID.</summary>
        /// <param name="activityId">The activity identifier.</param>
        /// <returns>The activity, or <see langword="null"/> when it does not exist.</returns>
        Task<Activity?> GetActivityByIdAsync(int activityId);

        #endregion

        #region Update - Activities

        /// <summary>Updates an activity.</summary>
        /// <param name="activityId">The activity identifier.</param>
        /// <param name="request">The updated activity details.</param>
        /// <returns>A task that completes when the activity is updated.</returns>
        Task UpdateActivityAsync(int activityId, UpdateActivityRequest request);

        #endregion

        #region Delete - Activities

        /// <summary>Deletes an activity.</summary>
        /// <param name="activityId">The activity identifier.</param>
        /// <returns><see langword="true"/> when the activity was deleted; otherwise, <see langword="false"/>.</returns>
        Task<bool> DeleteActivityAsync(int activityId);

        #endregion

        #region Validation - Activities

        /// <summary>Checks whether an activity belongs to a city.</summary>
        /// <param name="activityId">The activity identifier.</param>
        /// <param name="cityId">The city identifier.</param>
        /// <returns><see langword="true"/> when the activity belongs to the city; otherwise, <see langword="false"/>.</returns>
        Task<bool> ActivityBelongsToCityAsync(int activityId, int cityId);

        #endregion
    }
}
