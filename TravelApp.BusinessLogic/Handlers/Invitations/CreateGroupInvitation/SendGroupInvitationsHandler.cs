
using TravelApp.BusinessLogic.Auth;
using TravelApp.BusinessLogic.DTOs.Trips;
using TravelApp.BusinessLogic.Exceptions.SpecificCustomException;
using TravelApp.BusinessLogic.Interfaces;
using TravelApp.BusinessLogic.Models.Invitations;

namespace TravelApp.BusinessLogic.Handlers.Invitations.SendGroupInvitations
{
    /// <summary>Handles inviting group members to a trip.</summary>
    public class SendGroupInvitationsHandler
    {
        private readonly IUnitOfWorkFactory _unitOfWorkFactory;
        private readonly ICurrentUser _currentUser;

        /// <summary>Initializes a new SendGroupInvitationsHandler instance.</summary>
        public SendGroupInvitationsHandler(IUnitOfWorkFactory unitOfWorkFactory, ICurrentUser currentUser)
        {
            _unitOfWorkFactory = unitOfWorkFactory;
            _currentUser = currentUser;
        }

        /// <summary>Sends individual invitations to eligible group members.</summary>
        public async Task<SendGroupInvitationsResponse> SendGroupInvitationsAsync(int tripId, int groupId)
        {
            int senderId = _currentUser.UserId;
            DateTime todayUtc = DateTime.UtcNow.Date;

            await using IUnitOfWork unitOfWork = _unitOfWorkFactory.Create();

            // Check that the trip exists.
            TripDetailsDTO? trip = await unitOfWork.Trips.GetDetailsAsync(tripId);
            if (trip is null)
            {
                throw new TravelAppNotFoundException($"Trip with ID {tripId} was not found.");
            }

            // Trips without a start date can receive invitations.
            if (trip.StartDate.HasValue && trip.StartDate.Value.Date <= todayUtc)
            {
                throw new TravelAppConflictException("You cannot invite users to a trip that has started.");
            }

            // Check that the sender can invite users to the trip.
            TripAccessInfoDTO? senderAccess = await unitOfWork.Trips.GetAccessInfoAsync(tripId, senderId);

            bool senderIsParticipant = senderAccess?.IsParticipant == true;

            if (!senderIsParticipant && !_currentUser.IsAdmin)
            {
                throw new TravelAppForbiddenException( "You cannot invite users to this trip.");
            }

            // Check that the group exists.
            bool groupExists = await unitOfWork.Groups.GroupExistsAsync(groupId);
            if (!groupExists)
            {
                throw new TravelAppNotFoundException($"Group with ID {groupId} was not found.");
            }

            // Retrieve the group's members.
            List<int> memberIds = await unitOfWork.Groups.GetMemberIdsAsync(groupId);

            int createdCount = 0;
            int skippedCount = 0;

            foreach (int receiverId in memberIds.Distinct())
            {
                // Do not invite yourself.
                if (receiverId == senderId)
                {
                    skippedCount++;
                    continue;
                }

                // Do not invite existing trip participants.
                bool alreadyParticipant = await unitOfWork.Invitations.IsParticipantAsync(tripId, receiverId);
                if (alreadyParticipant)
                {
                    skippedCount++;
                    continue;
                }

                // Do not create duplicate pending invitations.
                bool alreadyInvited = await unitOfWork.Invitations.PendingInviteExistsForTripAsync(tripId, receiverId);
                if (alreadyInvited)
                {
                    skippedCount++;
                    continue;
                }

                Invite invite = new Invite
                {
                    SendDate = DateTime.UtcNow,
                    IsAccepted = null
                };

                await unitOfWork.Invitations.SendInviteAsync(
                    invite,
                    tripId,
                    senderId,
                    receiverId);

                createdCount++;
            }

            // Save all invitations together.
            if (createdCount > 0)
            {
                await unitOfWork.CommitAsync();
            }

            return new SendGroupInvitationsResponse
            {
                TripId = tripId,
                GroupId = groupId,
                CreatedCount = createdCount,
                SkippedCount = skippedCount
            };
        }
    }
}
