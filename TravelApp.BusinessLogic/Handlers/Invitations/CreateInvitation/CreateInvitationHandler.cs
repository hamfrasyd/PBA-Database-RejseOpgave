
using TravelApp.BusinessLogic.Auth;
using TravelApp.BusinessLogic.DTOs.Trips;
using TravelApp.BusinessLogic.Exceptions.SpecificCustomException;
using TravelApp.BusinessLogic.Interfaces;
using TravelApp.BusinessLogic.Models.Invitations;
using TravelApp.BusinessLogic.Models.Trips;

namespace TravelApp.BusinessLogic.Handlers.Invitations.SendInvitation
{
    /// <summary>Handles sending an invitation to a trip.</summary>
    public class CreateInvitationHandler
    {
        private readonly IUnitOfWorkFactory _unitOfWorkFactory;
        private readonly ICurrentUser _currentUser;

        /// <summary>Initializes a new instance of the CreateInvitationHandler class.</summary>
        public CreateInvitationHandler( IUnitOfWorkFactory unitOfWorkFactory, ICurrentUser currentUser)
        {
            _unitOfWorkFactory = unitOfWorkFactory;
            _currentUser = currentUser;
        }

        /// <summary>Sends a trip invitation to another user.</summary>
        public async Task<SendInvitationResponse> SendInvitationAsync(int tripId, int receiverId)
        {
            int senderId = _currentUser.UserId;
            if (senderId == receiverId)
            {
                throw new TravelAppValidationException("You cannot invite yourself.");
            }

            await using IUnitOfWork unitOfWork = _unitOfWorkFactory.Create();

            // Check that the trip exists.
            TripDetailsDTO? trip = await unitOfWork.Trips.GetDetailsAsync(tripId);
            if (trip is null)
            {
                throw new TravelAppNotFoundException($"Trip with ID {tripId} was not found.");
            }

            // Check the invitation deadline.
            DateTime todayUtc = DateTime.UtcNow.Date;
            // A trip without a start date can still receive invitations.
            // If a start date exists, the trip must not have started.
            if (trip.StartDate.HasValue && trip.StartDate.Value.Date <= todayUtc)
            {
                throw new TravelAppConflictException("You cannot invite users to a trip that has started.");
            }

            // Check that the sender may invite participants.
            TripAccessInfoDTO? senderAccess = await unitOfWork.Trips.GetAccessInfoAsync(tripId, senderId);
            bool senderIsParticipant = senderAccess?.IsParticipant == true;
            if (!senderIsParticipant && !_currentUser.IsAdmin)
            {
                throw new TravelAppForbiddenException("You cannot invite users to this trip.");
            }

            // Check that the receiver exists.
            bool receiverExists = await unitOfWork.Users.UserExistsAsync(receiverId);
            if (!receiverExists)
            {
                throw new TravelAppNotFoundException($"User with ID {receiverId} was not found.");
            }

            // Check that the receiver is not already participating.
            TripAccessInfoDTO? receiverAccess = await unitOfWork.Trips.GetAccessInfoAsync(tripId, receiverId);
            bool receiverIsParticipant = receiverAccess?.IsParticipant == true;
            if (receiverIsParticipant)
            {
                throw new TravelAppConflictException("The user is already a participant in this trip.");
            }

            // Prevent duplicate pending invitations.
            bool invitationExists = await unitOfWork.Invitations.PendingInviteExistsForTripAsync(tripId, receiverId);
            if (invitationExists)
            {
                throw new TravelAppConflictException("The user already has a pending invitation.");
            }

            // Create the invitation.
            Invite invite = new Invite
            {
                SendDate = DateTime.UtcNow,
                IsAccepted = null
            };

            await unitOfWork.Invitations.SendInviteAsync(invite, tripId, senderId,
                receiverId);

            await unitOfWork.CommitAsync();

            return new SendInvitationResponse
            {
                Id = invite.Id,
                TripId = tripId,
                ReceiverId = receiverId,
                SendDate = invite.SendDate
            };
        }
    }
}
