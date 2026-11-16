using TravelApp.BusinessLogic.Auth;
using TravelApp.BusinessLogic.DTOs.Trips;
using TravelApp.BusinessLogic.Interfaces;

namespace TravelApp.BusinessLogic.Handlers.Trips.GetTrip
{
    public class GetTripsHandler
    {
        private readonly IUnitOfWorkFactory _unitOfWorkFactory;
        private readonly ICurrentUser _currentUser;

        public GetTripsHandler(IUnitOfWorkFactory unitOfWorkFactory, ICurrentUser currentUser)
        {
            _unitOfWorkFactory = unitOfWorkFactory;
            _currentUser = currentUser;
        }

        public async Task<List<GetTripsResponse>> GetTripsAsync(int? countryId = null, int? continentId = null)
        {
            await using IUnitOfWork unitOfWork = _unitOfWorkFactory.Create();

            List<TripSummaryDTO> trips = await unitOfWork.Trips.GetSummariesByUserAsync(_currentUser.UserId, countryId, continentId);

            return trips.Select(trip => new GetTripsResponse
            {
                Id = trip.Id,
                StartDate = trip.StartDate,
                EndDate = trip.EndDate,
                IsCompleted = trip.IsCompleted,
                PlanNotes = trip.PlanNotes,
                ExperienceNotes = trip.ExperienceNotes,

                Cities = trip.Cities.Select(city => new GetTripsCityResponse
                {
                    Id = city.Id,
                    Name = city.Name
                }).ToList(),

                ActivityCount = trip.ActivityCount,
                ParticipantCount = trip.ParticipantCount
            }).ToList();
        }
    }
}