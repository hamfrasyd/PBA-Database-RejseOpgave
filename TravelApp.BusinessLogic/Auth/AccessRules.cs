
using TravelApp.BusinessLogic.Exceptions.SpecificCustomException;

namespace TravelApp.BusinessLogic.Auth
{
    /// <summary>
    /// Defines reusable access rules for resources.
    /// </summary>
    public static class AccessRules
    {
        /// <summary>
        /// Requires the current user to be the resource owner or an administrator.
        /// </summary>
        /// <param name="currentUser">The current logged-in user.</param>
        /// <param name="ownerId">The identifier of the resource owner.</param>
        /// <exception cref="TravelAppForbiddenException">Thrown when the current user is neither the owner nor an administrator.</exception>
        public static void RequireCurrentUserIsOwnerOrAdmin(ICurrentUser currentUser, int ownerId)
        {
            bool currentUserIsAdmin = currentUser.IsAdmin;
            bool currentUserIsNotOwner = currentUser.UserId != ownerId;

            if (currentUserIsNotOwner && currentUserIsAdmin is false)
            {
                throw new TravelAppForbiddenException("Access denied. Only the owner or an administrator can access this resource.");
            }
        }

        /// <summary>
        /// Requires the current user to be an administrator.
        /// </summary>
        /// <param name="currentUser">The current logged-in user.</param>
        /// <exception cref="TravelAppForbiddenException">Thrown when the current user is not an administrator.</exception>
        public static void RequireCurrentUserIsAdmin(ICurrentUser currentUser)
        {
            bool currentUserIsAdmin = currentUser.IsAdmin;

            if (currentUserIsAdmin is false)
            {
                throw new TravelAppForbiddenException("This operation requires an administrator account.");
            }
        }

        /// <summary>
        /// Allows access to public resources or requires the owner or an administrator for private resources.
        /// </summary>
        /// <param name="currentUser">The current logged-in user.</param>
        /// <param name="ownerId">The identifier of the resource owner.</param>
        /// <param name="isPrivate">A value indicating whether the resource is private.</param>
        /// <exception cref="TravelAppForbiddenException">Thrown when a private resource is accessed by someone who is neither the owner nor an administrator.</exception>
        public static void RequireCurrentUserIsOwnerOrAdminWhenProfileIsPrivate(ICurrentUser currentUser, int ownerId, bool isPrivate)
        {
            bool resourceIsPrivate = isPrivate;
            bool currentUserIsAdmin = currentUser.IsAdmin;
            bool currentUserIsNotOwner = currentUser.UserId != ownerId;

            if (resourceIsPrivate && currentUserIsNotOwner && currentUserIsAdmin is false)
            {
                throw new TravelAppForbiddenException("Access denied. This resource is private.");
            }
        }

        /// <summary>Allows friendship details only to either friend or an administrator.</summary>
        /// <param name="currentUser">The current logged-in user.</param>
        /// <param name="firstUserId">The first friend's identifier.</param>
        /// <param name="secondUserId">The second friend's identifier.</param>
        /// <exception cref="TravelAppForbiddenException">Thrown when the user is not part of the friendship.</exception>
        public static void RequireCurrentUserIsFriendOrAdmin(ICurrentUser currentUser, int firstUserId, int secondUserId)
        {
            bool currentUserIsFriend = currentUser.UserId == firstUserId || currentUser.UserId == secondUserId;
            if (currentUserIsFriend is false && currentUser.IsAdmin is false)
            {
                throw new TravelAppForbiddenException("Only members of this friendship or an administrator can access it.");
            }
        }

        /// <summary>Allows the participants of a trip or an administrator to send invitations.</summary>
        /// <param name="currentUser">The current logged-in user.</param>
        /// <param name="isTripParticipant">Whether the user participates in the trip.</param>
        /// <exception cref="TravelAppForbiddenException">Thrown when the user cannot invite anyone to the trip.</exception>
        public static void RequireCurrentUserIsTripParticipantOrAdmin(ICurrentUser currentUser, bool isTripParticipant)
        {
            if (isTripParticipant is false && currentUser.IsAdmin is false)
            {
                throw new TravelAppForbiddenException("Only trip participants or an administrator can send invitations.");
            }
        }

        /// <summary>Allows an invitation to be viewed only by its sender, receiver or an administrator.</summary>
        /// <param name="currentUser">The current logged-in user.</param>
        /// <param name="senderId">The sender's identifier.</param>
        /// <param name="receiverId">The receiver's identifier.</param>
        /// <exception cref="TravelAppForbiddenException">Thrown when the user is not allowed to view the invitation.</exception>
        public static void RequireCurrentUserIsInvitationParticipantOrAdmin(ICurrentUser currentUser, int senderId, int receiverId)
        {
            bool isInvolvedUser = currentUser.UserId == senderId || currentUser.UserId == receiverId;
            if (isInvolvedUser is false && currentUser.IsAdmin is false)
            {
                throw new TravelAppForbiddenException("Only the invitation sender, receiver or an administrator can view it.");
            }
        }

        /// <summary>Allows only the invited receiver to respond to an invitation.</summary>
        /// <param name="currentUser">The current logged-in user.</param>
        /// <param name="receiverId">The receiver's identifier.</param>
        /// <exception cref="TravelAppForbiddenException">Thrown when the user is not the receiver.</exception>
        public static void RequireCurrentUserIsInvitationReceiver(ICurrentUser currentUser, int receiverId)
        {
            if (currentUser.UserId != receiverId)
            {
                throw new TravelAppForbiddenException("Only the invitation receiver can respond to it.");
            }
        }

        /// <summary>Allows the sender or an administrator to cancel an invitation.</summary>
        /// <param name="currentUser">The current logged-in user.</param>
        /// <param name="senderId">The sender's identifier.</param>
        /// <exception cref="TravelAppForbiddenException">Thrown when the user cannot cancel the invitation.</exception>
        public static void RequireCurrentUserIsInvitationSenderOrAdmin(ICurrentUser currentUser, int senderId)
        {
            if (currentUser.UserId != senderId && currentUser.IsAdmin is false)
            {
                throw new TravelAppForbiddenException("Only the invitation sender or an administrator can cancel it.");
            }
        }
    }
}
