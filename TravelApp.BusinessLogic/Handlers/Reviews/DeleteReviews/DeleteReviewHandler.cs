
using TravelApp.BusinessLogic.Auth;
using TravelApp.BusinessLogic.DTOs.Reviews;
using TravelApp.BusinessLogic.Exceptions.SpecificCustomException;
using TravelApp.BusinessLogic.Interfaces;

namespace TravelApp.BusinessLogic.Handlers.Reviews.DeleteReview
{
    /// <summary>Handler for deleting a review.</summary>
    public class DeleteReviewHandler
    {
        private readonly IUnitOfWorkFactory _unitOfWorkFactory;
        private readonly ICurrentUser _currentUser;

        /// <summary>Initializes a new instance of the <see cref="DeleteReviewHandler"/> class.</summary>
        public DeleteReviewHandler(IUnitOfWorkFactory unitOfWorkFactory, ICurrentUser currentUser)
        {
            _unitOfWorkFactory = unitOfWorkFactory;
            _currentUser = currentUser;
        }

        /// <summary>Deletes an existing review.</summary>
        public async Task DeleteReviewAsync(int reviewId)
        {
            await using IUnitOfWork unitOfWork = _unitOfWorkFactory.Create();

            ReviewAccessInfoDTO? access = await unitOfWork.Reviews.GetAccessInfoAsync(reviewId);

            if (access is null)
            {
                throw new TravelAppNotFoundException($"Review {reviewId} was not found.");
            }

            if (access.UserId != _currentUser.UserId && !_currentUser.IsAdmin)
            {
                throw new TravelAppForbiddenException("You cannot delete this review.");
            }

            await unitOfWork.Reviews.DeleteAsync(reviewId);
            await unitOfWork.CommitAsync();
        }
    }
}
