using System.Text;
using TravelApp.BusinessLogic.DTOs.Activities;
using TravelApp.BusinessLogic.DTOs.Pagination;
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
        /// <param name="cityId">The city identifier to filter by, or <see langword="null"/> for all cities.</param>
        /// <param name="pageNumber">The page number for pagination, starting from 1.</param>
        /// <param name="pageSize">The number of items per page.</param>
        /// <returns>The matching activities.</returns>
        Task<PagedResultDTO<ActivityDTO>> GetActivitiesAsync(int? cityId, int pageNumber, int pageSize);

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
        /// <returns><see langword="true"/> when the activity exists and is marked for update; otherwise, <see langword="false"/>.</returns>
        Task<bool> UpdateActivityAsync(ActivityDTO activityToUpdate);

        #endregion

        #region Delete - Activities

        /// <summary>Deletes an activity.</summary>
        /// <param name="activityId">The activity identifier.</param>
        /// <returns>A task that completes when the activity is marked for deletion.</returns>
        Task DeleteAsync(int activityId);

        #endregion
    }
}