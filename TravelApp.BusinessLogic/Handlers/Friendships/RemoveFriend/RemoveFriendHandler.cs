using TravelApp.BusinessLogic.Auth;
using TravelApp.BusinessLogic.Exceptions.SpecificCustomException;
using TravelApp.BusinessLogic.Interfaces;
using TravelApp.BusinessLogic.Models.Friendships;

namespace TravelApp.BusinessLogic.Handlers.Friendships.RemoveFriend
{
    /// <summary>Handles the remove friend use case.</summary>
    public class RemoveFriendHandler
    {
        private readonly IUnitOfWorkFactory _unitOfWorkFactory;
        private readonly ICurrentUser _currentUser;

        /// <summary>Initializes a new RemoveFriendHandler instance.</summary>
        /// <param name="unitOfWorkFactory">The Unit of Work factory.</param>
        /// <param name="currentUser">The current logged-in user.</param>
        public RemoveFriendHandler(IUnitOfWorkFactory unitOfWorkFactory, ICurrentUser currentUser)
        {
            _unitOfWorkFactory = unitOfWorkFactory;
            _currentUser = currentUser;
        }

        /// <summary>Removes a friendship for either member or an administrator.</summary>
        /// <param name="friendshipId">The friendship identifier.</param>
        /// <returns>A task that completes when the friendship is removed.</returns>
        /// <exception cref="TravelAppNotFoundException">Thrown when the friendship does not exist.</exception>
        /// <exception cref="TravelAppForbiddenException">Thrown when the current user cannot remove the friendship.</exception>
        public async Task RemoveFriendAsync(int friendshipId)
        {
            await using IUnitOfWork unitOfWork = _unitOfWorkFactory.Create();

            Friendship? friendshipFromDb = await unitOfWork.Friendships.GetByIdAsync(friendshipId);
            if (friendshipFromDb is null)
            {
                throw new TravelAppNotFoundException($"Friendship with ID {friendshipId} was not found.");
            }

            AccessRules.RequireCurrentUserIsFriendOrAdmin(_currentUser, friendshipFromDb.UserA!.Id, friendshipFromDb.UserB!.Id);

            bool friendshipWasFound = await unitOfWork.Friendships.DeleteAsync(friendshipId);
            if (friendshipWasFound is false)
            {
                throw new TravelAppNotFoundException($"Friendship with ID {friendshipId} was not found.");
            }

            await unitOfWork.CommitAsync();
        }
    }
}
