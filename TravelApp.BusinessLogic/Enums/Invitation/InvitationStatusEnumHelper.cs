using System.Reflection;

namespace TravelApp.BusinessLogic.Enums
{
    /// <summary>Converts invitation statuses to and from status strings and applies the 14-day pending rule.</summary>
    public static class InvitationStatusEnumHelper
    {
        /// <summary>The number of days a pending invitation stays pending.</summary>
        public const int PendingLifetimeDays = 14;

        /// <summary>Returns the status string for an invitation status.</summary>
        /// <param name="status">The invitation status.</param>
        /// <returns>The status string, or null when the status has no <see cref="InvitationStatusNameAttribute"/>.</returns>
        public static string? EnumToStringOrNull(InvitationStatuses status)
        {
            FieldInfo? statusField = typeof(InvitationStatuses).GetField(status.ToString());
            if (statusField is null)
            {
                return null;
            }

            InvitationStatusNameAttribute? statusNameAttribute = statusField.GetCustomAttribute<InvitationStatusNameAttribute>();
            if (statusNameAttribute is null)
            {
                return null;
            }

            return statusNameAttribute.Name;
        }

        /// <summary>Matches a status string to an invitation status.</summary>
        /// <param name="statusString">The status string from a request or the database.</param>
        /// <param name="status">The matching status when the string is known.</param>
        /// <returns>true when the string matches a status; otherwise, false.</returns>
        public static bool TryGetStatusFromString(string statusString, out InvitationStatuses status)
        {
            foreach (InvitationStatuses invitationStatus in Enum.GetValues<InvitationStatuses>())
            {
                string? invitationStatusString = EnumToStringOrNull(invitationStatus);
                bool statusStringMatches = invitationStatusString == statusString;
                if (statusStringMatches)
                {
                    status = invitationStatus;
                    return true;
                }
            }

            status = default;
            return false;
        }

        /// <summary>Returns the Expired status string when a pending invitation reaches <see cref="PendingLifetimeDays"/>.</summary>
        /// <param name="statusString">The invitation status string.</param>
        /// <param name="sendDate">The date the invitation was sent in UTC.</param>
        /// <param name="currentUtc">The current UTC date and time.</param>
        /// <returns>Expired when pending has reached <see cref="PendingLifetimeDays"/>; otherwise, the original status string.</returns>
        /// <exception cref="InvalidOperationException">Thrown when Pending or Expired has no <see cref="InvitationStatusNameAttribute"/>.</exception>
        public static string GetStringWhenPendingReaches14Days(string statusString, DateTime sendDate, DateTime currentUtc)
        {
            string? pendingStatusString = EnumToStringOrNull(InvitationStatuses.Pending);
            if (pendingStatusString is null)
            {
                throw new InvalidOperationException($"Invitation status {InvitationStatuses.Pending} does not define a status string.");
            }

            bool statusIsPending = statusString == pendingStatusString;
            DateTime pendingExpiresAtUtc = sendDate.AddDays(PendingLifetimeDays);
            bool pendingHasReachedLifetime = pendingExpiresAtUtc <= currentUtc;

            if (statusIsPending && pendingHasReachedLifetime)
            {
                string? expiredStatusString = EnumToStringOrNull(InvitationStatuses.Expired);
                if (expiredStatusString is null)
                {
                    throw new InvalidOperationException($"Invitation status {InvitationStatuses.Expired} does not define a status string.");
                }

                return expiredStatusString;
            }

            return statusString;
        }
    }
}
