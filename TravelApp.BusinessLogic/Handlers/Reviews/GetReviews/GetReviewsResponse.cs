namespace TravelApp.BusinessLogic.Handlers.Reviews.GetReviews
{
    /// <summary>Response object for retrieving reviews.</summary>
    public class GetReviewsResponse
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