
using TravelApp.BusinessLogic.Auth;
using TravelApp.BusinessLogic.DTOs.Trips;
using TravelApp.BusinessLogic.Exceptions.SpecificCustomException;
using TravelApp.BusinessLogic.Interfaces;
using TravelApp.BusinessLogic.Models.Reviews;
using TravelApp.BusinessLogic.Models.Users;

namespace TravelApp.BusinessLogic.Handlers.Reviews.CreateReview
{
    /// <summary>Handler for creating a review.</summary>
    public class CreateReviewHandler
    {
        private readonly IUnitOfWorkFactory _unitOfWorkFactory;
        private readonly ICurrentUser _currentUser;

        /// <summary>Initializes a new instance of the <see cref="CreateReviewHandler"/> class.</summary>
        public CreateReviewHandler(IUnitOfWorkFactory unitOfWorkFactory, ICurrentUser currentUser)
        {
            _unitOfWorkFactory = unitOfWorkFactory;
            _currentUser = currentUser;
        }

        /// <summary>Creates a review for a trip.</summary>
        public async Task<CreateReviewResponse> CreateReviewAsync(int tripId, CreateReviewRequest request)
        {
            await using IUnitOfWork unitOfWork = _unitOfWorkFactory.Create();

            TripAccessInfoDTO? access = await unitOfWork.Trips.GetAccessInfoAsync(tripId, _currentUser.UserId);
            if (access is null)
            {
                throw new TravelAppNotFoundException($"Trip {tripId} was not found.");
            }

            if (!access.IsParticipant)
            {
                throw new TravelAppForbiddenException("You must be a trip participant to write a review.");
            }

            Review review = new Review
            {
                Content = request.Content.Trim(),
                Stars = request.Stars
            };

            await unitOfWork.Reviews.CreateReviewForUserAsync(review, tripId, _currentUser.UserId);

            await unitOfWork.CommitAsync();

            return new CreateReviewResponse
            {
                Id = review.Id,
                Content = review.Content,
                Stars = review.Stars,
                TripId = tripId,
                UserId = _currentUser.UserId
            };
        }
    }
}
