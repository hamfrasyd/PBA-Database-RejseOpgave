using System.ComponentModel.DataAnnotations;

namespace TravelApp.BusinessLogic.Handlers.Reviews.UpdateReview
{
    /// <summary>Request to update a review.</summary>
    public class UpdateReviewRequest
    {
        /// <summary>Gets or sets the content of the review.</summary>
        [MaxLength(1024)]
        public string? Content { get; set; }

        /// <summary>Gets or sets the star rating of the review.</summary>
        [Range(1, 5)]
        public byte? Stars { get; set; }
    }
}