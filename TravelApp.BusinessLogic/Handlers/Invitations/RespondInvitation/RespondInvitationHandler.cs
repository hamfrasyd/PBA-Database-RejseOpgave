using TravelApp.BusinessLogic.Auth;
using TravelApp.BusinessLogic.DTOs.Trips;
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
        public async Task<RespondInvitationResponse> RespondInvitationAsync(int invitationId, RespondInvitationRequest request)
        {
            string decision = request.Decision.Trim();
            bool isAccepted = decision == "Accepted";
            bool isDeclined = decision == "Declined";
            if (isAccepted is false && isDeclined is false)
            {
                throw new TravelAppValidationException("The decision must be Accepted or Declined.");
            }

            await using IUnitOfWork unitOfWork = _unitOfWorkFactory.Create();

            Invite? invitationFromDb = await unitOfWork.Invitations.GetByIdAsync(invitationId);
            if (invitationFromDb is null)
            {
                throw new TravelAppNotFoundException($"Invitation with ID {invitationId} was not found.");
            }

            int receiverId = invitationFromDb.Receiver!.Id;
            AccessRules.RequireCurrentUserIsInvitationReceiver(_currentUser, receiverId);

            string statusName = InvitationStatusRules.GetEffectiveStatus(invitationFromDb.InvitationStatus!.StatusName, invitationFromDb.SendDate, DateTime.UtcNow);
            if (statusName != "Pending")
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

            int? newStatusId = await unitOfWork.Invitations.GetStatusIdAsync(decision);
            if (newStatusId is null)
            {
                throw new InvalidOperationException($"The {decision} invitation status is missing from the database.");
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
                Status = decision
            };

            return response;
        }
    }
}
