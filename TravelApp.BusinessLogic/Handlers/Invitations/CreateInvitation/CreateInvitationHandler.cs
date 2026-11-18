using TravelApp.BusinessLogic.Auth;
using TravelApp.BusinessLogic.DTOs.Trips;
using TravelApp.BusinessLogic.Exceptions.SpecificCustomException;
using TravelApp.BusinessLogic.Interfaces;
using TravelApp.BusinessLogic.Models.Invitations;

namespace TravelApp.BusinessLogic.Handlers.Invitations.CreateInvitation
{
    /// <summary>Handles the create invitation use case.</summary>
    public class CreateInvitationHandler
    {
        private readonly IUnitOfWorkFactory _unitOfWorkFactory;
        private readonly ICurrentUser _currentUser;

        /// <summary>Initializes a new CreateInvitationHandler instance.</summary>
        /// <param name="unitOfWorkFactory">The Unit of Work factory.</param>
        /// <param name="currentUser">The current logged-in user.</param>
        public CreateInvitationHandler(IUnitOfWorkFactory unitOfWorkFactory, ICurrentUser currentUser)
        {
            _unitOfWorkFactory = unitOfWorkFactory;
            _currentUser = currentUser;
        }

        /// <summary>Invites another user to a trip that the sender may manage.</summary>
        /// <param name="request">The trip and receiver identifiers.</param>
        /// <returns>The newly created invitation.</returns>
        /// <exception cref="TravelAppNotFoundException">Thrown when the trip does not exist.</exception>
        /// <exception cref="TravelAppForbiddenException">Thrown when the sender does not have trip invitation access.</exception>
        /// <exception cref="TravelAppValidationException">Thrown when the receiver is invalid or missing.</exception>
        /// <exception cref="TravelAppConflictException">Thrown when the receiver is already participating or already has an active invitation.</exception>
        public async Task<CreateInvitationResponse> CreateInvitationAsync(CreateInvitationRequest request)
        {
            int senderId = _currentUser.UserId;
            if (senderId == request.ReceiverId)
            {
                throw new TravelAppValidationException("You cannot invite yourself to a trip.");
            }

            await using IUnitOfWork unitOfWork = _unitOfWorkFactory.Create();

            TripAccessInfoDTO? senderAccess = await unitOfWork.Trips.GetAccessInfoAsync(request.TripId, senderId);
            if (senderAccess is null)
            {
                throw new TravelAppNotFoundException($"Trip with ID {request.TripId} was not found.");
            }

            AccessRules.RequireCurrentUserIsTripParticipantOrAdmin(_currentUser, senderAccess.IsParticipant);

            bool receiverExists = await unitOfWork.Users.UserExistsAsync(request.ReceiverId);
            if (receiverExists is false)
            {
                throw new TravelAppValidationException($"User with ID {request.ReceiverId} does not exist.");
            }

            TripAccessInfoDTO? receiverAccess = await unitOfWork.Trips.GetAccessInfoAsync(request.TripId, request.ReceiverId);
            if (receiverAccess is not null && receiverAccess.IsParticipant)
            {
                throw new TravelAppConflictException("The user already participates in this trip.");
            }

            DateTime sendDateUtc = DateTime.UtcNow;
            bool activePendingExists = await unitOfWork.Invitations.ActivePendingExistsAsync(request.TripId, request.ReceiverId, sendDateUtc.AddDays(-14));
            if (activePendingExists is true)
            {
                throw new TravelAppConflictException("The user already has an active invitation to this trip.");
            }

            int? pendingStatusId = await unitOfWork.Invitations.GetStatusIdAsync("Pending");
            if (pendingStatusId is null)
            {
                throw new InvalidOperationException("The Pending invitation status is missing from the database.");
            }

            Invite invitation = new Invite
            {
                SendDate = sendDateUtc
            };

            await unitOfWork.Invitations.AddAsync(invitation, request.TripId, senderId, request.ReceiverId, pendingStatusId.Value);
            await unitOfWork.CommitAsync();

            CreateInvitationResponse response = new CreateInvitationResponse
            {
                Id = invitation.Id,
                TripId = request.TripId,
                SenderId = senderId,
                ReceiverId = request.ReceiverId,
                Status = "Pending",
                SendDate = invitation.SendDate
            };

            return response;
        }
    }
}
