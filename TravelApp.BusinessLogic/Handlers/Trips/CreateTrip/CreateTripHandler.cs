using TravelApp.BusinessLogic.Auth;
using TravelApp.BusinessLogic.Exceptions.SpecificCustomException;
using TravelApp.BusinessLogic.Handlers.Trips.GetTrip;
using TravelApp.BusinessLogic.Handlers.Trips.TripCity;
using TravelApp.BusinessLogic.Interfaces;
using TravelApp.BusinessLogic.Models;

namespace TravelApp.BusinessLogic.Handlers.Trips
{
    public class CreateTripHandler
    {
        private readonly IUnitOfWorkFactory _unitOfWorkFactory;
        private readonly ICurrentUser _currentUser;

        public CreateTripHandler(IUnitOfWorkFactory unitOfWorkFactory, ICurrentUser currentUser)
        {
            _unitOfWorkFactory = unitOfWorkFactory;
            _currentUser = currentUser;
        }

        public async Task<GetTripResponse> CreateTripAsync(CreateTripRequest request)
        {
            if (request.StartDate > request.EndDate)
            {
                throw new TravelAppValidationException("Start date cannot be after end date.");
            }

            await using IUnitOfWork unitOfWork = _unitOfWorkFactory.Create();

            TravelUser? user = await unitOfWork.Users.GetByIdAsync(_currentUser.UserId);

            if (user is null)
            {
                throw new TravelAppNotFoundException( "The current user was not found.");
            }

            Trip trip = new Trip
            {
                StartDate = request.StartDate,
                EndDate = request.EndDate,
                PlanNotes = request.PlanNotes,
                IsCompleted = false
            };

            trip.TripParticipants.Add(new TripParticipant
            {
                User = user
            });

            await unitOfWork.Trips.AddAsync(trip);

            await unitOfWork.CommitAsync();

            return new GetTripResponse
            {
                Id = trip.Id,
                StartDate = trip.StartDate,
                EndDate = trip.EndDate,
                IsCompleted = trip.IsCompleted,
                PlanNotes = trip.PlanNotes,
                ExperienceNotes = trip.ExperienceNotes,

                Cities = trip.TripStops.Select(stop => new GetTripCityResponse
                {
                    Id = stop.City!.Id,
                    City = stop.City.Name,
                    Country = stop.City.Country!.Name,
                    Continent = stop.City.Country.Continent!.Name
                }).ToList()
            };
        }
    }
}