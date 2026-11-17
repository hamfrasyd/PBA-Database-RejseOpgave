using TravelApp.BusinessLogic.Auth;
using TravelApp.BusinessLogic.DTOs.Users;
using TravelApp.BusinessLogic.Exceptions.SpecificCustomException;
using TravelApp.BusinessLogic.Interfaces;

namespace TravelApp.BusinessLogic.Handlers.Users.GetUserProfile
{
    /// <summary>Handles the get user profile use case.</summary>
    public class GetUserProfileHandler
    {
        private readonly IUnitOfWorkFactory _unitOfWorkFactory;
        private readonly ICurrentUser _currentUser;

        /// <summary>Initializes a new GetUserProfileHandler instance.</summary>
        /// <param name="unitOfWorkFactory">The unit of work factory.</param>
        /// <param name="currentUser">The current logged-in user.</param>
        public GetUserProfileHandler(IUnitOfWorkFactory unitOfWorkFactory, ICurrentUser currentUser)
        {
            _unitOfWorkFactory = unitOfWorkFactory;
            _currentUser = currentUser;
        }

        /// <summary>Retrieves user profile.</summary>
        /// <param name="userId">The user identifier.</param>
        /// <returns>The user profile.</returns>
        /// <exception cref="TravelAppNotFoundException">Thrown when the user does not exist.</exception>
        /// <exception cref="TravelAppForbiddenException">Thrown when the profile is private and the current user is neither the owner nor an administrator.</exception>
        public async Task<UserProfileResponse> GetUserProfileAsync(int userId)
        {
            await using IUnitOfWork unitOfWork = _unitOfWorkFactory.Create();

            UserProfileDetailsDTO? existingProfile = await unitOfWork.Users.GetProfileAsync(userId);

            if (existingProfile is null)
            {
                throw new TravelAppNotFoundException($"User {userId} was not found.");
            }

            AccessRules.RequirePublicOrOwnerOrAdmin(_currentUser, userId, existingProfile.IsPrivate);

            UserProfileResponse response = new UserProfileResponse
            {
                UserId = existingProfile.UserId,
                FullName = existingProfile.FullName,
                Age = existingProfile.Age,
                HomeCity = existingProfile.HomeCity,
                HomeCountry = existingProfile.HomeCountry,
                IsPrivate = existingProfile.IsPrivate,
                MemberSince = existingProfile.MemberSince,
                FriendCount = existingProfile.FriendCount,
                CompletedTrips = existingProfile.CompletedTrips,
                PlannedTrips = existingProfile.PlannedTrips,
                ReviewCount = existingProfile.ReviewCount,
                AchievementCount = existingProfile.AchievementCount
            };

            return response;
        }
    }
}
