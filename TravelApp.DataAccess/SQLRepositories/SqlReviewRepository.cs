using Microsoft.EntityFrameworkCore;
using TravelApp.BusinessLogic.DTOs.Reviews;
using TravelApp.BusinessLogic.Interfaces.Repository_Interfaces;
using TravelApp.BusinessLogic.Models.Reviews;
using TravelApp.BusinessLogic.Models.Trips;
using TravelApp.BusinessLogic.Models.Users;
using TravelApp.DataAccess.SQLRepositories.EntityFramework;

namespace TravelApp.DataAccess.SQLRepositories
{
    /// <summary>SQL Server implementation of the review repository.</summary>
    public class SqlReviewRepository : IReviewRepository
    {
        private readonly TravelAppContext _context;

        /// <summary>Initializes a new instance of the <see cref="SqlReviewRepository"/> class.</summary>
        public SqlReviewRepository(TravelAppContext context)
        {
            _context = context;
        }

        #region Create
        /// <summary>Creates a review for a trip and user.</summary>
        public async Task CreateReviewForUserAsync(Review review, int tripId, int userId)
        {
            Trip trip = await _context.TripEntities.FindAsync(tripId)
                ?? throw new InvalidOperationException($"Trip {tripId} was not found.");

            TravelUser user = await _context.TravelUserEntities.FindAsync(userId)
                ?? throw new InvalidOperationException( $"User {userId} was not found.");

            review.Trip = trip;
            review.User = user;

            await _context.ReviewEntities.AddAsync(review);
        }

        #endregion

        #region Read All
        /// <summary>Retrieves all reviews for a trip, optionally filtered by star rating.</summary>
        public async Task<List<ReviewSummaryDTO>> GetAllReviewsAsync(int tripId, byte? stars)
        {
            IQueryable<Review> reviews = _context.ReviewEntities.AsNoTracking()
                .Where(review => review.Trip!.Id == tripId && review.Trip.IsCompleted);
            if (stars.HasValue)
            {
                reviews = reviews.Where(review =>review.Stars == stars.Value);
            }

            List<ReviewSummaryDTO> result = await reviews
                .OrderByDescending(review => review.Stars)
                .Select(review => new ReviewSummaryDTO
                {
                    Id = review.Id,
                    Content = review.Content,
                    Stars = review.Stars,

                    UserId = review.User!.Id,
                    FirstName = review.User.FirstName,
                    LastName = review.User.LastName
                })
                .ToListAsync();

            return result;
        }
        #endregion

        #region Read By Id
        /// <summary>Retrieves a review by its unique identifier.</summary>
        public async Task<ReviewDetailsDTO?> GetReviewByIdAsync(int reviewId)
        {
            ReviewDetailsDTO? review = await _context.ReviewEntities
                .AsNoTracking().Where(review => review.Id == reviewId && review.Trip!.IsCompleted)
                .Select(review => new ReviewDetailsDTO
                {
                    Id = review.Id,
                    Content = review.Content,
                    Stars = review.Stars,
                    TripId = review.Trip!.Id,

                    UserId = review.User!.Id,
                    FirstName = review.User.FirstName,
                    LastName = review.User.LastName
                })
                .FirstOrDefaultAsync();

            return review;
        }
        #endregion

        #region Update
        /// <summary>Updates the content and rating of a review.</summary>
        public async Task UpdateAsync(int reviewId, UpdateReviewDTO update)
        {
            Review? review = await _context.ReviewEntities.FirstOrDefaultAsync(review => review.Id == reviewId);

            if (review is null)
            {
                throw new InvalidOperationException($"Review {reviewId} was not found.");
            }
            if (update.Content is not null)
            {
                review.Content = update.Content;
            }
            if (update.Stars is not null)
            {
                review.Stars = update.Stars.Value;
            }
        }

        /// <summary>Retrieves the author ID of a review.</summary>
        public async Task<int?> GetReviewAuthorIdAsync(int reviewId)
        {
            return await _context.ReviewEntities
                .AsNoTracking().Where(review => review.Id == reviewId).Select(review => (int?)review.User!.Id)
                .FirstOrDefaultAsync();
        }
        #endregion

        #region Delete
        /// <summary>Deletes a review by its identifier.</summary>
        public async Task DeleteAsync(int reviewId)
        {
            Review? review = await _context.ReviewEntities.FirstOrDefaultAsync(review => review.Id == reviewId);

            if (review is null)
            {
                throw new InvalidOperationException($"Review {reviewId} was not found.");
            }

            _context.ReviewEntities.Remove(review);
        }
        #endregion
    }
}
