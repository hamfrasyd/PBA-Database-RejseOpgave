
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
        public static void RequireOwnerOrAdmin(ICurrentUser currentUser, int ownerId)
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
        public static void RequireAdmin(ICurrentUser currentUser)
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
        public static void RequirePublicOrOwnerOrAdmin(ICurrentUser currentUser, int ownerId, bool isPrivate)
        {
            bool resourceIsPrivate = isPrivate;
            bool currentUserIsAdmin = currentUser.IsAdmin;
            bool currentUserIsNotOwner = currentUser.UserId != ownerId;

            if (resourceIsPrivate && currentUserIsNotOwner && currentUserIsAdmin is false)
            {
                throw new TravelAppForbiddenException("Access denied. This resource is private.");
            }
        }
    }
}
