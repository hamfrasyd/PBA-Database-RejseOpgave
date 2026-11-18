namespace TravelApp.BusinessLogic.Handlers.Invitations.GetInvitation
{
    /// <summary>Contains the details of a trip invitation.</summary>
    public class GetInvitationResponse
    {
        /// <summary>Gets or sets the invitation identifier.</summary>
        public int Id { get; set; }

        /// <summary>Gets or sets the trip identifier.</summary>
        public int TripId { get; set; }

        /// <summary>Gets or sets the sender's identifier.</summary>
        public int SenderId { get; set; }

        /// <summary>Gets or sets the receiver's identifier.</summary>
        public int ReceiverId { get; set; }

        /// <summary>Gets or sets the effective invitation status.</summary>
        public string Status { get; set; } = string.Empty;

        /// <summary>Gets or sets when the invitation was sent.</summary>
        public DateTime SendDate { get; set; }

        /// <summary>Gets or sets when the invitation expires.</summary>
        public DateTime ExpiresAt { get; set; }
    }
}
