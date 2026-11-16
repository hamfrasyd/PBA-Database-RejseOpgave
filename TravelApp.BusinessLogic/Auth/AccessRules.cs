
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
        /// <param name="currentUser">The current logged-in user.</param>
        /// <exception cref="TravelAppForbiddenException">Thrown when the current user is not an administrator.</exception>
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
        /// <param name="currentUser">The current logged-in user.</param>
        /// <param name="ownerId">The identifier of the resource owner.</param>
        /// <param name="isPrivate">A value indicating whether the resource is private.</param>
        /// <exception cref="TravelAppForbiddenException">Thrown when a private resource is accessed by someone who is neither the owner nor an administrator.</exception>
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
