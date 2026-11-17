namespace TravelApp.BusinessLogic.DTOs.Trips
{
    /// <summary>Describes whether the current user can access a trip.</summary>
    public class TripAccessInfoDTO
    {
        /// <summary>Gets or sets a value indicating whether the user is a trip participant.</summary>
        public bool IsParticipant { get; set; }
    }
}
