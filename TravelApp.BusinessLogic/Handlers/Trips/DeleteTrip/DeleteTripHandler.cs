using TravelApp.BusinessLogic.Auth;
using TravelApp.BusinessLogic.DTOs.Trips;
using TravelApp.BusinessLogic.Exceptions.SpecificCustomException;
using TravelApp.BusinessLogic.Interfaces;

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
            await using IUnitOfWork unitOfWork =  _unitOfWorkFactory.Create();

            TripAccessInfoDTO? access = await unitOfWork.Trips.GetAccessInfoAsync(tripId, _currentUser.UserId);

            if (access is null)
            {
                throw new TravelAppNotFoundException($"Trip {tripId} was not found.");
            }

            if (!access.IsParticipant && !_currentUser.IsAdmin)
            {
                throw new TravelAppForbiddenException("You cannot delete this trip.");
            }

            await unitOfWork.Trips.DeleteAsync(tripId);
            await unitOfWork.CommitAsync();
        }
    }
}