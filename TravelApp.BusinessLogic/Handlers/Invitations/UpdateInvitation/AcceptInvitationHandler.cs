
using TravelApp.BusinessLogic.Auth;
using TravelApp.BusinessLogic.Exceptions.SpecificCustomException;
using TravelApp.BusinessLogic.Interfaces;
using TravelApp.BusinessLogic.Models.Invitations;

namespace TravelApp.BusinessLogic.Handlers.Invitations.AcceptInvitation
{
    /// <summary>Handles accepting an invitation to a trip.</summary>
    public class AcceptInvitationHandler
    {
        private readonly IUnitOfWorkFactory _unitOfWorkFactory;
        private readonly ICurrentUser _currentUser;

        /// <summary>Initializes a new AcceptInvitationHandler.</summary>
        public AcceptInvitationHandler(IUnitOfWorkFactory unitOfWorkFactory, ICurrentUser currentUser)
        {
            _unitOfWorkFactory = unitOfWorkFactory;
            _currentUser = currentUser;
        }

        /// <summary>Accepts a pending invitation for the current user.</summary>
        public async Task AcceptInvitationAsync(int invitationId)
        {
            int currentUserId = _currentUser.UserId;

            await using IUnitOfWork unitOfWork = _unitOfWorkFactory.Create();

            // Find the invitation.
            Invite? invite = await unitOfWork.Invitations.GetInviteByIdAsync(invitationId);
            if (invite is null)
            {
                throw new TravelAppNotFoundException("Invitation was not found.");
            }

            // Only the receiver can accept.
            if (invite.Receiver?.Id != currentUserId)
            {
                throw new TravelAppNotFoundException("Invitation was not found.");
            }

            // Only pending invitations can be accepted.
            if (invite.IsAccepted != null)
            {
                throw new TravelAppConflictException("The invitation has already been answered.");
            }

            // Verify that the trip has not started.
            if (invite.Trip is null)
            {
                throw new TravelAppNotFoundException("Trip was not found.");
            }

            DateTime todayUtc = DateTime.UtcNow.Date;
            if (invite.Trip.StartDate.HasValue && invite.Trip.StartDate.Value.Date <= todayUtc)
            {
                throw new TravelAppConflictException("The invitation has expired.");
            }

            int tripId = invite.Trip.Id;

            // Ensure the user is not already a participant.
            bool alreadyParticipant = await unitOfWork.Invitations.IsParticipantAsync(tripId, currentUserId);
            if (alreadyParticipant)
            {
                throw new TravelAppConflictException( "You are already a participant in this trip.");
            }

            // Add the receiver to the trip.
            await unitOfWork.Trips.AddParticipantToTripAsync(tripId, currentUserId);

            // Mark the invitation as accepted.
            invite.IsAccepted = true;
            await unitOfWork.Invitations.UpdateInviteAsync(invite);

            // Save both changes together.
            await unitOfWork.CommitAsync();
        }
    }
}
