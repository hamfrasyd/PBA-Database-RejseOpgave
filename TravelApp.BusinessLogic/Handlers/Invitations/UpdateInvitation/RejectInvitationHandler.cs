
using TravelApp.BusinessLogic.Auth;
using TravelApp.BusinessLogic.Exceptions.SpecificCustomException;
using TravelApp.BusinessLogic.Interfaces;
using TravelApp.BusinessLogic.Models.Invitations;

namespace TravelApp.BusinessLogic.Handlers.Invitations.RejectInvitation
{
    /// <summary>Handles rejecting an invitation to a trip.</summary>
    public class RejectInvitationHandler
    {
        private readonly IUnitOfWorkFactory _unitOfWorkFactory;
        private readonly ICurrentUser _currentUser;

        /// <summary>Initializes a new RejectInvitationHandler instance.</summary>
        public RejectInvitationHandler(IUnitOfWorkFactory unitOfWorkFactory, ICurrentUser currentUser)
        {
            _unitOfWorkFactory = unitOfWorkFactory;
            _currentUser = currentUser;
        }

        /// <summary>Rejects a pending invitation for the current user.</summary>
        public async Task RejectInvitationAsync(int invitationId)
        {
            int currentUserId = _currentUser.UserId;

            await using IUnitOfWork unitOfWork = _unitOfWorkFactory.Create();

            Invite? invite = await unitOfWork.Invitations.GetInviteByIdAsync(invitationId);
            if (invite is null)
            {
                throw new TravelAppNotFoundException("Invitation was not found.");
            }
            // Only the receiver can reject the invitation.
            if (invite.Receiver?.Id != currentUserId)
            {
                throw new TravelAppNotFoundException("Invitation was not found.");
            }

            // Only pending invitations can be rejected.
            if (invite.IsAccepted != null)
            {
                throw new TravelAppConflictException("The invitation has already been answered.");
            }

            // Check that the trip has not started.
            if (invite.Trip is null)
            {
                throw new TravelAppNotFoundException("Trip was not found.");
            }

            if (invite.Trip.StartDate.HasValue && invite.Trip.StartDate.Value.Date <= DateTime.UtcNow.Date)
            {
                throw new TravelAppConflictException("The invitation has expired.");
            }

            // Mark the invitation as rejected.
            invite.IsAccepted = false;

            await unitOfWork.Invitations.UpdateInviteAsync(invite);

            await unitOfWork.CommitAsync();
        }
    }
}
