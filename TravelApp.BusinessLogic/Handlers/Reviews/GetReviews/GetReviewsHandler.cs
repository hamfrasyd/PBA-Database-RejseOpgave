
using TravelApp.BusinessLogic.Auth;
using TravelApp.BusinessLogic.DTOs.Reviews;
using TravelApp.BusinessLogic.Exceptions.SpecificCustomException;
using TravelApp.BusinessLogic.Handlers.Reviews.GetReviews;
using TravelApp.BusinessLogic.Handlers.Trips;
using TravelApp.BusinessLogic.Interfaces;

namespace TravelApp.BusinessLogic.Handlers.Reviews
{
    /// <summary>Handler for retrieving all reviews for a trip.</summary>
    public class GetReviewsHandler
    {
        private readonly IUnitOfWorkFactory _unitOfWorkFactory;
        private readonly ICurrentUser _currentUser;

        /// <summary>Initializes a new instance of the <see cref="GetReviewsHandler"/> class.</summary>
        public GetReviewsHandler(IUnitOfWorkFactory unitOfWorkFactory, ICurrentUser currentUser)
        {
            _unitOfWorkFactory = unitOfWorkFactory;
            _currentUser = currentUser;
        }

        /// <summary>Retrieves reviews for a trip with optional filtering and sorting.</summary>
        public async Task<List<GetReviewsResponse>> GetAllReviewsAsync(int tripId, byte? stars)
        {
            if (stars.HasValue && (stars.Value < 1 || stars.Value > 5))
            {
                throw new TravelAppValidationException("Stars must be between 1 and 5.");
            }

            await using IUnitOfWork unitOfWork = _unitOfWorkFactory.Create();

            List<ReviewSummaryDTO> reviews = await unitOfWork.Reviews.GetAllReviewsAsync(tripId, stars);
            return reviews.Select(review => new GetReviewsResponse
            {
                Id = review.Id,
                Content = review.Content,
                Stars = review.Stars,
                UserId = review.UserId,
                FirstName = review.FirstName,
                LastName = review.LastName
            }).ToList();
        }
    }
}
