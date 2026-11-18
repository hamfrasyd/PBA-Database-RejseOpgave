namespace TravelApp.BusinessLogic.DTOs.Invitations
{
    /// <summary>Contains information about a sent or received trip invitation.</summary>
    public class InvitationDTO
    {
        /// <summary>Gets or sets the invitation identifier.</summary>
        public int Id { get; set; }

        /// <summary>Gets or sets the trip identifier.</summary>
        public int TripId { get; set; }

        /// <summary>Gets or sets the sender's identifier.</summary>
        public int SenderId { get; set; }

        /// <summary>Gets or sets the sender's name.</summary>
        public string SenderName { get; set; } = string.Empty;

        /// <summary>Gets or sets the receiver's identifier.</summary>
        public int ReceiverId { get; set; }

        /// <summary>Gets or sets the receiver's name.</summary>
        public string ReceiverName { get; set; } = string.Empty;

        /// <summary>Gets or sets the stored invitation status.</summary>
        public string Status { get; set; } = string.Empty;

        /// <summary>Gets or sets the date the invitation was sent.</summary>
        public DateTime SendDate { get; set; }
    }
}
