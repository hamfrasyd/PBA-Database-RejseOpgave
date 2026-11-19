namespace TravelApp.BusinessLogic.DTOs.Reviews
{
    /// <summary>Data Transfer Object for updating review details.</summary>
    public class UpdateReviewDTO
    {
        /// <summary>Gets or sets the content of the review.</summary>
        public string? Content { get; set; }
        /// <summary>Gets or sets the star rating of the review.</summary>
        public byte? Stars { get; set; }
    }
}