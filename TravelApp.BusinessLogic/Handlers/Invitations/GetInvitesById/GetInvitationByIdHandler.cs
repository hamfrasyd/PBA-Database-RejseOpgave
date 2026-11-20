using TravelApp.BusinessLogic.Auth;
using TravelApp.BusinessLogic.Exceptions.SpecificCustomException;
using TravelApp.BusinessLogic.Interfaces;
using TravelApp.BusinessLogic.Models.Invitations;

namespace TravelApp.BusinessLogic.Handlers.Invitations.GetInvitesById
{
    /// <summary>Handles retrieving a specific invitation.</summary>
    public class GetInvitationByIdHandler
    {
        private readonly IUnitOfWorkFactory _unitOfWorkFactory;
        private readonly ICurrentUser _currentUser;

        /// <summary>Initializes a new instance of the GetInvitationByIdHandler class.</summary>
        public GetInvitationByIdHandler(IUnitOfWorkFactory unitOfWorkFactory, ICurrentUser currentUser)
        {
            _unitOfWorkFactory = unitOfWorkFactory;
            _currentUser = currentUser;
        }

        /// <summary>Gets an invitation by its identifier.</summary>
        public async Task<GetInvitationResponse> GetInviteById(int invitationId)
        {
            await using IUnitOfWork unitOfWork = _unitOfWorkFactory.Create();

            Invite? invite = await unitOfWork.Invitations.GetInviteByIdAsync(invitationId);
            if (invite is null)
            {
                throw new TravelAppNotFoundException("Invitation was not found.");
            }

            int currentUserId = _currentUser.UserId;
            bool isReceiver = invite.Receiver?.Id == currentUserId;
            bool isSender = invite.Sender?.Id == currentUserId;
            if (!isReceiver && !isSender)
            {
                throw new TravelAppNotFoundException("Invitation was not found.");
            }

            return new GetInvitationResponse
            {
                Id = invite.Id,
                TripId = invite.Trip!.Id,
                SenderId = invite.Sender!.Id,
                SenderName = $"{invite.Sender.FirstName} {invite.Sender.LastName}",
                ReceiverId = invite.Receiver!.Id,
                ReceiverName = $"{invite.Receiver.FirstName} {invite.Receiver.LastName}",
                TripStartDate = invite.Trip.StartDate,
                IsAccepted = invite.IsAccepted,
                SendDate = invite.SendDate
            };
        }
    }
}
