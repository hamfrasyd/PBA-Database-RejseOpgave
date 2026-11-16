using TravelApp.BusinessLogic.Handlers.Users;
using TravelApp.BusinessLogic.Exceptions.SpecificCustomException;
using TravelApp.BusinessLogic.Interfaces;
using TravelApp.BusinessLogic.Auth;

namespace TravelApp.BusinessLogic.Handlers.Achievements
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

            // #TODO brug Cecilias AccessInfo som inspiration til repo kaldet
            GetUserProfileResponse? existingProfile = await unitOfWork.Users.GetProfileAsync(userId);
            bool profileNotFound = existingProfile is null;
            if (profileNotFound)
            {
                throw new TravelAppNotFoundException($"User with ID {userId} not found.");
            }

            AccessRules.RequireOwnerOrAdmin(_currentUser, userId);

            var result = await unitOfWork.Achievements.GetEarnedByUserAsync(userId);
            return result;
        }
    }
}
