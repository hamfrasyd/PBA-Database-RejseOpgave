namespace TravelApp.BusinessLogic.Handlers.Invitations
{
    /// <summary>Defines how invitation status changes when the invitation expires.</summary>
    public static class InvitationStatusRules
    {
        /// <summary>Returns Expired when a pending invitation is at least 14 days old.</summary>
        /// <param name="statusName">The invitation status stored in the database.</param>
        /// <param name="sendDate">The date the invitation was sent in UTC.</param>
        /// <param name="currentUtc">The current UTC date and time.</param>
        /// <returns>The invitation's effective status.</returns>
        public static string GetEffectiveStatus(string statusName, DateTime sendDate, DateTime currentUtc)
        {
            bool isPending = statusName == "Pending";
            bool isExpired = sendDate.AddDays(14) <= currentUtc;

            if (isPending && isExpired)
            {
                return "Expired";
            }

            return statusName;
        }
    }
}
