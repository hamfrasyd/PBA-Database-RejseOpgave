using TravelApp.BusinessLogic.Auth;
using TravelApp.BusinessLogic.Exceptions.SpecificCustomException;
using TravelApp.BusinessLogic.Interfaces;
using TravelApp.BusinessLogic.Models;

namespace TravelApp.BusinessLogic.Handlers.Trips
{
    public class ManageTripRelationsHandler
    {
        private readonly IUnitOfWorkFactory _unitOfWorkFactory;
        private readonly ICurrentUser _currentUser;

        public ManageTripRelationsHandler(IUnitOfWorkFactory unitOfWorkFactory, ICurrentUser currentUser)
        {
            _unitOfWorkFactory = unitOfWorkFactory;
            _currentUser = currentUser;
        }

        private static async Task<Trip> GetTripAsync(IUnitOfWork unitOfWork, int tripId, ICurrentUser currentUser)
        {
            Trip? trip = await unitOfWork.Trips.GetByIdAsync(tripId);

            if (trip is null)
            {
                throw new TravelAppNotFoundException($"Trip {tripId} was not found.");
            }

            bool isParticipant = trip.TripParticipants.Any(p => p.User?.Id == currentUser.UserId);

            if (!isParticipant && !currentUser.IsAdmin)
            {
                throw new TravelAppForbiddenException("You cannot modify this trip.");
            }

            return trip;
        }

        public async Task AddCityAsync(int tripId, int cityId)
        {
            await using IUnitOfWork unitOfWork = _unitOfWorkFactory.Create();

            Trip trip = await GetTripAsync(unitOfWork, tripId, _currentUser);

            City? city = await unitOfWork.Destinations.GetCityByIdAsync(cityId);

            if (city is null)
            {
                throw new TravelAppNotFoundException($"City {cityId} was not found.");
            }

            bool exists = trip.TripStops.Any(s => s.City?.Id == cityId);

            if (exists)
            {
                throw new TravelAppConflictException("City is already part of the trip.");
            }

            unitOfWork.Trips.AddStop(new TripStop
            {
                Trip = trip,
                City = city
            });

            await unitOfWork.CommitAsync();
        }

        public async Task RemoveCityAsync(int tripId, int cityId)
        {
            await using IUnitOfWork unitOfWork = _unitOfWorkFactory.Create();

            Trip trip = await GetTripAsync(unitOfWork, tripId, _currentUser);

            TripStop? stop = trip.TripStops.FirstOrDefault(s => s.City?.Id == cityId);

            if (stop is null)
            {
                throw new TravelAppNotFoundException("City is not part of the trip.");
            }

            // Prevent activities from becoming disconnected from the trip's selected destinations.
            bool hasActivities = trip.TripActivities.Any(a => a.Activity?.City?.Id == cityId);

            if (hasActivities)
            {
                throw new TravelAppConflictException("Remove the city's activities first.");
            }

            unitOfWork.Trips.RemoveStop(stop);
            await unitOfWork.CommitAsync();
        }

        public async Task AddActivityAsync(int tripId, int activityId)
        {
            await using IUnitOfWork unitOfWork = _unitOfWorkFactory.Create();

            Trip trip = await GetTripAsync(unitOfWork, tripId, _currentUser);

            Activity? activity = await unitOfWork.Destinations.GetActivityByIdAsync(activityId);

            if (activity is null)
            {
                throw new TravelAppNotFoundException($"Activity {activityId} was not found.");
            }

            bool cityIsOnTrip = trip.TripStops.Any(s => s.City?.Id == activity.City?.Id);

            if (!cityIsOnTrip)
            {
                throw new TravelAppValidationException("Add the activity's city to the trip first.");
            }

            bool exists = trip.TripActivities.Any(a => a.Activity?.Id == activityId);

            if (exists)
            {
                throw new TravelAppConflictException("Activity is already part of the trip.");
            }

            unitOfWork.Trips.AddTripActivity(new TripActivity
            {
                Trip = trip,
                Activity = activity
            });

            await unitOfWork.CommitAsync();
        }

        public async Task RemoveActivityAsync(int tripId, int activityId)
        {
            await using IUnitOfWork unitOfWork = _unitOfWorkFactory.Create();

            Trip trip = await GetTripAsync(unitOfWork, tripId, _currentUser);

            TripActivity? activity = trip.TripActivities.FirstOrDefault(a => a.Activity?.Id == activityId);

            if (activity is null)
            {
                throw new TravelAppNotFoundException("Activity is not part of the trip.");
            }

            unitOfWork.Trips.RemoveTripActivity(activity);
            await unitOfWork.CommitAsync();
        }

        public async Task AddParticipantAsync(int tripId, int userId)
        {
            await using IUnitOfWork unitOfWork = _unitOfWorkFactory.Create();

            Trip trip = await GetTripAsync(unitOfWork, tripId, _currentUser);
            TravelUser? user = await unitOfWork.Users.GetByIdAsync(userId);
            if (user is null)
            {
                throw new TravelAppNotFoundException($"User {userId} was not found.");
            }

            bool exists = trip.TripParticipants.Any(p => p.User?.Id == userId);
            if (exists)
            {
                throw new TravelAppConflictException("User is already participating.");
            }

            unitOfWork.Trips.AddParticipant(new TripParticipant
            {
                Trip = trip,
                User = user
            });
            await unitOfWork.CommitAsync();
        }

        public async Task RemoveParticipantAsync(int tripId, int userId)
        {
            await using IUnitOfWork unitOfWork = _unitOfWorkFactory.Create();

            Trip trip = await GetTripAsync(unitOfWork, tripId, _currentUser);

            TripParticipant? participant = trip.TripParticipants.FirstOrDefault(p => p.User?.Id == userId);

            if (participant is null)
            {
                throw new TravelAppNotFoundException("User is not participating in the trip.");
            }

            if (trip.TripParticipants.Count == 1)
            {
                throw new TravelAppConflictException("A trip must have at least one participant.");
            }

            unitOfWork.Trips.RemoveParticipant(participant);
            await unitOfWork.CommitAsync();
        }
    }
}