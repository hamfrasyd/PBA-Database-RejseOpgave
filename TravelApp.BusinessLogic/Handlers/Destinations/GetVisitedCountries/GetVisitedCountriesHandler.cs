using TravelApp.BusinessLogic.Handlers.Users;
using TravelApp.BusinessLogic.Exceptions.SpecificCustomException;
using TravelApp.BusinessLogic.Interfaces;
using TravelApp.BusinessLogic.Auth;

namespace TravelApp.BusinessLogic.Handlers.Destinations
{
    /// <summary>Retrieves countries visited on completed trips for a user's travel map.</summary>
    public class GetVisitedCountriesHandler
    {
        private readonly IUnitOfWorkFactory _unitOfWorkFactory;
        private readonly ICurrentUser _currentUser;

        /// <summary>Initializes a new GetVisitedCountriesHandler instance.</summary>
        public GetVisitedCountriesHandler(IUnitOfWorkFactory unitOfWorkFactory, ICurrentUser currentUser)
        {
            _unitOfWorkFactory = unitOfWorkFactory;
            _currentUser = currentUser;
        }

        /// <summary>Retrieves visited countries.</summary>
        public async Task<List<GetVisitedCountriesResponse>> GetVisitedCountriesAsync(int userId)
        {
            await using IUnitOfWork unitOfWork = _unitOfWorkFactory.Create();

            GetUserProfileResponse? profileFromDb = await unitOfWork.Users.GetProfileAsync(userId);

            if (profileFromDb is null || (profileFromDb.IsPrivate && _currentUser.IsAdmin is false && _currentUser.UserId != userId))
            {
                throw new TravelAppNotFoundException($"User {userId} was not found.");
            }

            List<GetCountriesResponse> countriesFromDb = await unitOfWork.Users.GetVisitedCountriesAsync(userId);

            List<GetVisitedCountriesResponse> visitedCountriesResponse = new List<GetVisitedCountriesResponse>();

            foreach (GetCountriesResponse country in countriesFromDb)
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
