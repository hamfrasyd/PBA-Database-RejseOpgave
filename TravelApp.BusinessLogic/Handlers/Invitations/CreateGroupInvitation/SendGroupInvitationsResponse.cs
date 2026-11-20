
namespace TravelApp.BusinessLogic.Handlers.Invitations.SendGroupInvitations
{
    /// <summary>Represents the result of inviting a group to a trip.</summary>
    public class SendGroupInvitationsResponse
    {
        /// <summary>Gets or sets the trip identifier.</summary>
        public int TripId { get; set; }

        /// <summary>Gets or sets the group identifier.</summary>
        public int GroupId { get; set; }

        /// <summary>Gets or sets the number of invitations created.</summary>
        public int CreatedCount { get; set; }

        /// <summary>Gets or sets the number of members skipped.</summary>
        public int SkippedCount { get; set; }
    }
}
