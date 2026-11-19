using TravelApp.BusinessLogic.Auth;
using TravelApp.BusinessLogic.DTOs.Trips;
using TravelApp.BusinessLogic.Enums;
using TravelApp.BusinessLogic.Exceptions.SpecificCustomException;
using TravelApp.BusinessLogic.Interfaces;
using TravelApp.BusinessLogic.Models.Invitations;
using TravelApp.BusinessLogic.Models.Trips;
using TravelApp.BusinessLogic.Models.Users;

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
        /// <exception cref="TravelAppNotFoundException">Thrown when the invitation, its receiver, its status, or its trip does not exist.</exception>
        /// <exception cref="TravelAppForbiddenException">Thrown when the user is not the receiver.</exception>
        /// <exception cref="TravelAppValidationException">Thrown when the decision is invalid.</exception>
        /// <exception cref="TravelAppConflictException">Thrown when the invitation is no longer active or the user already participates.</exception>
        /// <exception cref="InvalidOperationException">Thrown when Accepted, Declined, or Pending has no <see cref="InvitationStatusNameAttribute"/>.</exception>
        public async Task<RespondInvitationResponse> RespondInvitationAsync(int invitationId, RespondInvitationRequest request)
        {
            // Step 1: Read Accepted or Declined status from the receiver's decision.
            InvitationStatuses decisionStatus = Step1_ReadAcceptedOrDeclinedStatus(request.Decision.Trim());

            // Step 2: Load the invitation that the receiver is answering from DB.
            await using IUnitOfWork unitOfWork = _unitOfWorkFactory.Create();
            Invite invitationFromDb = await Step2_ReadInvitationAsync(unitOfWork, invitationId);

            // Step 3: Require the current user to be the invitation receiver.
            Step3_RequireCurrentUserIsReceiver(invitationFromDb, invitationId);

            // Step 4: Reject an invitation that is no longer pending.
            Step4_RequireInvitationIsStillPending(invitationFromDb, invitationId);

            // Step 5: Add the receiver to the trip when the decision is Accepted.
            int reciverId = invitationFromDb.Receiver!.Id;
            await Step5_AddReceiverToTripWhenAcceptedAsync(unitOfWork, invitationFromDb, reciverId, decisionStatus);

            // Step 6: Save the receiver's decision and commit the unit of work.
            string decisionStatusString = await Step6_SaveDecisionAsync(unitOfWork, invitationId, decisionStatus);

            RespondInvitationResponse response = new RespondInvitationResponse
            {
                Id = invitationFromDb.Id,
                Status = decisionStatusString
            };

            return response;
        }

        /// <summary>Reads Accepted or Declined from the receiver's decision.</summary>
        /// <param name="decision">The trimmed decision text.</param>
        /// <returns>Accepted or Declined.</returns>
        /// <exception cref="TravelAppValidationException">Thrown when the decision is neither Accepted nor Declined.</exception>
        /// <exception cref="InvalidOperationException">Thrown when Accepted or Declined has no <see cref="InvitationStatusNameAttribute"/>.</exception>
        private static InvitationStatuses Step1_ReadAcceptedOrDeclinedStatus(string decision)
        {
            bool decisionMatchesStatus = InvitationStatusEnumHelper.TryGetStatusFromString(decision, out InvitationStatuses decisionStatus);
            bool decisionIsAccepted = decisionMatchesStatus && decisionStatus == InvitationStatuses.Accepted;
            bool decisionIsDeclined = decisionMatchesStatus && decisionStatus == InvitationStatuses.Declined;
            if (decisionIsAccepted || decisionIsDeclined)
            {
                return decisionStatus;
            }

            string acceptedStatusString = RequiredStatusString(InvitationStatuses.Accepted);
            string declinedStatusString = RequiredStatusString(InvitationStatuses.Declined);
            throw new TravelAppValidationException($"The decision must be {acceptedStatusString} or {declinedStatusString}.");
        }

        /// <summary>Loads the invitation that the receiver is answering.</summary>
        /// <param name="unitOfWork">The unit of work for this response.</param>
        /// <param name="invitationId">The invitation identifier.</param>
        /// <returns>The invitation.</returns>
        /// <exception cref="TravelAppNotFoundException">Thrown when the invitation does not exist.</exception>
        private static async Task<Invite> Step2_ReadInvitationAsync(IUnitOfWork unitOfWork, int invitationId)
        {
            Invite? invitationFromDb = await unitOfWork.Invitations.GetByIdAsync(invitationId);
            if (invitationFromDb is null)
            {
                throw new TravelAppNotFoundException($"Invitation with ID {invitationId} was not found.");
            }

            return invitationFromDb;
        }

        /// <summary>Requires the current user to be the invitation receiver.</summary>
        /// <param name="invitationFromDb">The invitation loaded from the database.</param>
        /// <param name="invitationId">The invitation identifier.</param>
        /// <returns>The receiver's identifier.</returns>
        /// <exception cref="TravelAppNotFoundException">Thrown when the receiver is missing.</exception>
        /// <exception cref="TravelAppForbiddenException">Thrown when the user is not the receiver.</exception>
        private void Step3_RequireCurrentUserIsReceiver(Invite invitationFromDb, int invitationId)
        {
            TravelUser? receiver = invitationFromDb.Receiver;
            if (receiver is null)
            {
                throw new TravelAppNotFoundException($"Invitation with ID {invitationId} was not found.");
            }

            AccessRules.RequireCurrentUserIsInvitationReceiver(_currentUser, receiver.Id);
        }

        /// <summary>Rejects an invitation that is no longer pending.</summary>
        /// <param name="invitationFromDb">The invitation loaded from the database.</param>
        /// <param name="invitationId">The invitation identifier.</param>
        /// <exception cref="TravelAppNotFoundException">Thrown when the invitation status is missing.</exception>
        /// <exception cref="TravelAppConflictException">Thrown when the invitation is no longer pending.</exception>
        /// <exception cref="InvalidOperationException">Thrown when Pending has no <see cref="InvitationStatusNameAttribute"/>.</exception>
        private static void Step4_RequireInvitationIsStillPending(Invite invitationFromDb, int invitationId)
        {
            InvitationStatus? invitationStatus = invitationFromDb.InvitationStatus;
            if (invitationStatus is null)
            {
                throw new TravelAppNotFoundException($"Invitation with ID {invitationId} was not found.");
            }

            DateTime currentUtc = DateTime.UtcNow;
            string statusString = InvitationStatusEnumHelper.GetStringWhenPendingReaches14Days(invitationStatus.StatusName, invitationFromDb.SendDate, currentUtc);
            string pendingStatusString = RequiredStatusString(InvitationStatuses.Pending);
            bool invitationIsStillPending = statusString == pendingStatusString;
            if (invitationIsStillPending is false)
            {
                throw new TravelAppConflictException($"Invitation with ID {invitationId} is {statusString} and cannot be answered.");
            }
        }

        /// <summary>Adds the receiver to the trip when the decision is Accepted.</summary>
        /// <param name="unitOfWork">The unit of work for this response.</param>
        /// <param name="invitationFromDb">The invitation loaded from the database.</param>
        /// <param name="receiverId">The receiver's identifier.</param>
        /// <param name="decisionStatus">The receiver's decision.</param>
        /// <returns>A task that completes when an accepted receiver is staged as a participant.</returns>
        /// <exception cref="TravelAppNotFoundException">Thrown when the trip does not exist.</exception>
        /// <exception cref="TravelAppConflictException">Thrown when the receiver already participates.</exception>
        private static async Task Step5_AddReceiverToTripWhenAcceptedAsync(IUnitOfWork unitOfWork, Invite invitationFromDb, int receiverId, InvitationStatuses decisionStatus)
        {
            bool decisionIsAccepted = decisionStatus == InvitationStatuses.Accepted;
            if (decisionIsAccepted is false)
            {
                return;
            }

            Trip? trip = invitationFromDb.Trip;
            if (trip is null)
            {
                throw new TravelAppNotFoundException($"Invitation with ID {invitationFromDb.Id} was not found.");
            }

            int tripId = trip.Id;
            TripAccessInfoDTO? receiverAccess = await unitOfWork.Trips.GetAccessInfoAsync(tripId, receiverId);
            if (receiverAccess is null)
            {
                throw new TravelAppNotFoundException($"Trip with ID {tripId} was not found.");
            }

            if (receiverAccess.IsParticipant is true)
            {
                throw new TravelAppConflictException("You already participate in this trip.");
            }

            await unitOfWork.Trips.AddParticipantToTripAsync(tripId, receiverId);
        }

        /// <summary>Saves the receiver's decision and commits the unit of work.</summary>
        /// <param name="unitOfWork">The unit of work for this response.</param>
        /// <param name="invitationId">The invitation identifier.</param>
        /// <param name="decisionStatus">The receiver's decision.</param>
        /// <returns>The saved status string.</returns>
        /// <exception cref="InvalidOperationException">Thrown when the decision has no status string or the status row is missing from the database.</exception>
        /// <exception cref="TravelAppNotFoundException">Thrown when the invitation does not exist.</exception>
        private static async Task<string> Step6_SaveDecisionAsync(IUnitOfWork unitOfWork, int invitationId, InvitationStatuses decisionStatus)
        {
            string decisionStatusString = RequiredStatusString(decisionStatus);
            int? newStatusId = await unitOfWork.Invitations.GetStatusIdAsync(decisionStatusString);
            if (newStatusId is null)
            {
                throw new InvalidOperationException($"The {decisionStatusString} invitation status is missing from the database.");
            }

            bool invitationWasFound = await unitOfWork.Invitations.UpdateStatusAsync(invitationId, newStatusId.Value);
            if (invitationWasFound is false)
            {
                throw new TravelAppNotFoundException($"Invitation with ID {invitationId} was not found.");
            }

            // The new trip participant and invitation status use the same DbContext and SaveChanges.
            await unitOfWork.CommitAsync();
            return decisionStatusString;
        }

        /// <summary>Returns the status string for an invitation status.</summary>
        /// <param name="status">The invitation status.</param>
        /// <returns>The status string.</returns>
        /// <exception cref="InvalidOperationException">Thrown when the status has no <see cref="InvitationStatusNameAttribute"/>.</exception>
        private static string RequiredStatusString(InvitationStatuses status)
        {
            string? statusString = InvitationStatusEnumHelper.EnumToStringOrNull(status);
            if (statusString is null)
            {
                throw new InvalidOperationException($"Invitation status {status} does not define a status string.");
            }

            return statusString;
        }
    }
}
