
using TravelApp.BusinessLogic.Auth;
using TravelApp.BusinessLogic.Interfaces;
using TravelApp.BusinessLogic.Models.Invitations;

namespace TravelApp.BusinessLogic.Handlers.Invitations.GetInvitations
{
    /// <summary>Handles retrieving pending invitations for the current user.</summary>
    public class GetPendingInvitationsHandler
    {
        private readonly IUnitOfWorkFactory _unitOfWorkFactory;
        private readonly ICurrentUser _currentUser;

        /// <summary>Initializes a new instance of the GetPendingInvitationsHandler class.</summary>
        public GetPendingInvitationsHandler(IUnitOfWorkFactory unitOfWorkFactory, ICurrentUser currentUser)
        {
            _unitOfWorkFactory = unitOfWorkFactory;
            _currentUser = currentUser;
        }

        /// <summary>Gets all pending invitations for the current user.</summary>
        public async Task<List<GetMyPendingInvitationResponse>> GetInvitesByReciever()
        {
            int userId = _currentUser.UserId;
            DateTime todayUtc = DateTime.UtcNow.Date;

            await using IUnitOfWork unitOfWork = _unitOfWorkFactory.Create();

            List<Invite> invitations = await unitOfWork.Invitations.GetPendingInvitesByReceiverAsync(userId, todayUtc);

            return invitations.Select(invite => new GetMyPendingInvitationResponse
            {
                Id = invite.Id,
                TripId = invite.Trip!.Id,
                SenderId = invite.Sender!.Id,
                SenderName = $"{invite.Sender.FirstName} {invite.Sender.LastName}",
                TripStartDate = invite.Trip.StartDate,
                IsAccepted = invite.IsAccepted,
                SendDate = invite.SendDate
            }).ToList();
        }
    }
}
