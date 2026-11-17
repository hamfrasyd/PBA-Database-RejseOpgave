using TravelApp.BusinessLogic.Auth;
using TravelApp.BusinessLogic.DTOs.Trips;
using TravelApp.BusinessLogic.Exceptions.SpecificCustomException;
using TravelApp.BusinessLogic.Handlers.Trips.TripParticipants;
using TravelApp.BusinessLogic.Interfaces;
using TravelApp.BusinessLogic.Models.Trips;
using TravelApp.BusinessLogic.Models.Users;

namespace TravelApp.BusinessLogic.Handlers.Trips
{
    /// <summary>Handler for managing trip participants.</summary>
    public class TripParticipantsHandler
    {
        private readonly IUnitOfWorkFactory _unitOfWorkFactory;
        private readonly ICurrentUser _currentUser;

        /// <summary>Initializes a new instance of the <see cref="TripParticipantsHandler"/> class.</summary>
        public TripParticipantsHandler(IUnitOfWorkFactory unitOfWorkFactory, ICurrentUser currentUser)
        {
            _unitOfWorkFactory = unitOfWorkFactory;
            _currentUser = currentUser;
        }

        /// <summary>Retrieves the participants of a specific trip.</summary>
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

        /// <summary>Adds a participant to a trip.</summary>
        public async Task AddParticipantAsync(int tripId, int userId)
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

            bool exists = relations.ParticipantIds.Contains(userId);
            if (exists)
            {
                throw new TravelAppConflictException("User is already participating.");
            }

            await unitOfWork.Trips.AddParticipantToTripAsync(tripId, userId);

            await unitOfWork.CommitAsync();
        }

        /// <summary>Removes a participant from a trip.</summary>
        public async Task RemoveParticipantAsync(int tripId, int userId)
        {
            await using IUnitOfWork unitOfWork = _unitOfWorkFactory.Create();

            await TripAccessHelper.EnsureCanModifyAsync(unitOfWork, tripId, _currentUser);

            TripRelationsDTO? relations = await unitOfWork.Trips.GetRelationsAsync(tripId);
            if (relations is null)
            {
                throw new TravelAppNotFoundException($"Trip {tripId} was not found.");
            }

            bool exists = relations.ParticipantIds.Contains(userId);
            if (!exists)
            {
                throw new TravelAppNotFoundException("User is not participating in the trip.");
            }

            if (relations.ParticipantIds.Count == 1)
            {
                throw new TravelAppConflictException("A trip must have at least one participant.");
            }

            await unitOfWork.Trips.RemoveParticipantFromTripAsync(tripId, userId);
            await unitOfWork.CommitAsync();
        }
    }
}