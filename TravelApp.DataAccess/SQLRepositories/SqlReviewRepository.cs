using System;
using System.Collections.Generic;
using System.Text;
using TravelApp.BusinessLogic.DTOs.Activities;
using TravelApp.BusinessLogic.Interfaces.Repository_Interfaces;
using TravelApp.BusinessLogic.Models.Reviews;

namespace TravelApp.DataAccess.SQLRepositories
{
    /// <summary>SQL Server implementation of the review repository.</summary>
    public class SqlReviewRepository : IReviwRepository
    {
        public Task<ActivityDto?> GetActivityByIdAsync(int activityId)
        {
            throw new NotImplementedException();
        }


        #region Read All

        /// <summary>Retrieves all reviews, optionally filtered by city.</summary>
        public Task<List<Review>> GetAllReviewsAsync()
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}
