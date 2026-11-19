namespace TravelApp.BusinessLogic.DTOs.Reviews
{
    /// <summary>Data transfer object for review summary. For List of reviews.</summary>
    public class ReviewSummaryDTO
    {
        /// <summary>Unique identifier for the review.</summary>
        public int Id { get; set; }
        /// <summary>Content of the review.</summary>
        public string Content { get; set; } = string.Empty;
        /// <summary>Rating for the review.</summary>
        public byte Stars { get; set; }

        /// <summary>Identifier for the user who wrote the review.</summary>    
        public int UserId { get; set; }
        /// <summary>First name of the user who wrote the review.</summary>
        public string FirstName { get; set; } = string.Empty;
        /// <summary>Last name of the user who wrote the review.</summary>
        public string LastName { get; set; } = string.Empty;
    }
}