using TravelApp.BusinessLogic.Auth;
using TravelApp.BusinessLogic.DTOs.Trips;
using TravelApp.BusinessLogic.Exceptions.SpecificCustomException;
using TravelApp.BusinessLogic.Handlers.Trips.ManageTripRelations;
using TravelApp.BusinessLogic.Interfaces;
using TravelApp.BusinessLogic.Models.Users;

namespace TravelApp.BusinessLogic.Handlers.Trips.AddTripParticipant
{
    /// <summary>Handles the add trip participant use case.</summary>
    public class AddTripParticipantHandler
    {
        private readonly IUnitOfWorkFactory _unitOfWorkFactory;
        private readonly ICurrentUser _currentUser;

        /// <summary>Initializes a new instance of the <see cref="AddTripParticipantHandler"/> class.</summary>
        /// <param name="unitOfWorkFactory">The unit of work factory.</param>
        /// <param name="currentUser">The current logged-in user.</param>
        public AddTripParticipantHandler(IUnitOfWorkFactory unitOfWorkFactory, ICurrentUser currentUser)
        {
            _unitOfWorkFactory = unitOfWorkFactory;
            _currentUser = currentUser;
        }

        /// <summary>Adds a participant to a trip.</summary>
        /// <param name="tripId">The trip identifier.</param>
        /// <param name="userId">The user identifier.</param>
        /// <exception cref="TravelAppNotFoundException">Thrown when the trip or user does not exist.</exception>
        /// <exception cref="TravelAppForbiddenException">Thrown when the current user cannot modify the trip.</exception>
        /// <exception cref="TravelAppConflictException">Thrown when the user is already participating.</exception>
        public async Task AddTripParticipantAsync(int tripId, int userId)
        {
            await using IUnitOfWork unitOfWork = _unitOfWorkFactory.Create();

            await TripAccessHelper.EnsureCanModifyAsync(unitOfWork, tripId, _currentUser);

            TravelUser? user = await unitOfWork.Users.GetByIdAsync(userId);
            if (user is null)
            {
                throw new TravelAppNotFoundException($"User {userId} was not found.");
            }

            TripRelationsDTO? relations = await unitOfWork.Trips.GetRelationsAsync(tripId);
            if (relations is null)
            {
                throw new TravelAppNotFoundException($"Trip {tripId} was not found.");
            }

            bool userIsParticipating = relations.ParticipantIds.Contains(userId);
            if (userIsParticipating)
            {
                throw new TravelAppConflictException("User is already participating.");
            }

            await unitOfWork.Trips.AddParticipantToTripAsync(tripId, userId);

            await unitOfWork.CommitAsync();
        }
    }
}
