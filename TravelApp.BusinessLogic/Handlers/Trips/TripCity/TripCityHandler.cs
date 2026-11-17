
using TravelApp.BusinessLogic.Auth;
using TravelApp.BusinessLogic.DTOs.Trips;
using TravelApp.BusinessLogic.Exceptions.SpecificCustomException;
using TravelApp.BusinessLogic.Interfaces;
using TravelApp.BusinessLogic.Models.Destinations;

namespace TravelApp.BusinessLogic.Handlers.Trips.TripCity
{
    /// <summary>Handler for managing trip-city relationships.</summary>
    public class TripCityHandler
    {
        private readonly IUnitOfWorkFactory _unitOfWorkFactory;
        private readonly ICurrentUser _currentUser;

        /// <summary>Initializes a new instance of the <see cref="TripCityHandler"/> class.</summary>
        public TripCityHandler(IUnitOfWorkFactory unitOfWorkFactory, ICurrentUser currentUser)
        {
            _unitOfWorkFactory = unitOfWorkFactory;
            _currentUser = currentUser;
        }

        /// <summary>Adds a city to a trip.</summary>
        public async Task AddCityAsync(int tripId, int cityId)
        {
            await using IUnitOfWork unitOfWork = _unitOfWorkFactory.Create();

            await TripAccessHelper.EnsureCanModifyAsync(unitOfWork, tripId, _currentUser);

            City? city = await unitOfWork.Locations.GetCityByIdAsync(cityId);
            if (city is null)
            {
                throw new TravelAppNotFoundException($"City {cityId} was not found.");
            }

            TripRelationsDTO? relations = await unitOfWork.Trips.GetRelationsAsync(tripId);
            if (relations is null)
            {
                throw new TravelAppNotFoundException($"Trip {tripId} was not found.");
            }

            bool exists = relations.CityIds.Contains(cityId);
            if (exists)
            {
                throw new TravelAppConflictException("City is already part of the trip.");
            }

            await unitOfWork.Trips.AddCityToTripAsync(tripId, cityId);

            await unitOfWork.CommitAsync();
        }

        /// <summary>Removes a city from a trip.</summary>
        public async Task RemoveCityAsync(int tripId, int cityId)
        {
            await using IUnitOfWork unitOfWork = _unitOfWorkFactory.Create();

            await TripAccessHelper.EnsureCanModifyAsync(unitOfWork, tripId, _currentUser);

            TripRelationsDTO? relations = await unitOfWork.Trips.GetRelationsAsync(tripId);
            if (relations is null)
            {
                throw new TravelAppNotFoundException($"Trip {tripId} was not found.");
            }

            bool exists = relations.CityIds.Contains(cityId);
            if (!exists)
            {
                throw new TravelAppNotFoundException("City is not part of the trip.");
            }

            bool hasActivities = relations.Activities.Any(activity => activity.CityId == cityId);
            if (hasActivities)
            {
                throw new TravelAppConflictException("Remove the city's activities first.");
            }

            await unitOfWork.Trips.RemoveCityFromTripAsync(tripId, cityId);

            await unitOfWork.CommitAsync();
        }
    }
}
