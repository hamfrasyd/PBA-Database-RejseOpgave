/// <summary>Represents the response for retrieving a pending invitation.</summary>
public class GetMyPendingInvitationResponse
{
    /// <summary>Gets or sets the invitation identifier.</summary>
    public int Id { get; set; }
    /// <summary>Gets or sets the trip identifier.</summary>
    public int TripId { get; set; }
    /// <summary>Gets or sets the receiver identifier.</summary>
    public int SenderId { get; set; }
    /// <summary>Gets or sets the sender's full name.</summary>
    public string SenderName { get; set; } = string.Empty;
    /// <summary>Gets or sets the trip start date.</summary>
    public DateTime? TripStartDate { get; set; }
    /// <summary>Gets or sets the invitation response.</summary>
    public bool? IsAccepted { get; set; }
    /// <summary>Gets or sets the invitation send date.</summary>
    public DateTime SendDate { get; set; }
}