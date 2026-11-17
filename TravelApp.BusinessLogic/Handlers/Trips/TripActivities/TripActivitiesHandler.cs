using TravelApp.BusinessLogic.Auth;
using TravelApp.BusinessLogic.DTOs.Trips;
using TravelApp.BusinessLogic.Exceptions.SpecificCustomException;
using TravelApp.BusinessLogic.Handlers.Activities.GetActivity;
using TravelApp.BusinessLogic.Interfaces;
using TravelApp.BusinessLogic.Models.Destinations;
using TravelApp.BusinessLogic.Models.Trips;

namespace TravelApp.BusinessLogic.Handlers.Trips
{
    /// <summary>Handler for managing trip-activity relationships.</summary>
    public class TripActivitiesHandler
    {
        private readonly IUnitOfWorkFactory _unitOfWorkFactory;
        private readonly ICurrentUser _currentUser;

        /// <summary>Initializes a new instance of the <see cref="TripActivitiesHandler"/> class.</summary>
        public TripActivitiesHandler(IUnitOfWorkFactory unitOfWorkFactory, ICurrentUser currentUser)
        {
            _unitOfWorkFactory = unitOfWorkFactory;
            _currentUser = currentUser;
        }

        /// <summary>Retrieves the activities associated with a specific trip.</summary>
        public async Task<List<GetActivityResponse>> GetTripActivitiesAsync(int tripId)
        {
            await using IUnitOfWork unitOfWork = _unitOfWorkFactory.Create();

            await TripAccessHelper.EnsureCanViewAsync(unitOfWork, tripId, _currentUser);

            List<TripActivityDTO> activities = await unitOfWork.Trips.GetActivitiesnTripAsync(tripId);

            return activities.Select(activity => new GetActivityResponse
            {
                Id = activity.Id,
                CityId = activity.CityId,
                Name = activity.Name,
                Description = activity.Description
            }).ToList();
        }

        /// <summary>Adds an activity to a trip.</summary>
        public async Task AddActivityAsync(int tripId, int activityId)
        {
            await using IUnitOfWork unitOfWork = _unitOfWorkFactory.Create();

            await TripAccessHelper.EnsureCanModifyAsync(unitOfWork, tripId, _currentUser);

            Activity? activity = await unitOfWork.Activities.GetActivityByIdAsync(activityId);
            if (activity is null)
            {
                throw new TravelAppNotFoundException($"Activity {activityId} was not found.");
            }

            TripRelationsDTO? relations = await unitOfWork.Trips.GetRelationsAsync(tripId);
            if (relations is null)
            {
                throw new TravelAppNotFoundException($"Trip {tripId} was not found.");
            }

            bool cityIsOnTrip = relations.CityIds.Contains(activity.City!.Id);
            if (!cityIsOnTrip)
            {
                throw new TravelAppValidationException("Add the activity's city to the trip first.");
            }

            bool exists = relations.Activities.Any(item => item.Id == activityId);
            if (exists)
            {
                throw new TravelAppConflictException("Activity is already part of the trip.");
            }

            await unitOfWork.Trips.AddActivityToTripAsync(tripId, activityId);

            await unitOfWork.CommitAsync();
        }

        /// <summary>Removes an activity from a trip.</summary>
        public async Task RemoveActivityAsync(int tripId, int activityId)
        {
            await using IUnitOfWork unitOfWork = _unitOfWorkFactory.Create();

            await TripAccessHelper.EnsureCanModifyAsync(unitOfWork, tripId, _currentUser);

            TripRelationsDTO? relations = await unitOfWork.Trips.GetRelationsAsync(tripId);
            if (relations is null)
            {
                throw new TravelAppNotFoundException($"Trip {tripId} was not found.");
            }

            bool exists = relations.Activities.Any(activity => activity.Id == activityId);
            if (!exists)
            {
                throw new TravelAppNotFoundException("Activity is not part of the trip.");
            }

            await unitOfWork.Trips.RemoveActivityFromTripAsync(tripId, activityId);
            await unitOfWork.CommitAsync();
        }
    }
}