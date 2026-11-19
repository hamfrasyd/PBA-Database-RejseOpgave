namespace TravelApp.BusinessLogic.Enums
{
    /// <summary>Names the invitation statuses stored in the database.</summary>
    public enum InvitationStatuses
    {
        /// <summary>The invitation is waiting for a response.</summary>
        [InvitationStatusName("Pending")]
        Pending,

        /// <summary>The receiver accepted the invitation.</summary>
        [InvitationStatusName("Accepted")]
        Accepted,

        /// <summary>The receiver declined the invitation.</summary>
        [InvitationStatusName("Declined")]
        Declined,

        /// <summary>The invitation was not answered in time.</summary>
        [InvitationStatusName("Expired")]
        Expired
    }
}
