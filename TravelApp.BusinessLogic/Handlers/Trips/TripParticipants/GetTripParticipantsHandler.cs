using TravelApp.BusinessLogic.Auth;
using TravelApp.BusinessLogic.Exceptions.SpecificCustomException;
using TravelApp.BusinessLogic.Handlers.Trips.TripParticipants;
using TravelApp.BusinessLogic.Interfaces;
using TravelApp.BusinessLogic.Models.Trips;

namespace TravelApp.BusinessLogic.Handlers.Trips
{
    /// <summary>Returns the participants of a trip.</summary>
    public class GetTripParticipantsHandler
    {
        private readonly IUnitOfWorkFactory _unitOfWorkFactory;
        private readonly ICurrentUser _currentUser;

        /// <summary>Initializes a new instance of the <see cref="GetTripParticipantsHandler"/> class.</summary>
        /// <param name="unitOfWorkFactory">The unit of work factory.</param>
        /// <param name="currentUser">The current logged-in user.</param>
        public GetTripParticipantsHandler(IUnitOfWorkFactory unitOfWorkFactory, ICurrentUser currentUser)
        {
            _unitOfWorkFactory = unitOfWorkFactory;
            _currentUser = currentUser;
        }

        /// <summary>Returns the participants of the trip with the specified identifier.</summary>
        /// <param name="tripId">The trip identifier.</param>
        /// <returns>The participants of the trip.</returns>
        /// <exception cref="TravelAppNotFoundException">Thrown when the trip does not exist.</exception>
        /// <exception cref="TravelAppForbiddenException">Thrown when the current user cannot view the trip.</exception>
        public async Task<List<GetTripParticipantResponse>> GetTripParticipantsAsync(int tripId)
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
                throw new TravelAppForbiddenException("You cannot view this trip.");
            }

            return trip.TripParticipants.Select(participant => new GetTripParticipantResponse
            {
                Id = participant.User!.Id,
                FullName = $"{participant.User.FirstName} {participant.User.LastName}"
            }).ToList();
        }
    }
}