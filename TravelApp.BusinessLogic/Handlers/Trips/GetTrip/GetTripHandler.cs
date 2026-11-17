using TravelApp.BusinessLogic.Auth;
using TravelApp.BusinessLogic.DTOs.Trips;
using TravelApp.BusinessLogic.Exceptions.SpecificCustomException;
using TravelApp.BusinessLogic.Handlers.Trips.GetTrip;
using TravelApp.BusinessLogic.Handlers.Trips.TripCity;
using TravelApp.BusinessLogic.Interfaces;

namespace TravelApp.BusinessLogic.Handlers.Trips
{
    /// <summary>Handles the retrieval of trip details.</summary>
    public class GetTripHandler
    {
        private readonly IUnitOfWorkFactory _unitOfWorkFactory;
        private readonly ICurrentUser _currentUser;

        /// <summary>Initializes a new instance of the <see cref="GetTripHandler"/> class.</summary>
        public GetTripHandler(IUnitOfWorkFactory unitOfWorkFactory, ICurrentUser currentUser)
        {
            _unitOfWorkFactory = unitOfWorkFactory;
            _currentUser = currentUser;
        }

        /// <summary>Retrieves the details of a specific trip.</summary>
        public async Task<GetTripResponse> GetTripAsync(int tripId)
        {
            await using IUnitOfWork unitOfWork = _unitOfWorkFactory.Create();

            await TripAccessHelper.EnsureCanViewAsync(unitOfWork, tripId, _currentUser);

            TripDetailsDTO? trip = await unitOfWork.Trips.GetDetailsAsync(tripId);
            if (trip is null)
            {
                throw new TravelAppNotFoundException( $"Trip {tripId} was not found.");
            }

            return new GetTripResponse
            {
                Id = trip.Id,
                StartDate = trip.StartDate,
                EndDate = trip.EndDate,
                IsCompleted = trip.IsCompleted,
                PlanNotes = trip.PlanNotes,
                ExperienceNotes = trip.ExperienceNotes,

                Cities = trip.Cities.Select(city => new GetTripCityResponse
                {
                    Id = city.Id,
                    City = city.City,
                    Country = city.Country,
                    Continent = city.Continent
                }).ToList()
            };
        }
    }
}