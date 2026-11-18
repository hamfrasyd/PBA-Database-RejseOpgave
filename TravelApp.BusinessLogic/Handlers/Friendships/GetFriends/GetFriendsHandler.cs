using TravelApp.BusinessLogic.Auth;
using TravelApp.BusinessLogic.DTOs.Friendships;
using TravelApp.BusinessLogic.Exceptions.SpecificCustomException;
using TravelApp.BusinessLogic.Interfaces;
using TravelApp.BusinessLogic.Models.Users;

namespace TravelApp.BusinessLogic.Handlers.Friendships.GetFriends
{
    /// <summary>Handles the get friends use case.</summary>
    public class GetFriendsHandler
    {
        private readonly IUnitOfWorkFactory _unitOfWorkFactory;
        private readonly ICurrentUser _currentUser;

        /// <summary>Initializes a new GetFriendsHandler instance.</summary>
        /// <param name="unitOfWorkFactory">The Unit of Work factory.</param>
        /// <param name="currentUser">The current logged-in user.</param>
        public GetFriendsHandler(IUnitOfWorkFactory unitOfWorkFactory, ICurrentUser currentUser)
        {
            _unitOfWorkFactory = unitOfWorkFactory;
            _currentUser = currentUser;
        }

        /// <summary>Retrieves the friends visible to the current user.</summary>
        /// <param name="userId">The user whose friendships to retrieve.</param>
        /// <returns>The user's friends.</returns>
        /// <exception cref="TravelAppNotFoundException">Thrown when the user does not exist.</exception>
        /// <exception cref="TravelAppForbiddenException">Thrown when the user profile is private.</exception>
        public async Task<List<GetFriendsResponse>> GetFriendsAsync(int userId)
        {
            await using IUnitOfWork unitOfWork = _unitOfWorkFactory.Create();

            TravelUser? userFromDb = await unitOfWork.Users.GetByIdAsync(userId);
            if (userFromDb is null)
            {
                throw new TravelAppNotFoundException($"User with ID {userId} was not found.");
            }

            AccessRules.RequireCurrentUserIsOwnerOrAdminWhenProfileIsPrivate(_currentUser, userFromDb.Id, userFromDb.IsPrivate);

            bool canSeePrivateFriends = _currentUser.UserId == userId || _currentUser.IsAdmin;
            List<FriendDTO> friendsFromDb = await unitOfWork.Friendships.GetFriendsAsync(userId, _currentUser.UserId, canSeePrivateFriends);

            List<GetFriendsResponse> response = friendsFromDb.Select(friend => new GetFriendsResponse
            {
                FriendshipId = friend.FriendshipId,
                UserId = friend.UserId,
                FirstName = friend.FirstName,
                LastName = friend.LastName,
                StartDate = friend.StartDate
            }).ToList();

            return response;
        }
    }
}
