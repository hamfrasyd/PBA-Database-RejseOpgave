using TravelApp.BusinessLogic.DTOs.Activities;

namespace TravelApp.BusinessLogic.Interfaces
{
    /// <summary>Defines database operations for activities.</summary>
    public interface IActivitiesRepository
    {
        #region Create - Activities

        /// <summary>Stages an activity for insertion. Returns the activity when the provider already has an id; otherwise <see langword="null"/>.</summary>
        /// <param name="activityToCreate">The activity values to persist.</param>
        /// <returns>The created activity when known immediately; otherwise <see langword="null"/>.</returns>
        Task<ActivityDTO?> CreateActivityAsync(CreateActivityDTO activityToCreate);

        /// <summary>Returns the activity staged by <see cref="CreateActivityAsync"/> after the unit of work has committed.</summary>
        /// <returns>The created activity, including its generated identifier when assigned at commit.</returns>
        ActivityDTO GetCreatedActivity();

        #endregion

        #region Read - All Activities

        /// <summary>Retrieves activities, optionally filtered by city.</summary>
        /// <param name="cityId">The city identifier to filter by, or <see langword="null"/> for all cities.</param>
        /// <returns>The matching activities.</returns>
        Task<List<ActivityDTO>> GetActivitiesAsync(int? cityId);

        #endregion

        #region Read - By-ID Activities

        /// <summary>Retrieves an activity by ID.</summary>
        /// <param name="activityId">The activity identifier.</param>
        /// <returns>The activity, or <see langword="null"/> when it does not exist.</returns>
        Task<ActivityDTO?> GetActivityByIdAsync(int activityId);

        #endregion

        #region Update - Activities

        /// <summary>Updates an activity.</summary>
        /// <param name="activityToUpdate">The activity values to persist.</param>
        /// <returns>The updated activity, or <see langword="null"/> when the activity does not exist.</returns>
        Task<ActivityDTO?> UpdateActivityAsync(UpdateActivityDTO activityToUpdate);

        #endregion

        #region Delete - Activities

        /// <summary>Deletes an activity.</summary>
        /// <param name="activityId">The activity identifier.</param>
        /// <returns>A task that completes when the activity is marked for deletion.</returns>
        Task DeleteAsync(int activityId);

        #endregion
    }
}