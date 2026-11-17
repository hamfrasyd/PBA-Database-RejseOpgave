
using TravelApp.BusinessLogic.Auth;
using TravelApp.BusinessLogic.DTOs.Trips;
using TravelApp.BusinessLogic.Exceptions.SpecificCustomException;
using TravelApp.BusinessLogic.Interfaces;
using TravelApp.BusinessLogic.Models.Destinations;
using TravelApp.BusinessLogic.Models.Users;

namespace TravelApp.BusinessLogic.Handlers.Trips
{
    public class ManageTripRelationsHandler
    {
        private readonly IUnitOfWorkFactory _unitOfWorkFactory;
        private readonly ICurrentUser _currentUser;

        public ManageTripRelationsHandler(
            IUnitOfWorkFactory unitOfWorkFactory,
            ICurrentUser currentUser)
        {
            _unitOfWorkFactory = unitOfWorkFactory;
            _currentUser = currentUser;
        }

        private static async Task<TripRelationsDTO> GetTripRelationsAsync(IUnitOfWork unitOfWork, int tripId, ICurrentUser currentUser)
        {
            TripAccessInfoDTO? access = await unitOfWork.Trips.GetAccessInfoAsync(tripId, currentUser.UserId);
            if (access is null)
            {
                throw new TravelAppNotFoundException($"Trip {tripId} was not found.");
            }
            if (!access.IsParticipant && !currentUser.IsAdmin)
            {
                throw new TravelAppForbiddenException("You cannot modify this trip.");
            }

            TripRelationsDTO? relations = await unitOfWork.Trips.GetRelationsAsync(tripId);
            if (relations is null)
            {
                throw new TravelAppNotFoundException($"Trip {tripId} was not found.");
            }
            return relations;
        }

        public async Task AddCityAsync(int tripId, int cityId)
        {
            await using IUnitOfWork unitOfWork = _unitOfWorkFactory.Create();

            TripRelationsDTO trip = await GetTripRelationsAsync(unitOfWork, tripId, _currentUser);

            City? city = await unitOfWork.Locations.GetCityByIdAsync(cityId);
            if (city is null)
            {
                throw new TravelAppNotFoundException($"City {cityId} was not found.");
            }
            bool exists = trip.CityIds.Contains(cityId);
            if (exists)
            {
                throw new TravelAppConflictException("City is already part of the trip.");
            }
            await unitOfWork.Trips.AddCityAsync(tripId, cityId);
            await unitOfWork.CommitAsync();
        }

        public async Task RemoveCityAsync(int tripId, int cityId)
        {
            await using IUnitOfWork unitOfWork = _unitOfWorkFactory.Create();

            TripRelationsDTO trip = await GetTripRelationsAsync(unitOfWork, tripId, _currentUser);

            bool exists = trip.CityIds.Contains(cityId);
            if (!exists)
            {
                throw new TravelAppNotFoundException("City is not part of the trip.");
            }

            bool hasActivities = trip.Activities.Any(activity => activity.CityId == cityId);

            if (hasActivities)
            {
                throw new TravelAppConflictException("Remove the city's activities first.");
            }

            await unitOfWork.Trips.RemoveCityAsync(tripId, cityId);
            await unitOfWork.CommitAsync();
        }

        public async Task AddActivityAsync(int tripId, int activityId)
        {
            await using IUnitOfWork unitOfWork = _unitOfWorkFactory.Create();

            TripRelationsDTO trip = await GetTripRelationsAsync(unitOfWork, tripId, _currentUser);

            Activity? activity = await unitOfWork.Activities.GetActivityByIdAsync(activityId);

            if (activity is null)
            {
                throw new TravelAppNotFoundException($"Activity {activityId} was not found.");
            }

            bool cityIsOnTrip = trip.CityIds.Contains(activity.City!.Id);

            if (!cityIsOnTrip)
            {
                throw new TravelAppValidationException("Add the activity's city to the trip first.");
            }

            bool exists = trip.Activities.Any(a => a.Id == activityId);

            if (exists)
            {
                throw new TravelAppConflictException("Activity is already part of the trip.");
            }

            await unitOfWork.Trips.AddActivityAsync(tripId, activityId);
            await unitOfWork.CommitAsync();
        }

        public async Task RemoveActivityAsync(int tripId, int activityId)
        {
            await using IUnitOfWork unitOfWork = _unitOfWorkFactory.Create();

            TripRelationsDTO trip = await GetTripRelationsAsync(unitOfWork, tripId, _currentUser);

            bool exists = trip.Activities.Any(activity => activity.Id == activityId);

            if (!exists)
            {
                throw new TravelAppNotFoundException("Activity is not part of the trip.");
            }

            await unitOfWork.Trips.RemoveActivityAsync(tripId, activityId);
            await unitOfWork.CommitAsync();
        }

        public async Task AddParticipantAsync(int tripId, int userId)
        {
            await using IUnitOfWork unitOfWork = _unitOfWorkFactory.Create();

            TripRelationsDTO trip = await GetTripRelationsAsync(unitOfWork, tripId, _currentUser);

            TravelUser? user = await unitOfWork.Users.GetByIdAsync(userId);
            if (user is null)
            {
                throw new TravelAppNotFoundException($"User {userId} was not found.");
            }

            bool exists = trip.ParticipantIds.Contains(userId);

            if (exists)
            {
                throw new TravelAppConflictException("User is already participating.");
            }

            await unitOfWork.Trips.AddParticipantAsync(tripId, userId);
            await unitOfWork.CommitAsync();
        }

        public async Task RemoveParticipantAsync(int tripId, int userId)
        {
            await using IUnitOfWork unitOfWork = _unitOfWorkFactory.Create();

            TripRelationsDTO trip = await GetTripRelationsAsync(unitOfWork, tripId, _currentUser);

            bool exists = trip.ParticipantIds.Contains(userId);
            if (!exists)
            {
                throw new TravelAppNotFoundException("User is not participating in the trip.");
            }

            if (trip.ParticipantIds.Count == 1)
            {
                throw new TravelAppConflictException("A trip must have at least one participant.");
            }

            await unitOfWork.Trips.RemoveParticipantAsync(tripId, userId);
            await unitOfWork.CommitAsync();
        }
    }
}
