using TravelApp.BusinessLogic.Models;
using TravelApp.BusinessLogic.Exceptions.SpecificCustomException;
using TravelApp.BusinessLogic.Interfaces;
using TravelApp.BusinessLogic.Auth;

namespace TravelApp.BusinessLogic.Handlers.Users
{
    /// <summary>Handles the delete user use case.</summary>
    public class DeleteUserHandler
    {
        private readonly IUnitOfWorkFactory _unitOfWorkFactory;
        private readonly ICurrentUser _currentUser;

        /// <summary>Initializes a new DeleteUserHandler instance.</summary>
        public DeleteUserHandler(IUnitOfWorkFactory unitOfWorkFactory, ICurrentUser currentUser)
        {
            _unitOfWorkFactory = unitOfWorkFactory;
            _currentUser = currentUser;
        }

        /// <summary>Deletes user.</summary>
        public async Task DeleteUserAsync(int userId)
        {
            AccessRules.RequireOwnerOrAdmin(_currentUser, userId);
            await using IUnitOfWork unitOfWork = _unitOfWorkFactory.Create();

            bool wasDeleted = await unitOfWork.Users.DeleteAsync(userId);

            if (wasDeleted is false)
            {
                throw new TravelAppNotFoundException($"User {userId} was not found.");
            }

            await unitOfWork.CommitAsync();
        }
    }
}
