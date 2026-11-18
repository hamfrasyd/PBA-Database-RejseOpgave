using TravelApp.BusinessLogic.Auth;
using TravelApp.BusinessLogic.Exceptions.SpecificCustomException;
using TravelApp.BusinessLogic.Interfaces;
using TravelApp.BusinessLogic.Models.Friendships;

namespace TravelApp.BusinessLogic.Handlers.Friendships.AddFriend
{
    /// <summary>Handles the add friend use case.</summary>
    public class AddFriendHandler
    {
        private readonly IUnitOfWorkFactory _unitOfWorkFactory;
        private readonly ICurrentUser _currentUser;

        /// <summary>Initializes a new AddFriendHandler instance.</summary>
        /// <param name="unitOfWorkFactory">The Unit of Work factory.</param>
        /// <param name="currentUser">The current logged-in user.</param>
        public AddFriendHandler(IUnitOfWorkFactory unitOfWorkFactory, ICurrentUser currentUser)
        {
            _unitOfWorkFactory = unitOfWorkFactory;
            _currentUser = currentUser;
        }

        /// <summary>Adds a friendship between the current user and another user.</summary>
        /// <param name="friendUserId">The other user's identifier.</param>
        /// <returns>The created friendship.</returns>
        /// <exception cref="TravelAppValidationException">Thrown when the user tries to add themselves or the other user does not exist.</exception>
        /// <exception cref="TravelAppConflictException">Thrown when the two users are already friends.</exception>
        public async Task<AddFriendResponse> AddFriendAsync(int friendUserId)
        {
            int currentUserId = _currentUser.UserId;
            if (friendUserId == currentUserId)
            {
                throw new TravelAppValidationException("You cannot add yourself as a friend.");
            }

            await using IUnitOfWork unitOfWork = _unitOfWorkFactory.Create();

            bool userExists = await unitOfWork.Users.UserExistsAsync(friendUserId);
            if (userExists is false)
            {
                throw new TravelAppValidationException($"User with ID {friendUserId} does not exist.");
            }

            bool friendshipExists = await unitOfWork.Friendships.FriendshipExistsAsync(currentUserId, friendUserId);
            if (friendshipExists is true)
            {
                throw new TravelAppConflictException("These users are already friends.");
            }

            Friendship friendship = new Friendship
            {
                StartDate = DateTime.UtcNow
            };

            await unitOfWork.Friendships.AddAsync(friendship, currentUserId, friendUserId);
            await unitOfWork.CommitAsync();

            AddFriendResponse response = new AddFriendResponse
            {
                Id = friendship.Id,
                UserId = currentUserId,
                FriendUserId = friendUserId,
                StartDate = friendship.StartDate
            };

            return response;
        }
    }
}
