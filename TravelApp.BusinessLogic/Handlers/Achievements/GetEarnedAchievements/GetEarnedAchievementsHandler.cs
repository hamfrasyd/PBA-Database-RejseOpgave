using TravelApp.BusinessLogic.Exceptions.SpecificCustomException;
using TravelApp.BusinessLogic.Interfaces;
using TravelApp.BusinessLogic.Auth;
using TravelApp.BusinessLogic.Handlers.Users.GetUserProfile;

namespace TravelApp.BusinessLogic.Handlers.Achievements.GetEarnedAchievements
{
    /// <summary>Handles the get earned achievements use case.</summary>
    public class GetEarnedAchievementsHandler
    {
        private readonly IUnitOfWorkFactory _unitOfWorkFactory;
        private readonly ICurrentUser _currentUser;
        /// <summary>Initializes a new GetEarnedAchievementsHandler instance.</summary>
        public GetEarnedAchievementsHandler(IUnitOfWorkFactory unitOfWorkFactory, ICurrentUser currentUser)
        {
            _unitOfWorkFactory = unitOfWorkFactory;
            _currentUser = currentUser;
        }
        /// <summary>Retrieves earned achievements.</summary>
        public async Task<List<GetEarnedAchievementsResponse>> GetEarnedAchievementsAsync(int userId)
        {
            await using IUnitOfWork unitOfWork = _unitOfWorkFactory.Create();

            GetUserProfileResponse? profile = await unitOfWork.Users.GetProfileAsync(userId);
            if (profile is null || (profile.IsPrivate && _currentUser.IsAdmin is false && _currentUser.UserId != userId))
            {
                throw new TravelAppNotFoundException($"User {userId} was not found.");
            }

            var result = await unitOfWork.Achievements.GetEarnedByUserAsync(userId);
            return result;
        }
    }
}
