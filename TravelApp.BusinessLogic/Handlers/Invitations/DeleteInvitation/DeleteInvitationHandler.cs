using TravelApp.BusinessLogic.Auth;
using TravelApp.BusinessLogic.Exceptions.SpecificCustomException;
using TravelApp.BusinessLogic.Enums;
using TravelApp.BusinessLogic.Interfaces;
using TravelApp.BusinessLogic.Models.Invitations;

namespace TravelApp.BusinessLogic.Handlers.Invitations.DeleteInvitation
{
    /// <summary>Handles the delete invitation use case.</summary>
    public class DeleteInvitationHandler
    {
        private readonly IUnitOfWorkFactory _unitOfWorkFactory;
        private readonly ICurrentUser _currentUser;

        /// <summary>Initializes a new DeleteInvitationHandler instance.</summary>
        /// <param name="unitOfWorkFactory">The Unit of Work factory.</param>
        /// <param name="currentUser">The current logged-in user.</param>
        public DeleteInvitationHandler(IUnitOfWorkFactory unitOfWorkFactory, ICurrentUser currentUser)
        {
            _unitOfWorkFactory = unitOfWorkFactory;
            _currentUser = currentUser;
        }

        /// <summary>Cancels an invitation for its sender or an administrator.</summary>
        /// <param name="invitationId">The invitation identifier.</param>
        /// <returns>A task that completes when the invitation is deleted.</returns>
        /// <exception cref="TravelAppNotFoundException">Thrown when the invitation does not exist or its status is missing.</exception>
        /// <exception cref="TravelAppForbiddenException">Thrown when the current user cannot cancel the invitation.</exception>
        /// <exception cref="TravelAppConflictException">Thrown when an accepted invitation cannot be cancelled.</exception>
        /// <exception cref="InvalidOperationException">Thrown when Accepted has no <see cref="InvitationStatusNameAttribute"/>.</exception>
        public async Task DeleteInvitationAsync(int invitationId)
        {
            await using IUnitOfWork unitOfWork = _unitOfWorkFactory.Create();

            Invite? invitationFromDb = await unitOfWork.Invitations.GetByIdAsync(invitationId);
            if (invitationFromDb is null)
            {
                throw new TravelAppNotFoundException($"Invitation with ID {invitationId} was not found.");
            }

            AccessRules.RequireCurrentUserIsInvitationSenderOrAdmin(_currentUser, invitationFromDb.Sender!.Id);

            string? acceptedEnumStatusName = InvitationStatusEnumHelper.EnumToStringOrNull(InvitationStatuses.Accepted);
            if (acceptedEnumStatusName is null)
            {
                throw new InvalidOperationException($"Invitation status {InvitationStatuses.Accepted} does not define a status string.");
            }

            InvitationStatus? invitationStatus = invitationFromDb.InvitationStatus;
            if (invitationStatus is null)
            {   
                throw new TravelAppNotFoundException($"Invitation with ID {invitationId} was not found.");
            }

            bool invitationIsAccepted = invitationStatus.StatusName == acceptedEnumStatusName;
            if (invitationIsAccepted)
            {
                throw new TravelAppConflictException("Accepted invitations cannot be cancelled because the user already joined the trip.");
            }

            bool invitationWasFound = await unitOfWork.Invitations.DeleteAsync(invitationId);
            if (invitationWasFound is false)
            {
                throw new TravelAppNotFoundException($"Invitation with ID {invitationId} was not found.");
            }

            await unitOfWork.CommitAsync();
        }
    }
}
