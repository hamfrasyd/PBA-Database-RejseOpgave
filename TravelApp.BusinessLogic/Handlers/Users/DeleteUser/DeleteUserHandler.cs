using TravelApp.BusinessLogic.Exceptions.SpecificCustomException;
using TravelApp.BusinessLogic.Interfaces;
using TravelApp.BusinessLogic.Auth;

namespace TravelApp.BusinessLogic.Handlers.Users.DeleteUser
{
    /// <summary>Handles the delete user use case.</summary>
    public class DeleteUserHandler
    {
        private readonly IUnitOfWorkFactory _unitOfWorkFactory;
        private readonly ICurrentUser _currentUser;

        /// <summary>Initializes a new DeleteUserHandler instance.</summary>
        /// <param name="unitOfWorkFactory">The unit of work factory.</param>
        /// <param name="currentUser">The current logged-in user.</param>
        public DeleteUserHandler(IUnitOfWorkFactory unitOfWorkFactory, ICurrentUser currentUser)
        {
            _unitOfWorkFactory = unitOfWorkFactory;
            _currentUser = currentUser;
        }

        /// <summary>Deletes the user with the specified identifier.</summary>
        /// <param name="userId">The user identifier.</param>
        /// <exception cref="TravelAppForbiddenException">Thrown when the current user cannot delete the user.</exception>
        /// <exception cref="TravelAppNotFoundException">Thrown when the user does not exist.</exception>
        public async Task DeleteUserAsync(int userId)
        {
            AccessRules.RequireCurrentUserIsOwnerOrAdmin(_currentUser, userId);

            await using IUnitOfWork unitOfWork = _unitOfWorkFactory.Create();

            await unitOfWork.Users.DeleteAsync(userId);
            await unitOfWork.CommitAsync();
        }

        public async Task DeleteReviewAsync(int reviewId)
        {
            await using IUnitOfWork unitOfWork = _unitOfWorkFactory.Create();

            int? ownerOfReview = await unitOfWork.Reviews.GetAccessInfoAsync(reviewId);

            if (ownerOfReview is null)
            {
                throw new TravelAppNotFoundException($"Review {reviewId} was not found.");
            }

            if (ownerOfReview.UserId != _currentUser.UserId && !_currentUser.IsAdmin)
            {
                throw new TravelAppForbiddenException("You cannot delete this review.");
            }

            await unitOfWork.Reviews.DeleteAsync(reviewId);
            await unitOfWork.CommitAsync();
        }
    }
}
