/// <summary>Represents the response returned after sending an invitation.</summary>
public class SendInvitationResponse
{
    /// <summary>Gets or sets the identifier of the sent invitation.</summary>
    public int Id { get; set; }
    /// <summary>Gets or sets the identifier of the trip associated with the invitation.</summary>
    public int TripId { get; set; }
    /// <summary>Gets or sets the identifier of the user receiving the invitation.</summary>
    public int ReceiverId { get; set; }

    /// <summary>Gets or sets when the invitation was sent.</summary>
    public DateTime SendDate { get; set; }
}