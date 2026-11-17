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
        /// <exception cref="TravelAppNotFoundException">Thrown when the user does not exist or the profile is private.</exception>
        public async Task<GetUserProfileResponse> GetUserProfileAsync(int userId)
        {
            await using IUnitOfWork unitOfWork = _unitOfWorkFactory.Create();

            UserProfileDetailsDto? existingProfile = await unitOfWork.Users.GetProfileAsync(userId);

            bool profileNotFound = existingProfile is null;
            bool profileIsPrivate = existingProfile is not null && existingProfile.IsPrivate;
            bool currentUserIsAdmin = _currentUser.IsAdmin;
            bool currentUserIsNotOwner = _currentUser.UserId != userId;

            bool privateProfileIsHidden = profileIsPrivate && currentUserIsAdmin is false && currentUserIsNotOwner;

            if (profileNotFound || privateProfileIsHidden)
            {
                throw new TravelAppNotFoundException($"User {userId} was not found.");
            }

            UserProfileDetailsDto profile = existingProfile ?? throw new TravelAppNotFoundException($"User {userId} was not found.");

            GetUserProfileResponse response = new GetUserProfileResponse
            {
                UserId = profile.UserId,
                FullName = profile.FullName,
                Age = profile.Age,
                HomeCity = profile.HomeCity,
                HomeCountry = profile.HomeCountry,
                IsPrivate = profile.IsPrivate,
                MemberSince = profile.MemberSince,
                FriendCount = profile.FriendCount,
                CompletedTrips = profile.CompletedTrips,
                PlannedTrips = profile.PlannedTrips,
                ReviewCount = profile.ReviewCount,
                AchievementCount = profile.AchievementCount
            };

            return response;
        }
    }
}
