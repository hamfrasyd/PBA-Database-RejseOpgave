using TravelApp.BusinessLogic.Auth;
using TravelApp.BusinessLogic.Exceptions.SpecificCustomException;
using TravelApp.BusinessLogic.Interfaces;
using TravelApp.BusinessLogic.Models;

namespace TravelApp.BusinessLogic.Handlers.Trips
{
    public class DeleteTripHandler
    {
        private readonly IUnitOfWorkFactory _unitOfWorkFactory;
        private readonly ICurrentUser _currentUser;

        public DeleteTripHandler(IUnitOfWorkFactory unitOfWorkFactory, ICurrentUser currentUser)
        {
            _unitOfWorkFactory = unitOfWorkFactory;
            _currentUser = currentUser;
        }

        public async Task DeleteTripAsync(int tripId)
        {
            await using IUnitOfWork unitOfWork = _unitOfWorkFactory.Create();

            Trip? trip = await unitOfWork.Trips.GetByIdAsync(tripId);

            if (trip is null)
            {
                throw new TravelAppNotFoundException($"Trip {tripId} was not found.");
            }

            bool isParticipant = trip.TripParticipants.Any(p => p.User?.Id == _currentUser.UserId);

            if (!isParticipant && !_currentUser.IsAdmin)
            {
                throw new TravelAppForbiddenException("You cannot delete this trip.");
            }

            unitOfWork.Trips.Delete(trip);

            await unitOfWork.CommitAsync();
        }
    }
}