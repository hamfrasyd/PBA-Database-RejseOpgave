using TravelApp.BusinessLogic.Auth;
using TravelApp.BusinessLogic.Exceptions.SpecificCustomException;
using TravelApp.BusinessLogic.Interfaces;
using TravelApp.BusinessLogic.Models.Trips;

namespace TravelApp.BusinessLogic.Handlers.Trips
{
    public class UpdateTripHandler
    {
        private readonly IUnitOfWorkFactory _unitOfWorkFactory;
        private readonly ICurrentUser _currentUser;

        public UpdateTripHandler(IUnitOfWorkFactory unitOfWorkFactory, ICurrentUser currentUser)
        {
            _unitOfWorkFactory = unitOfWorkFactory;
            _currentUser = currentUser;
        }

        public async Task UpdateTripAsync(UpdateTripRequest request)
        {
            if (request.StartDate > request.EndDate)
            {
                throw new TravelAppValidationException("Start date cannot be after end date.");
            }

            await using IUnitOfWork unitOfWork = _unitOfWorkFactory.Create();

            Trip? trip = await unitOfWork.Trips.GetByIdAsync(request.TripId);

            if (trip is null)
            {
                throw new TravelAppNotFoundException($"Trip {request.TripId} was not found.");
            }

            bool isParticipant = trip.TripParticipants.Any(p => p.User?.Id == _currentUser.UserId);

            if (!isParticipant && !_currentUser.IsAdmin)
            {
                throw new TravelAppForbiddenException("You cannot update this trip.");
            }

            trip.StartDate = request.StartDate;
            trip.EndDate = request.EndDate;
            trip.IsCompleted = request.IsCompleted;
            trip.PlanNotes = request.PlanNotes;
            trip.ExperienceNotes = request.ExperienceNotes;

            unitOfWork.Trips.Update(trip);
            await unitOfWork.CommitAsync();
        }
    }
}