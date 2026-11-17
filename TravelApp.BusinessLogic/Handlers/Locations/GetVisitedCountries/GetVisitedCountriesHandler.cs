using TravelApp.BusinessLogic.Auth;
using TravelApp.BusinessLogic.DTOs.Users;
using TravelApp.BusinessLogic.Exceptions.SpecificCustomException;
using TravelApp.BusinessLogic.Handlers.Locations.Data;
using TravelApp.BusinessLogic.Interfaces;

namespace TravelApp.BusinessLogic.Handlers.Locations.GetVisitedCountries
{
    /// <summary>Retrieves countries visited on completed trips for a user's travel map.</summary>
    public class GetVisitedCountriesHandler
    {
        private readonly IUnitOfWorkFactory _unitOfWorkFactory;
        private readonly ICurrentUser _currentUser;

        /// <summary>Initializes a new GetVisitedCountriesHandler instance.</summary>
        /// <param name="unitOfWorkFactory">The unit of work factory.</param>
        /// <param name="currentUser">The current logged-in user.</param>
        public GetVisitedCountriesHandler(IUnitOfWorkFactory unitOfWorkFactory, ICurrentUser currentUser)
        {
            _unitOfWorkFactory = unitOfWorkFactory;
            _currentUser = currentUser;
        }

        /// <summary>Retrieves visited countries.</summary>
        /// <param name="userId">The user identifier.</param>
        /// <returns>The countries visited on completed trips.</returns>
        /// <exception cref="TravelAppNotFoundException">Thrown when the user does not exist or the profile is private.</exception>
        public async Task<List<GetVisitedCountriesResponse>> GetVisitedCountriesAsync(int userId)
        {
            await using IUnitOfWork unitOfWork = _unitOfWorkFactory.Create();

            UserProfileDetailsDto? profileFromDb = await unitOfWork.Users.GetProfileAsync(userId);

            bool profileIsMissing = profileFromDb is null;
            bool profileIsPrivate = profileFromDb is not null && profileFromDb.IsPrivate;
            bool currentUserIsAdmin = _currentUser.IsAdmin;
            bool currentUserOwnsProfile = _currentUser.UserId == userId;
            bool privateProfileIsHidden = profileIsPrivate && currentUserIsAdmin is false && currentUserOwnsProfile is false;

            if (profileIsMissing || privateProfileIsHidden)
            {
                throw new TravelAppNotFoundException($"User {userId} was not found.");
            }

            List<VisitedCountryDto> countriesFromDb = await unitOfWork.Users.GetVisitedCountriesAsync(userId);

            List<GetVisitedCountriesResponse> visitedCountriesResponse = new List<GetVisitedCountriesResponse>();

            foreach (VisitedCountryDto country in countriesFromDb)
            {
                visitedCountriesResponse.Add(new GetVisitedCountriesResponse
                {
                    Id = country.Id,
                    ContinentId = country.ContinentId,
                    Name = country.Name
                });
            }

            return visitedCountriesResponse;
        }
    }
}
