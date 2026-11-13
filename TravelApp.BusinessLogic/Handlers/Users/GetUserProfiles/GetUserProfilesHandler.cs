using TravelApp.BusinessLogic.Auth;
using TravelApp.BusinessLogic.Interfaces;

namespace TravelApp.BusinessLogic.Handlers.Users
{
    /// <summary>Handles the get user profiles use case.</summary>
    public class GetUserProfilesHandler
    {
        private readonly IUnitOfWorkFactory _unitOfWorkFactory;
        private readonly ICurrentUser _currentUser;

        /// <summary>Initializes a new GetUserProfilesHandler instance.</summary>
        public GetUserProfilesHandler(IUnitOfWorkFactory unitOfWorkFactory, ICurrentUser currentUser)
        {
            _unitOfWorkFactory = unitOfWorkFactory;
            _currentUser = currentUser;
        }

        /// <summary>Retrieves user profiles.</summary>
        public async Task<List<GetUserProfilesResponse>> GetUserProfilesAsync(GetUserProfilesRequest request)
        {
            await using IUnitOfWork unitOfWork = _unitOfWorkFactory.Create();

            List<GetUserProfilesResponse> result = await unitOfWork.Users.GetProfilesAsync(request, _currentUser.UserId, _currentUser.IsAdmin);
            return result;
        }
    }
}
