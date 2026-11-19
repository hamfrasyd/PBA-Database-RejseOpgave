using TravelApp.BusinessLogic.Auth;
using TravelApp.BusinessLogic.DTOs.Trips;
using TravelApp.BusinessLogic.Enums;
using TravelApp.BusinessLogic.Exceptions.SpecificCustomException;
using TravelApp.BusinessLogic.Interfaces;
using TravelApp.BusinessLogic.Models.Invitations;

namespace TravelApp.BusinessLogic.Handlers.Invitations.RespondInvitation
{
    /// <summary>Handles the respond to invitation use case.</summary>
    public class RespondInvitationHandler
    {
        private readonly IUnitOfWorkFactory _unitOfWorkFactory;
        private readonly ICurrentUser _currentUser;

        /// <summary>Initializes a new RespondInvitationHandler instance.</summary>
        /// <param name="unitOfWorkFactory">The Unit of Work factory.</param>
        /// <param name="currentUser">The current logged-in user.</param>
        public RespondInvitationHandler(IUnitOfWorkFactory unitOfWorkFactory, ICurrentUser currentUser)
        {
            _unitOfWorkFactory = unitOfWorkFactory;
            _currentUser = currentUser;
        }

        /// <summary>Accepts or declines a pending invitation for its receiver.</summary>
        /// <param name="invitationId">The invitation identifier.</param>
        /// <param name="request">The receiver's decision.</param>
        /// <returns>The updated invitation status.</returns>
        /// <exception cref="TravelAppNotFoundException">Thrown when the invitation does not exist.</exception>
        /// <exception cref="TravelAppForbiddenException">Thrown when the user is not the receiver.</exception>
        /// <exception cref="TravelAppValidationException">Thrown when the decision is invalid.</exception>
        /// <exception cref="TravelAppConflictException">Thrown when the invitation is no longer active or the user already participates.</exception>
        /// <exception cref="InvalidOperationException">Thrown when Accepted, Declined, or Pending has no <see cref="InvitationStatusNameAttribute"/>.</exception>
        public async Task<RespondInvitationResponse> RespondInvitationAsync(int invitationId, RespondInvitationRequest request)
        {
            string decision = request.Decision.Trim();
            bool decisionWasKnown = InvitationStatusEnumHelper.TryGetStatusFromString(decision, out InvitationStatuses decisionStatus);
            bool isAccepted = decisionWasKnown && decisionStatus == InvitationStatuses.Accepted;
            bool isDeclined = decisionWasKnown && decisionStatus == InvitationStatuses.Declined;
            if (isAccepted is false && isDeclined is false)
            {
                string? acceptedStatusName = InvitationStatusEnumHelper.EnumToStringOrNull(InvitationStatuses.Accepted);
                string? declinedStatusName = InvitationStatusEnumHelper.EnumToStringOrNull(InvitationStatuses.Declined);
                if (acceptedStatusName is null)
                {
                    throw new InvalidOperationException($"Invitation status {InvitationStatuses.Accepted} does not define a status string.");
                }

                if (declinedStatusName is null)
                {
                    throw new InvalidOperationException($"Invitation status {InvitationStatuses.Declined} does not define a status string.");
                }
                throw new TravelAppValidationException($"The decision must be {acceptedStatusName} or {declinedStatusName}.");
            }

            await using IUnitOfWork unitOfWork = _unitOfWorkFactory.Create();

            Invite? invitationFromDb = await unitOfWork.Invitations.GetByIdAsync(invitationId);
            if (invitationFromDb is null)
            {
                throw new TravelAppNotFoundException($"Invitation with ID {invitationId} was not found.");
            }

            int receiverId = invitationFromDb.Receiver!.Id;
            AccessRules.RequireCurrentUserIsInvitationReceiver(_currentUser, receiverId);

            DateTime currentUtc = DateTime.UtcNow;
            string statusName = InvitationStatusEnumHelper.GetStringWhenPendingReaches14Days(invitationFromDb.InvitationStatus!.StatusName, invitationFromDb.SendDate, currentUtc);
            string? pendingStatusName = InvitationStatusEnumHelper.EnumToStringOrNull(InvitationStatuses.Pending);
            if (pendingStatusName is null)
            {
                throw new InvalidOperationException($"Invitation status {InvitationStatuses.Pending} does not define a status string.");
            }
            if (statusName != pendingStatusName)
            {
                throw new TravelAppConflictException($"Invitation with ID {invitationId} is {statusName} and cannot be answered.");
            }

            if (isAccepted is true)
            {
                int tripId = invitationFromDb.Trip!.Id;
                TripAccessInfoDTO? receiverAccess = await unitOfWork.Trips.GetAccessInfoAsync(tripId, receiverId);
                if (receiverAccess is null)
                {
                    throw new TravelAppNotFoundException($"Trip with ID {tripId} was not found.");
                }

                if (receiverAccess.IsParticipant is true)
                {
                    throw new TravelAppConflictException("You already participate in this trip.");
                }

                // The new trip participant and invitation status use the same DbContext and SaveChanges.
                await unitOfWork.Trips.AddParticipantToTripAsync(tripId, receiverId);
            }

            string? decisionName = InvitationStatusEnumHelper.EnumToStringOrNull(decisionStatus);
            if (decisionName is null)
            {
                throw new InvalidOperationException($"Invitation status {decisionStatus} does not define a status string.");
            }
            int? newStatusId = await unitOfWork.Invitations.GetStatusIdAsync(decisionName);
            if (newStatusId is null)
            {
                throw new InvalidOperationException($"The {decisionName} invitation status is missing from the database.");
            }

            bool invitationWasFound = await unitOfWork.Invitations.UpdateStatusAsync(invitationId, newStatusId.Value);
            if (invitationWasFound is false)
            {
                throw new TravelAppNotFoundException($"Invitation with ID {invitationId} was not found.");
            }

            await unitOfWork.CommitAsync();

            RespondInvitationResponse response = new RespondInvitationResponse
            {
                Id = invitationFromDb.Id,
                Status = decisionName
            };

            return response;
        }
    }
}
