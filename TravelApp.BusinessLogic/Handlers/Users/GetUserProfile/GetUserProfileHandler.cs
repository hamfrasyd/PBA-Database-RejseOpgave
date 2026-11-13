using TravelApp.BusinessLogic.Auth;
using TravelApp.BusinessLogic.Exceptions.SpecificCustomException;
using TravelApp.BusinessLogic.Interfaces;

namespace TravelApp.BusinessLogic.Handlers.Users
{
    /// <summary>Handles the get user profile use case.</summary>
    public class GetUserProfileHandler
    {
        private readonly IUnitOfWorkFactory _unitOfWorkFactory;
        private readonly ICurrentUser _currentUser;

        /// <summary>Initializes a new GetUserProfileHandler instance.</summary>
        public GetUserProfileHandler(IUnitOfWorkFactory unitOfWorkFactory, ICurrentUser currentUser)
        {
            _unitOfWorkFactory = unitOfWorkFactory;
            _currentUser = currentUser;
        }

        /// <summary>Retrieves user profile.</summary>
        public async Task<GetUserProfileResponse> GetUserProfileAsync(int userId)
        {
            await using IUnitOfWork unitOfWork = _unitOfWorkFactory.Create();

            GetUserProfileResponse? profile = await unitOfWork.Users.GetProfileAsync(userId);
            if (profile is null || (profile.IsPrivate && _currentUser.IsAdmin is false && _currentUser.UserId != userId))
            {
                throw new TravelAppNotFoundException($"User {userId} was not found.");
            }

            return profile;
        }
    }
}
