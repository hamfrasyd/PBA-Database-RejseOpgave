using TravelApp.BusinessLogic.Auth;
using TravelApp.BusinessLogic.DTOs.Invitations;
using TravelApp.BusinessLogic.Interfaces;

namespace TravelApp.BusinessLogic.Handlers.Invitations.GetInvitations
{
    /// <summary>Handles the get invitations use case.</summary>
    public class GetInvitationsHandler
    {
        private readonly IUnitOfWorkFactory _unitOfWorkFactory;
        private readonly ICurrentUser _currentUser;

        /// <summary>Initializes a new GetInvitationsHandler instance.</summary>
        /// <param name="unitOfWorkFactory">The Unit of Work factory.</param>
        /// <param name="currentUser">The current logged-in user.</param>
        public GetInvitationsHandler(IUnitOfWorkFactory unitOfWorkFactory, ICurrentUser currentUser)
        {
            _unitOfWorkFactory = unitOfWorkFactory;
            _currentUser = currentUser;
        }

        /// <summary>Retrieves invitations sent or received by the current user.</summary>
        /// <param name="sentByCurrentUser">Whether to retrieve sent rather than received invitations.</param>
        /// <returns>The current user's invitations.</returns>
        public async Task<List<GetInvitationsResponse>> GetInvitationsAsync(bool sentByCurrentUser)
        {
            await using IUnitOfWork unitOfWork = _unitOfWorkFactory.Create();

            List<InvitationDTO> invitationsFromDb = await unitOfWork.Invitations.GetForUserAsync(_currentUser.UserId, sentByCurrentUser);
            DateTime currentUtc = DateTime.UtcNow;

            List<GetInvitationsResponse> response = invitationsFromDb.Select(invitation => new GetInvitationsResponse
            {
                Id = invitation.Id,
                TripId = invitation.TripId,
                SenderId = invitation.SenderId,
                SenderName = invitation.SenderName,
                ReceiverId = invitation.ReceiverId,
                ReceiverName = invitation.ReceiverName,
                Status = InvitationStatusRules.GetEffectiveStatus(invitation.Status, invitation.SendDate, currentUtc),
                SendDate = invitation.SendDate,
                ExpiresAt = invitation.SendDate.AddDays(14)
            }).ToList();

            return response;
        }
    }
}
