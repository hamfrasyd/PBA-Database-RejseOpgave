using TravelApp.BusinessLogic.Auth;
using TravelApp.BusinessLogic.DTOs.Reviews;
using TravelApp.BusinessLogic.Exceptions.SpecificCustomException;
using TravelApp.BusinessLogic.Handlers.Trips;
using TravelApp.BusinessLogic.Interfaces;

namespace TravelApp.BusinessLogic.Handlers.Reviews.GetReview
{
    /// <summary>Handler for retrieving a review by its identifier.</summary>
    public class GetReviewHandler
    {
        private readonly IUnitOfWorkFactory _unitOfWorkFactory;
        private readonly ICurrentUser _currentUser;

        /// <summary>Initializes a new instance of the <see cref="GetReviewHandler"/> class.</summary>
        public GetReviewHandler(IUnitOfWorkFactory unitOfWorkFactory, ICurrentUser currentUser)
        {
            _unitOfWorkFactory = unitOfWorkFactory;
            _currentUser = currentUser;
        }

        /// <summary>Retrieves a review by its identifier.</summary>
        public async Task<GetReviewResponse> GetReviewById(int reviewId)
        {
            await using IUnitOfWork unitOfWork = _unitOfWorkFactory.Create();

            ReviewDetailsDTO? review = await unitOfWork.Reviews.GetReviewByIdAsync(reviewId);
            if (review is null)
            {
                throw new TravelAppNotFoundException($"Review {reviewId} was not found.");
            }

            return new GetReviewResponse
            {
                Id = review.Id,
                Content = review.Content,
                Stars = review.Stars,
                TripId = review.TripId,
                UserId = review.UserId,
                FirstName = review.FirstName,
                LastName = review.LastName
            };
        }
    }
}
