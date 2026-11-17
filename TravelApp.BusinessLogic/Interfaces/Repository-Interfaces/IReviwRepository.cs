using System;
using System.Collections.Generic;
using System.Text;
using TravelApp.BusinessLogic.DTOs.Activities;
using TravelApp.BusinessLogic.Models.Reviews;

namespace TravelApp.BusinessLogic.Interfaces.Repository_Interfaces
{
    /// <summary>Defines database operations for reviews.</summary>
    public interface IReviwRepository
    {
        #region Read All
        /// <summary>Retrieves all reviews, optionally filtered by city.</summary>
        Task<List<Review>> GetAllReviewsAsync();
        #endregion

        #region Read By Id
        Task<ActivityDto?> GetActivityByIdAsync(int activityId);

        #endregion
    }
}
