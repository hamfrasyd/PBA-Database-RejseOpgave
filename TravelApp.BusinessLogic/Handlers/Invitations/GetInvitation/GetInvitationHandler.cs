using TravelApp.BusinessLogic.Auth;
using TravelApp.BusinessLogic.Exceptions.SpecificCustomException;
using TravelApp.BusinessLogic.Interfaces;
using TravelApp.BusinessLogic.Models.Invitations;

namespace TravelApp.BusinessLogic.Handlers.Invitations.GetInvitation
{
    /// <summary>Handles the get invitation use case.</summary>
    public class GetInvitationHandler
    {
        private readonly IUnitOfWorkFactory _unitOfWorkFactory;
        private readonly ICurrentUser _currentUser;

        /// <summary>Initializes a new GetInvitationHandler instance.</summary>
        /// <param name="unitOfWorkFactory">The Unit of Work factory.</param>
        /// <param name="currentUser">The current logged-in user.</param>
        public GetInvitationHandler(IUnitOfWorkFactory unitOfWorkFactory, ICurrentUser currentUser)
        {
            _unitOfWorkFactory = unitOfWorkFactory;
            _currentUser = currentUser;
        }

        /// <summary>Retrieves an invitation visible to the current user.</summary>
        /// <param name="invitationId">The invitation identifier.</param>
        /// <returns>The invitation details.</returns>
        /// <exception cref="TravelAppNotFoundException">Thrown when the invitation does not exist.</exception>
        /// <exception cref="TravelAppForbiddenException">Thrown when the current user cannot view the invitation.</exception>
        public async Task<GetInvitationResponse> GetInvitationAsync(int invitationId)
        {
            await using IUnitOfWork unitOfWork = _unitOfWorkFactory.Create();

            Invite? invitationFromDb = await unitOfWork.Invitations.GetByIdAsync(invitationId);
            if (invitationFromDb is null)
            {
                throw new TravelAppNotFoundException($"Invitation with ID {invitationId} was not found.");
            }

            int senderId = invitationFromDb.Sender!.Id;
            int receiverId = invitationFromDb.Receiver!.Id;
            AccessRules.RequireCurrentUserIsInvitationParticipantOrAdmin(_currentUser, senderId, receiverId);

            GetInvitationResponse response = new GetInvitationResponse
            {
                Id = invitationFromDb.Id,
                TripId = invitationFromDb.Trip!.Id,
                SenderId = senderId,
                ReceiverId = receiverId,
                Status = InvitationStatusRules.GetEffectiveStatus(invitationFromDb.InvitationStatus!.StatusName, invitationFromDb.SendDate, DateTime.UtcNow),
                SendDate = invitationFromDb.SendDate,
                ExpiresAt = invitationFromDb.SendDate.AddDays(14)
            };

            return response;
        }
    }
}
