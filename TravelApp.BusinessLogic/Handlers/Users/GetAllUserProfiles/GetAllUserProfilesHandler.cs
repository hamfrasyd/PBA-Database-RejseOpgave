using TravelApp.BusinessLogic.Auth;
using TravelApp.BusinessLogic.DTOs.Users;
using TravelApp.BusinessLogic.Exceptions.SpecificCustomException;
using TravelApp.BusinessLogic.Handlers.Users.GetUserProfile;
using TravelApp.BusinessLogic.Interfaces;
using TravelApp.BusinessLogic.Models.Users;

namespace TravelApp.BusinessLogic.Handlers.Users.GetAllUserProfiles
{
    /// <summary>Handles the get all user profiles use case.</summary>
    public class GetAllUserProfilesHandler
    {
        private readonly IUnitOfWorkFactory _unitOfWorkFactory;
        private readonly ICurrentUser _currentUser;

        /// <summary>Initializes a new GetAllUserProfilesHandler instance.</summary>
        /// <param name="unitOfWorkFactory">The unit of work factory.</param>
        /// <param name="currentUser">The current logged-in user.</param>
        public GetAllUserProfilesHandler(IUnitOfWorkFactory unitOfWorkFactory, ICurrentUser currentUser)
        {
            _unitOfWorkFactory = unitOfWorkFactory;
            _currentUser = currentUser;
        }

        /// <summary>Retrieves all user profiles that match the filter.</summary>
        /// <param name="request">The filter and sort options.</param>
        /// <returns>The matching user profiles.</returns>
        /// <exception cref="TravelAppValidationException">Thrown when the search text is empty.</exception>
        public async Task<List<UserProfileResponse>> GetAllUserProfilesAsync(GetAllUserProfilesRequest request)
        {
            if (request.Search is not null && string.IsNullOrWhiteSpace(request.Search))
            {
                throw new TravelAppValidationException("User search cannot be empty.");
            }

            await using IUnitOfWork unitOfWork = _unitOfWorkFactory.Create();

            UserProfileSearchDTO userSearchProfileDTO = new UserProfileSearchDTO
            {
                Search = request.Search?.Trim(),
                Descending = request.Descending,
                CurrentUserId = _currentUser.UserId,
                IncludePrivateProfiles = _currentUser.IsAdmin
            };

            List<UserProfileView> userProfiles = await unitOfWork.Users.GetProfilesAsync(userSearchProfileDTO);

            List<UserProfileResponse> profiles = userProfiles.Select(profile => new UserProfileResponse
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
            }).ToList();

            return profiles;
        }
    }
}
