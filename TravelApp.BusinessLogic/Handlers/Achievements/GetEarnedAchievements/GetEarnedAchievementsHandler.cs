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

        /// <summary>Initializes a new instance of the <see cref="GetEarnedAchievementsHandler"/> class.</summary>
        /// <param name="unitOfWorkFactory">The unit of work factory.</param>
        /// <param name="currentUser">The current logged-in user.</param>
        public GetEarnedAchievementsHandler(IUnitOfWorkFactory unitOfWorkFactory, ICurrentUser currentUser)
        {
            _unitOfWorkFactory = unitOfWorkFactory;
            _currentUser = currentUser;
        }

        /// <summary>Retrieves earned achievements for the specified user.</summary>
        public async Task<List<GetEarnedAchievementsResponse>> GetEarnedAchievementsAsync(int userId)
        {
            AccessRules.RequireOwnerOrAdmin(_currentUser, userId);

            await using IUnitOfWork unitOfWork = _unitOfWorkFactory.Create();

            // TODO: Replace with Cecilia's AccessInfo when available.
            GetUserProfileResponse? existingProfile = await unitOfWork.Users.GetProfileAsync(userId);

            if (existingProfile is null)
            {
                throw new TravelAppNotFoundException($"User with ID {userId} not found.");
            }

            List<GetEarnedAchievementsResponse> earnedAchievements = await unitOfWork.Achievements.GetEarnedByUserAsync(userId);

            return earnedAchievements;
        }
    }
}
