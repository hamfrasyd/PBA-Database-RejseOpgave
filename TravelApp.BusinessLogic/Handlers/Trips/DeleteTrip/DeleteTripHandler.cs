using TravelApp.BusinessLogic.Auth;
using TravelApp.BusinessLogic.DTOs.Trips;
using TravelApp.BusinessLogic.Exceptions.SpecificCustomException;
using TravelApp.BusinessLogic.Interfaces;

namespace TravelApp.BusinessLogic.Handlers.Trips
{
    /// <summary>Deletes a trip the current user is allowed to remove.</summary>
    public class DeleteTripHandler
    {
        private readonly IUnitOfWorkFactory _unitOfWorkFactory;
        private readonly ICurrentUser _currentUser;

        /// <summary>Initializes a new instance of the <see cref="DeleteTripHandler"/> class.</summary>
        /// <param name="unitOfWorkFactory">The unit of work factory.</param>
        /// <param name="currentUser">The current logged-in user.</param>
        public DeleteTripHandler(IUnitOfWorkFactory unitOfWorkFactory, ICurrentUser currentUser)
        {
            _unitOfWorkFactory = unitOfWorkFactory;
            _currentUser = currentUser;
        }

        /// <summary>Deletes the trip with the specified identifier.</summary>
        /// <param name="tripId">The trip identifier.</param>
        /// <exception cref="TravelAppNotFoundException">Thrown when the trip does not exist.</exception>
        /// <exception cref="TravelAppForbiddenException">Thrown when the current user cannot delete the trip.</exception>
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