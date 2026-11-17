using TravelApp.BusinessLogic.Auth;
using TravelApp.BusinessLogic.Interfaces;

namespace TravelApp.BusinessLogic.Handlers.Users.GetUserProfiles
{
    /// <summary>Handles the get user profiles use case.</summary>
    public class GetUserProfilesHandler
    {
        private readonly IUnitOfWorkFactory _unitOfWorkFactory;
        private readonly ICurrentUser _currentUser;

        /// <summary>Initializes a new GetUserProfilesHandler instance.</summary>
        /// <param name="unitOfWorkFactory">The unit of work factory.</param>
        /// <param name="currentUser">The current logged-in user.</param>
        public GetUserProfilesHandler(IUnitOfWorkFactory unitOfWorkFactory, ICurrentUser currentUser)
        {
            _unitOfWorkFactory = unitOfWorkFactory;
            _currentUser = currentUser;
        }

        /// <summary>Retrieves user profiles.</summary>
        /// <param name="request">The filter and sort options.</param>
        /// <returns>The matching user profiles.</returns>
        public async Task<List<GetUserProfilesResponse>> GetUserProfilesAsync(GetUserProfilesRequest request)
        {
            await using IUnitOfWork unitOfWork = _unitOfWorkFactory.Create();

            List<GetUserProfilesResponse> result = await unitOfWork.Users.GetProfilesAsync(request, _currentUser.UserId, _currentUser.IsAdmin);
            return result;
        }
    }
}
