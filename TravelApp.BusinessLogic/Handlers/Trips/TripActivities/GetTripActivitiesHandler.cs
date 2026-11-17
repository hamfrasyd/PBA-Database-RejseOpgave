using TravelApp.BusinessLogic.Auth;
using TravelApp.BusinessLogic.Exceptions.SpecificCustomException;
using TravelApp.BusinessLogic.Handlers.Activities.GetActivity;
using TravelApp.BusinessLogic.Interfaces;
using TravelApp.BusinessLogic.Models.Trips;

namespace TravelApp.BusinessLogic.Handlers.Trips
{
    public class GetTripActivitiesHandler
    {
        private readonly IUnitOfWorkFactory _unitOfWorkFactory;
        private readonly ICurrentUser _currentUser;

        public GetTripActivitiesHandler(IUnitOfWorkFactory unitOfWorkFactory, ICurrentUser currentUser)
        {
            _unitOfWorkFactory = unitOfWorkFactory;
            _currentUser = currentUser;
        }

        public async Task<List<GetActivityResponse>> GetTripActivitiesAsync(int tripId)
        {
            await using IUnitOfWork unitOfWork = _unitOfWorkFactory.Create();

            Trip? trip = await unitOfWork.Trips.GetByIdAsync(tripId);

            if (trip is null)
            {
                throw new TravelAppNotFoundException($"Trip {tripId} was not found.");
            }

            bool isParticipant = trip.TripParticipants.Any(p => p.User?.Id == _currentUser.UserId);

            if (!isParticipant && !_currentUser.IsAdmin)
            {
                throw new TravelAppForbiddenException("You cannot view this trip.");
            }

            return trip.TripActivities.Select(item => new GetActivityResponse
            {
                Id = item.Activity!.Id,
                CityId = item.Activity.City!.Id,
                Name = item.Activity.Name,
                Description = item.Activity.Description
            }).ToList();
        }
    }
}