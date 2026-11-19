
using TravelApp.BusinessLogic.Auth;
using TravelApp.BusinessLogic.DTOs.Reviews;
using TravelApp.BusinessLogic.Exceptions.SpecificCustomException;
using TravelApp.BusinessLogic.Interfaces;

namespace TravelApp.BusinessLogic.Handlers.Reviews.UpdateReview
{
    /// <summary>Handler for updating a review.</summary>
    public class UpdateReviewHandler
    {
        private readonly IUnitOfWorkFactory _unitOfWorkFactory;
        private readonly ICurrentUser _currentUser;

        /// <summary>Initializes a new instance of the UpdateReviewHandler class.</summary>
        public UpdateReviewHandler(IUnitOfWorkFactory unitOfWorkFactory, ICurrentUser currentUser)
        {
            _unitOfWorkFactory = unitOfWorkFactory;
            _currentUser = currentUser;
        }

        /// <summary>Updates an existing review.</summary>
        public async Task UpdateReviewAsync(int reviewId, UpdateReviewRequest request)
        {
            await using IUnitOfWork unitOfWork = _unitOfWorkFactory.Create();

            ReviewAccessInfoDTO? access = await unitOfWork.Reviews.GetAccessInfoAsync(reviewId);

            if (access is null)
            {
                throw new TravelAppNotFoundException($"Review {reviewId} was not found.");
            }

            if (access.UserId != _currentUser.UserId)
            {
                throw new TravelAppForbiddenException("You cannot update this review.");
            }

            if (request.Content is null && request.Stars is null)
            {
                throw new TravelAppValidationException("At least one field must be provided.");
            }

            if (request.Content is not null && string.IsNullOrWhiteSpace(request.Content))
            {
                throw new TravelAppValidationException("Review content cannot be empty.");
            }

            UpdateReviewDTO update = new UpdateReviewDTO
            {
                Content = request.Content?.Trim(),
                Stars = request.Stars
            };

            await unitOfWork.Reviews.UpdateAsync(reviewId, update);
            await unitOfWork.CommitAsync();
        }
    }
}