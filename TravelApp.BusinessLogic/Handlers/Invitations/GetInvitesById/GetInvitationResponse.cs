namespace TravelApp.BusinessLogic.Handlers.Invitations.GetInvitesById
{
    /// <summary>Represents an invitation returned to the user.</summary>
    public class GetInvitationResponse
    {
        /// <summary>Gets or sets the invitation identifier.</summary>
        public int Id { get; set; }

        /// <summary>Gets or sets the trip identifier.</summary>
        public int TripId { get; set; }

        /// <summary>Gets or sets the sender identifier.</summary>
        public int SenderId { get; set; }

        /// <summary>Gets or sets the sender's name.</summary>
        public string SenderName { get; set; } = string.Empty;

        /// <summary>Gets or sets the receiver identifier.</summary>
        public int ReceiverId { get; set; }

        /// <summary>Gets or sets the receiver's full name.</summary>
        public string ReceiverName { get; set; } = string.Empty;

        /// <summary>Gets or sets the trip start date.</summary>
        public DateTime? TripStartDate { get; set; }

        /// <summary>Gets or sets the invitation response.</summary>
        public bool? IsAccepted { get; set; }

        /// <summary>Gets or sets the date the invitation was sent.</summary>
        public DateTime SendDate { get; set; }
    }
}
