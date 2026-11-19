using TravelApp.BusinessLogic.Auth;
using TravelApp.BusinessLogic.DTOs.Trips;
using TravelApp.BusinessLogic.Exceptions.SpecificCustomException;
using TravelApp.BusinessLogic.Handlers.Trips.ManageTripRelations;
using TravelApp.BusinessLogic.Interfaces;

namespace TravelApp.BusinessLogic.Handlers.Trips.RemoveTripParticipant
{
    /// <summary>Handles the remove trip participant use case.</summary>
    public class RemoveTripParticipantHandler
    {
        private readonly IUnitOfWorkFactory _unitOfWorkFactory;
        private readonly ICurrentUser _currentUser;

        /// <summary>Initializes a new instance of the <see cref="RemoveTripParticipantHandler"/> class.</summary>
        /// <param name="unitOfWorkFactory">The unit of work factory.</param>
        /// <param name="currentUser">The current logged-in user.</param>
        public RemoveTripParticipantHandler(IUnitOfWorkFactory unitOfWorkFactory, ICurrentUser currentUser)
        {
            _unitOfWorkFactory = unitOfWorkFactory;
            _currentUser = currentUser;
        }

        /// <summary>Removes a participant from a trip.</summary>
        /// <param name="tripId">The trip identifier.</param>
        /// <param name="userId">The user identifier.</param>
        /// <exception cref="TravelAppNotFoundException">Thrown when the trip does not exist or the user is not participating.</exception>
        /// <exception cref="TravelAppForbiddenException">Thrown when the current user cannot modify the trip.</exception>
        /// <exception cref="TravelAppConflictException">Thrown when the trip would have no participants left.</exception>
        public async Task RemoveTripParticipantAsync(int tripId, int userId)
        {
            await using IUnitOfWork unitOfWork = _unitOfWorkFactory.Create();

            await TripAccessHelper.EnsureCanModifyAsync(unitOfWork, tripId, _currentUser);

            TripRelationsDTO? relations = await unitOfWork.Trips.GetRelationsAsync(tripId);
            if (relations is null)
            {
                throw new TravelAppNotFoundException($"Trip {tripId} was not found.");
            }

            bool userIsParticipating = relations.ParticipantIds.Contains(userId);
            if (userIsParticipating is false)
            {
                throw new TravelAppNotFoundException("User is not participating in the trip.");
            }

            bool participantIsTheLastOne = relations.ParticipantIds.Count == 1;
            if (participantIsTheLastOne)
            {
                throw new TravelAppConflictException("A trip must have at least one participant.");
            }

            await unitOfWork.Trips.RemoveParticipantFromTripAsync(tripId, userId);
            await unitOfWork.CommitAsync();
        }
    }
}
