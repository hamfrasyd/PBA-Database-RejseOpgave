using TravelApp.BusinessLogic.Auth;
using TravelApp.BusinessLogic.DTOs.Locations;
using TravelApp.BusinessLogic.Exceptions.SpecificCustomException;
using TravelApp.BusinessLogic.Interfaces;
using TravelApp.BusinessLogic.Models.Users;

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
        /// <exception cref="TravelAppNotFoundException">Thrown when the user does not exist.</exception>
        /// <exception cref="TravelAppForbiddenException">Thrown when the profile is private and the current user is neither the owner nor an administrator.</exception>
        public async Task<List<GetVisitedCountriesResponse>> GetVisitedCountriesAsync(int userId)
        {
            await using IUnitOfWork unitOfWork = _unitOfWorkFactory.Create();

            UserProfileView? profileFromDb = await unitOfWork.Users.GetProfileAsync(userId);

            if (profileFromDb is null)
            {
                throw new TravelAppNotFoundException($"User with ID {userId} was not found.");
            }

            AccessRules.RequireCurrentUserIsOwnerOrAdminWhenProfileIsPrivate(_currentUser, userId, profileFromDb.IsPrivate);

            List<VisitedCountryDTO> countriesFromDb = await unitOfWork.Users.GetVisitedCountriesAsync(userId);

            List<GetVisitedCountriesResponse> visitedCountriesResponse = countriesFromDb.Select(country => new GetVisitedCountriesResponse
            {
                Id = country.Id,
                ContinentId = country.ContinentId,
                Name = country.Name
            }).ToList();

            return visitedCountriesResponse;
        }
    }
}
