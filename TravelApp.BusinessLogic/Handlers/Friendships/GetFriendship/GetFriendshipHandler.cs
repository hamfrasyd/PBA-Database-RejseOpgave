using TravelApp.BusinessLogic.Auth;
using TravelApp.BusinessLogic.Exceptions.SpecificCustomException;
using TravelApp.BusinessLogic.Interfaces;
using TravelApp.BusinessLogic.Models.Friendships;
using TravelApp.BusinessLogic.Models.Users;

namespace TravelApp.BusinessLogic.Handlers.Friendships.GetFriendship
{
    /// <summary>Handles the get friendship use case.</summary>
    public class GetFriendshipHandler
    {
        private readonly IUnitOfWorkFactory _unitOfWorkFactory;
        private readonly ICurrentUser _currentUser;

        /// <summary>Initializes a new GetFriendshipHandler instance.</summary>
        /// <param name="unitOfWorkFactory">The Unit of Work factory.</param>
        /// <param name="currentUser">The current logged-in user.</param>
        public GetFriendshipHandler(IUnitOfWorkFactory unitOfWorkFactory, ICurrentUser currentUser)
        {
            _unitOfWorkFactory = unitOfWorkFactory;
            _currentUser = currentUser;
        }

        /// <summary>Retrieves a friendship for either member or an administrator.</summary>
        /// <param name="friendshipId">The friendship identifier.</param>
        /// <returns>The friendship details.</returns>
        /// <exception cref="TravelAppNotFoundException">Thrown when the friendship does not exist or one of its users is missing.</exception>
        /// <exception cref="TravelAppForbiddenException">Thrown when the current user cannot view the friendship.</exception>
        public async Task<GetFriendshipResponse> GetFriendshipAsync(int friendshipId)
        {
            await using IUnitOfWork unitOfWork = _unitOfWorkFactory.Create();

            Friendship? friendshipFromDb = await unitOfWork.Friendships.GetByIdAsync(friendshipId);
            if (friendshipFromDb is null)
            {
                throw new TravelAppNotFoundException($"Friendship with ID {friendshipId} was not found.");
            }

            TravelUser? firstUser = friendshipFromDb.UserA;
            if (firstUser is null)
            {
                throw new TravelAppNotFoundException($"Friendship with ID {friendshipId} was not found.");
            }

            TravelUser? secondUser = friendshipFromDb.UserB;
            if (secondUser is null)
            {
                throw new TravelAppNotFoundException($"Friendship with ID {friendshipId} was not found.");
            }

            int firstUserId = firstUser.Id;
            int secondUserId = secondUser.Id;
            AccessRules.RequireCurrentUserIsFriendOrAdmin(_currentUser, firstUserId, secondUserId);

            GetFriendshipResponse response = new GetFriendshipResponse
            {
                Id = friendshipFromDb.Id,
                FirstUserId = firstUserId,
                SecondUserId = secondUserId,
                StartDate = friendshipFromDb.StartDate
            };

            return response;
        }
    }
}
