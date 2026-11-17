
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TravelApp.BusinessLogic.Handlers.Reviews;
using TravelApp.BusinessLogic.Handlers.Reviews.CreateReview;
using TravelApp.BusinessLogic.Handlers.Reviews.GetReview;
using TravelApp.BusinessLogic.Handlers.Reviews.GetReviews;

namespace TravelApp.API.Controllers
{
    /// <summary>Exposes HTTP endpoints for reviews.</summary>
    [ApiController]
    [Authorize]
    [Route("api/reviews")]
    public class ReviewsController : ControllerBase
    {
        /// <summary>Creates a review for a trip.</summary>
        // POST api/trips/{tripId}/reviews
        [HttpPost("trips/{tripId:int}/reviews")]
        [ProducesResponseType(typeof(CreateReviewResponse), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<CreateReviewResponse>> CreateReview([FromRoute] int tripId, [FromBody] CreateReviewRequest request, [FromServices] CreateReviewHandler handler)
        {
            CreateReviewResponse result = await handler.CreateReviewAsync(tripId, request);

            return Created($"/api/reviews/{result.Id}", result);
        }

        /// <summary>Retrieves reviews for a trip.</summary>
        // GET api/trips/{tripId}/reviews
        [HttpGet("trips/{tripId:int}/reviews")]
        [ProducesResponseType(typeof(List<GetReviewsResponse>), StatusCodes.Status200OK)]
        public async Task<ActionResult<List<GetReviewsResponse>>> GetReviews([FromRoute] int tripId, [FromQuery] byte? stars, [FromServices] GetReviewsHandler handler)
        {
            List<GetReviewsResponse> result = await handler.GetAllReviewsAsync(tripId, stars);

            return Ok(result);
        }

        /// <summary>Retrieves a review by its identifier.</summary>
        // GET api/reviews/{reviewId}
        [HttpGet("reviews/{reviewId:int}")]
        [ProducesResponseType(typeof(GetReviewResponse), StatusCodes.Status200OK)]
        public async Task<ActionResult<GetReviewResponse>> GetReview([FromRoute] int reviewId, [FromServices] GetReviewHandler handler)
        {
            GetReviewResponse result = await handler.GetReviewById(reviewId);

            return Ok(result);
        }
    }
}
