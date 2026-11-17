using TravelApp.BusinessLogic.Auth;
using TravelApp.BusinessLogic.DTOs.Users;
using TravelApp.BusinessLogic.Handlers.Activities.GetActivity;
using TravelApp.BusinessLogic.Interfaces;
using static System.Runtime.InteropServices.JavaScript.JSType;

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

            UserProfileSearchDTO userSearchProfileDTO = new UserProfileSearchDTO
            {
                Search = request.Search,
                Descending = request.Descending,
                CurrentUserId = _currentUser.UserId,
                IncludePrivateProfiles = _currentUser.IsAdmin
            };

            List<UserProfileDetailsDTO> userProfiles = await unitOfWork.Users.GetProfilesAsync(userSearchProfileDTO);

            List<GetUserProfilesResponse> profileDetailsResponeList = userProfiles.Select(profile => new GetUserProfilesResponse
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

            return profileDetailsResponeList;
        }
    }
}
