using TravelApp.BusinessLogic.DTOs.Reviews;
using TravelApp.BusinessLogic.Models.Reviews;

namespace TravelApp.BusinessLogic.Interfaces.Repository_Interfaces
{
    /// <summary>Defines database operations for reviews.</summary>
    public interface IReviewRepository
    {
        #region Create
        /// <summary>Creates a review for a trip and user.</summary>
        Task CreateReviewForUserAsync(Review review, int tripId, int userId);
        #endregion

        #region Read All
        /// <summary>Retrieves all reviews for a trip, optionally filtered by star rating.</summary>
        Task<List<ReviewSummaryDTO>> GetAllReviewsAsync(int tripId, byte? stars);
        #endregion

        #region Read By Id
        /// <summary>Retrieves a review by its unique identifier.</summary>
        Task<ReviewDetailsDTO?> GetReviewByIdAsync(int reviewId);
        #endregion
    }
}
