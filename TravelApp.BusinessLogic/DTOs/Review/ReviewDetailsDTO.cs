namespace TravelApp.BusinessLogic.DTOs.Reviews
{
    /// <summary>Data transfer object for review details.</summary>
    public class ReviewDetailsDTO
    {
        /// <summary>Unique identifier for the review.</summary>
        public int Id { get; set; }
        /// <summary>Content of the review.</summary>
        public string Content { get; set; } = string.Empty;
        /// <summary>Rating for the review.</summary>
        public byte Stars { get; set; }
        /// <summary>Identifier for the trip associated with the review.</summary>
        public int TripId { get; set; }

        /// <summary>Identifier for the user who wrote the review.</summary>
        public int UserId { get; set; }
        /// <summary>First name of the user who wrote the review.</summary>
        public string FirstName { get; set; } = string.Empty;
        /// <summary>Last name of the user who wrote the review.</summary>
        public string LastName { get; set; } = string.Empty;
    }
}