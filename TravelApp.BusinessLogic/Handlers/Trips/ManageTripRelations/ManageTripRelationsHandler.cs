
using TravelApp.BusinessLogic.Auth;
using TravelApp.BusinessLogic.DTOs.Activities;
using TravelApp.BusinessLogic.DTOs.Trips;
using TravelApp.BusinessLogic.Exceptions.SpecificCustomException;
using TravelApp.BusinessLogic.Interfaces;
using TravelApp.BusinessLogic.Models.Destinations;
using TravelApp.BusinessLogic.Models.Users;

namespace TravelApp.BusinessLogic.Handlers.Trips
{
    /// <summary>Adds and removes cities, activities, and participants on a trip.</summary>
    public class ManageTripRelationsHandler
    {
        private readonly IUnitOfWorkFactory _unitOfWorkFactory;
        private readonly ICurrentUser _currentUser;

        /// <summary>Initializes a new instance of the <see cref="ManageTripRelationsHandler"/> class.</summary>
        /// <param name="unitOfWorkFactory">The unit of work factory.</param>
        /// <param name="currentUser">The current logged-in user.</param>
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

        /// <summary>Adds a city to the trip.</summary>
        /// <param name="tripId">The trip identifier.</param>
        /// <param name="cityId">The city identifier.</param>
        /// <exception cref="TravelAppNotFoundException">Thrown when the trip or city does not exist.</exception>
        /// <exception cref="TravelAppForbiddenException">Thrown when the current user cannot modify the trip.</exception>
        /// <exception cref="TravelAppConflictException">Thrown when the city is already part of the trip.</exception>
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

        /// <summary>Removes a city from the trip.</summary>
        /// <param name="tripId">The trip identifier.</param>
        /// <param name="cityId">The city identifier.</param>
        /// <exception cref="TravelAppNotFoundException">Thrown when the trip does not exist or the city is not on the trip.</exception>
        /// <exception cref="TravelAppForbiddenException">Thrown when the current user cannot modify the trip.</exception>
        /// <exception cref="TravelAppConflictException">Thrown when the city still has activities on the trip.</exception>
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

        /// <summary>Adds an activity to the trip.</summary>
        /// <param name="tripId">The trip identifier.</param>
        /// <param name="activityId">The activity identifier.</param>
        /// <exception cref="TravelAppNotFoundException">Thrown when the trip or activity does not exist.</exception>
        /// <exception cref="TravelAppForbiddenException">Thrown when the current user cannot modify the trip.</exception>
        /// <exception cref="TravelAppValidationException">Thrown when the activity's city is not on the trip.</exception>
        /// <exception cref="TravelAppConflictException">Thrown when the activity is already part of the trip.</exception>
        public async Task AddActivityAsync(int tripId, int activityId)
        {
            await using IUnitOfWork unitOfWork = _unitOfWorkFactory.Create();

            TripRelationsDTO trip = await GetTripRelationsAsync(unitOfWork, tripId, _currentUser);

            ActivityDto? activity = await unitOfWork.Activities.GetActivityByIdAsync(activityId);

            if (activity is null)
            {
                throw new TravelAppNotFoundException($"Activity {activityId} was not found.");
            }

            bool cityIsOnTrip = trip.CityIds.Contains(activity.CityId);

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

        /// <summary>Removes an activity from the trip.</summary>
        /// <param name="tripId">The trip identifier.</param>
        /// <param name="activityId">The activity identifier.</param>
        /// <exception cref="TravelAppNotFoundException">Thrown when the trip does not exist or the activity is not on the trip.</exception>
        /// <exception cref="TravelAppForbiddenException">Thrown when the current user cannot modify the trip.</exception>
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

        /// <summary>Adds a participant to the trip.</summary>
        /// <param name="tripId">The trip identifier.</param>
        /// <param name="userId">The user identifier.</param>
        /// <exception cref="TravelAppNotFoundException">Thrown when the trip or user does not exist.</exception>
        /// <exception cref="TravelAppForbiddenException">Thrown when the current user cannot modify the trip.</exception>
        /// <exception cref="TravelAppConflictException">Thrown when the user is already participating.</exception>
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

        /// <summary>Removes a participant from the trip.</summary>
        /// <param name="tripId">The trip identifier.</param>
        /// <param name="userId">The user identifier.</param>
        /// <exception cref="TravelAppNotFoundException">Thrown when the trip does not exist or the user is not participating.</exception>
        /// <exception cref="TravelAppForbiddenException">Thrown when the current user cannot modify the trip.</exception>
        /// <exception cref="TravelAppConflictException">Thrown when the user is the last participant.</exception>
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
