using System.ComponentModel.DataAnnotations;

namespace TravelApp.BusinessLogic.Handlers.Reviews.CreateReview
{
    /// <summary>Request object for creating a review.</summary>
    public class CreateReviewRequest
    {
        /// <summary>Content of the review.</summary>
        [Required]
        [MaxLength(1024)]
        public string Content { get; set; } = string.Empty;

        /// <summary>Rating of the review.</summary>
        [Range(1, 5)]
        public byte Stars { get; set; }
    }
}