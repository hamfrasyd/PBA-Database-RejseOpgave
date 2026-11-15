using TravelApp.BusinessLogic.Auth;
using TravelApp.BusinessLogic.Interfaces;
using TravelApp.BusinessLogic.Models;

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

        public async Task<List<GetTripsResponse>> GetTripsAsync()
        {
            await using IUnitOfWork unitOfWork = _unitOfWorkFactory.Create();

            List<Trip> trips = await unitOfWork.Trips.GetByUserAsync(_currentUser.UserId);

            return trips.Select(trip => new GetTripsResponse
            {
                Id = trip.Id,
                StartDate = trip.StartDate,
                EndDate = trip.EndDate,
                IsCompleted = trip.IsCompleted,
                PlanNotes = trip.PlanNotes,
                ExperienceNotes = trip.ExperienceNotes
            }).ToList();
        }
    }
}