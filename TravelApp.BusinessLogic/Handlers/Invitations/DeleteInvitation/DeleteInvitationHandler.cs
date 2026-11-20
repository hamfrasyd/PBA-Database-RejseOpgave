
using TravelApp.BusinessLogic.Auth;
using TravelApp.BusinessLogic.Exceptions.SpecificCustomException;
using TravelApp.BusinessLogic.Interfaces;
using TravelApp.BusinessLogic.Models.Invitations;

namespace TravelApp.BusinessLogic.Handlers.Invitations.DeleteInvitation
{
    /// <summary>Handles cancelling a pending invitation.</summary>
    public class DeleteInvitationHandler
    {
        private readonly IUnitOfWorkFactory _unitOfWorkFactory;
        private readonly ICurrentUser _currentUser;

        /// <summary>Initializes a new DeleteInvitationHandler instance.</summary>
        public DeleteInvitationHandler(IUnitOfWorkFactory unitOfWorkFactory, ICurrentUser currentUser)
        {
            _unitOfWorkFactory = unitOfWorkFactory;
            _currentUser = currentUser;
        }

        /// <summary>Deletes a pending invitation sent by the current user.</summary>
        public async Task DeleteInvitationAsync(int invitationId)
        {
            int currentUserId = _currentUser.UserId;

            await using IUnitOfWork unitOfWork = _unitOfWorkFactory.Create();

            Invite? invite = await unitOfWork.Invitations.GetInviteByIdAsync(invitationId);
            if (invite is null)
            {
                throw new TravelAppNotFoundException("Invitation was not found.");
            }

            // Only the sender can cancel an invitation.
            if (invite.Sender?.Id != currentUserId)
            {
                throw new TravelAppNotFoundException("Invitation was not found.");
            }

            // Only pending invitations can be cancelled.
            if (invite.IsAccepted != null)
            {
                throw new TravelAppConflictException("Only pending invitations can be cancelled.");
            }

            await unitOfWork.Invitations.DeleteInviteAsync(invite);

            await unitOfWork.CommitAsync();
        }
    }
}
