using TravelApp.BusinessLogic.Exceptions.SpecificCustomException;

namespace TravelApp.BusinessLogic.Auth
{
    /// <summary>Represents an access rules.</summary>
    public static class AccessRules
    {
        /// <summary>Checks owner or admin.</summary>
        public static void RequireOwnerOrAdmin(ICurrentUser currentUser, int ownerId)
        {
            if (currentUser.IsAdmin is false && currentUser.UserId != ownerId)
            {
                throw new TravelAppForbiddenException("You cannot modify another user's information.");
            }
        }

        /// <summary>Checks admin.</summary>
        public static void RequireAdmin(ICurrentUser currentUser)
        {
            if (currentUser.IsAdmin is false)
            {
                throw new TravelAppForbiddenException("This operation requires an administrator account.");
            }
        }
    }
}
