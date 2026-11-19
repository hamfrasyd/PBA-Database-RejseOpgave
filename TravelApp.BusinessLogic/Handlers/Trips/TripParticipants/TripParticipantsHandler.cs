using TravelApp.BusinessLogic.Auth;
using TravelApp.BusinessLogic.DTOs.Trips;
using TravelApp.BusinessLogic.Exceptions.SpecificCustomException;
using TravelApp.BusinessLogic.Handlers.Trips.ManageTripRelations;
using TravelApp.BusinessLogic.Interfaces;

namespace TravelApp.BusinessLogic.Handlers.Trips.TripParticipants
{
    /// <summary>Handles the get trip participants use case.</summary>
    public class TripParticipantsHandler
    {
        private readonly IUnitOfWorkFactory _unitOfWorkFactory;
        private readonly ICurrentUser _currentUser;

        /// <summary>Initializes a new instance of the <see cref="TripParticipantsHandler"/> class.</summary>
        /// <param name="unitOfWorkFactory">The unit of work factory.</param>
        /// <param name="currentUser">The current logged-in user.</param>
        public TripParticipantsHandler(IUnitOfWorkFactory unitOfWorkFactory, ICurrentUser currentUser)
        {
            _unitOfWorkFactory = unitOfWorkFactory;
            _currentUser = currentUser;
        }

        /// <summary>Retrieves the participants of a trip.</summary>
        /// <param name="tripId">The trip identifier.</param>
        /// <returns>The participants on the trip.</returns>
        /// <exception cref="TravelAppNotFoundException">Thrown when the trip does not exist.</exception>
        /// <exception cref="TravelAppForbiddenException">Thrown when the current user cannot view the trip.</exception>
        public async Task<List<GetTripParticipantResponse>> GetTripParticipantsAsync(int tripId)
        {
            await using IUnitOfWork unitOfWork = _unitOfWorkFactory.Create();

            await TripAccessHelper.EnsureCanViewAsync(unitOfWork, tripId, _currentUser);

            List<TripParticipantDTO> participants = await unitOfWork.Trips.GetParticipantsInTripAsync(tripId);

            return participants.Select(participant => new GetTripParticipantResponse
            {
                Id = participant.Id,
                FullName = $"{participant.FirstName} {participant.LastName}"
            }).ToList();
        }
    }
}
