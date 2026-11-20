using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TravelApp.API.ExceptionFilters;
using TravelApp.BusinessLogic.DTOs.Pagination;
using TravelApp.BusinessLogic.Handlers.Activities.GetActivity;
using TravelApp.BusinessLogic.Handlers.Trips.AddTripParticipant;
using TravelApp.BusinessLogic.Handlers.Trips.CreateTrip;
using TravelApp.BusinessLogic.Handlers.Trips.DeleteTrip;
using TravelApp.BusinessLogic.Handlers.Trips.GetTrip;
using TravelApp.BusinessLogic.Handlers.Trips.RemoveTripParticipant;
using TravelApp.BusinessLogic.Handlers.Trips.TripActivities;
using TravelApp.BusinessLogic.Handlers.Trips.TripCity;
using TravelApp.BusinessLogic.Handlers.Trips.TripParticipants;
using TravelApp.BusinessLogic.Handlers.Trips.UpdateTrip;

namespace TravelApp.API.Controllers
{
    /// <summary>Exposes HTTP endpoints for trips and their related resources.</summary>
    [ApiController]
    [Authorize]
    [Route("api/trips")]
    public class TripsController : ControllerBase
    {
        /// <summary>Retrieves all trips in the system.</summary>
        [HttpGet]
        [ProducesResponseType(typeof(PagedResultDTO<GetTripsResponse>), StatusCodes.Status200OK)]
        public async Task<ActionResult<PagedResultDTO<GetTripsResponse>>> GetAllTrips(
            [FromServices] GetAllTripsHandler handler, 
            [FromQuery] int? countryId = null, [FromQuery] int? continentId = null, [FromQuery] bool? isCompleted = null, [FromQuery] int pageNumber = 1)
        {
            PagedResultDTO<GetTripsResponse> result = await handler.GetAllTripsAsync(countryId, continentId, isCompleted, pageNumber);
            return Ok(result);
        }

        /// <summary>Retrieves the current user's trips.</summary>
        [HttpGet("mine")]
        [ProducesResponseType(typeof(PagedResultDTO<GetTripsResponse>), StatusCodes.Status200OK)]
        public async Task<ActionResult<PagedResultDTO<GetTripsResponse>>> GetMyTrips(
            [FromServices] GetTripsHandler handler,
            [FromQuery] int? countryId = null, [FromQuery] int? continentId = null, [FromQuery] int pageNumber = 1)
        {
            PagedResultDTO<GetTripsResponse> result = await handler.GetTripsAsync(countryId, continentId, pageNumber);
            return Ok(result);
        }

        /// <summary>Retrieves a trip with its cities, countries and continents.</summary>
        // GET api/trips/{tripId}
        [HttpGet("{tripId:int}")]
        [ProducesResponseType(typeof(GetTripResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(UnauthorizedResult), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<GetTripResponse>> GetTrip([FromRoute] int tripId, [FromServices] GetTripHandler getTripHandler)
        {
            GetTripResponse result = await getTripHandler.GetTripAsync(tripId);
            return Ok(result);
        }

        /// <summary>Lists activities associated with a trip.</summary>
        // GET api/trips/{tripId}/activities
        [HttpGet("{tripId:int}/activities")]
        [ProducesResponseType(typeof(List<GetActivityResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(UnauthorizedResult), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<List<GetActivityResponse>>> GetTripActivities([FromRoute] int tripId, [FromServices] TripActivitiesHandler getTripActivitiesHandler)
        {
            List<GetActivityResponse> result = await getTripActivitiesHandler.GetTripActivitiesAsync(tripId);

            return Ok(result);
        }

        /// <summary>Lists users participating in a trip.</summary>
        // GET api/trips/{tripId}/participants
        [HttpGet("{tripId:int}/participants")]
        [ProducesResponseType(typeof(List<GetTripParticipantResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(UnauthorizedResult), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<List<GetTripParticipantResponse>>> GetTripParticipants([FromRoute] int tripId, [FromServices] TripParticipantsHandler getTripParticipantsHandler)
        {
            List<GetTripParticipantResponse> result = await getTripParticipantsHandler.GetTripParticipantsAsync(tripId);

            return Ok(result);
        }

        /// <summary>Creates a trip for the current user.</summary>
        // POST api/trips
        [HttpPost]
        [ProducesResponseType(typeof(CreateTripResponse), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(UnauthorizedResult), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<CreateTripResponse>> CreateTrip([FromBody] CreateTripRequest request, [FromServices] CreateTripHandler createTripHandler)
        {
            CreateTripResponse result = await createTripHandler.CreateTripAsync(request);
            return Created($"/api/trips/{result.Id}", result);
        }

        /// <summary>Updates a trip's dates, status and notes.</summary>
        // PUT api/trips/{tripId}
        [HttpPut("{tripId:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(UnauthorizedResult), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateTrip(
            [FromRoute] int tripId, [FromBody] UpdateTripRequest request, 
            [FromServices] UpdateTripHandler updateTripHandler)
        {
            await updateTripHandler.UpdateTripAsync(tripId, request);
            return NoContent();
        }

        /// <summary>Deletes a trip and its related records.</summary>
        // DELETE api/trips/{tripId}
        [HttpDelete("{tripId:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(UnauthorizedResult), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteTrip([FromRoute] int tripId, [FromServices] DeleteTripHandler deleteTripHandler)
        {
            await deleteTripHandler.DeleteTripAsync(tripId);
            return NoContent();
        }

        /// <summary>Adds an existing city to a trip.</summary>
        // POST api/trips/{tripId}/cities/{cityId}
        [HttpPost("{tripId:int}/cities/{cityId:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(UnauthorizedResult), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status409Conflict)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> AddCity([FromRoute] int tripId, [FromRoute] int cityId, [FromServices] TripCityHandler handler)
        {
            await handler.AddCityAsync(tripId, cityId);
            return NoContent();
        }

        /// <summary>Removes a city from a trip.</summary>
        // DELETE api/trips/{tripId}/cities/{cityId}
        [HttpDelete("{tripId:int}/cities/{cityId:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(UnauthorizedResult), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status409Conflict)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> RemoveCity([FromRoute] int tripId, [FromRoute] int cityId, [FromServices] TripCityHandler handler)
        {
            await handler.RemoveCityAsync(tripId, cityId);
            return NoContent();
        }

        /// <summary>Adds an existing activity to a trip.</summary>
        // POST api/trips/{tripId}/activities/{activityId}
        [HttpPost("{tripId:int}/activities/{activityId:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(UnauthorizedResult), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status409Conflict)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> AddActivity([FromRoute] int tripId, [FromRoute] int activityId, [FromServices] TripActivitiesHandler handler)
        {
            await handler.AddActivityAsync(tripId, activityId);
            return NoContent();
        }

        /// <summary>Removes an activity from a trip.</summary>
        // DELETE api/trips/{tripId}/activities/{activityId}
        [HttpDelete("{tripId:int}/activities/{activityId:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(UnauthorizedResult), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> RemoveActivity([FromRoute] int tripId, [FromRoute] int activityId, [FromServices] TripActivitiesHandler handler)
        {
            await handler.RemoveActivityAsync(tripId, activityId);
            return NoContent();
        }

        /// <summary>Adds an existing user as a trip participant.</summary>
        // POST api/trips/{tripId}/participants/{userId}
        [HttpPost("{tripId:int}/participants/{userId:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(UnauthorizedResult), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status409Conflict)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> AddParticipant([FromRoute] int tripId, [FromRoute] int userId, [FromServices] AddTripParticipantHandler addTripParticipantHandler)
        {
            await addTripParticipantHandler.AddTripParticipantAsync(tripId, userId);
            return NoContent();
        }

        /// <summary>Removes a participant from a trip.</summary>
        // DELETE api/trips/{tripId}/participants/{userId}
        [HttpDelete("{tripId:int}/participants/{userId:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(UnauthorizedResult), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status409Conflict)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> RemoveParticipant([FromRoute] int tripId, [FromRoute] int userId, [FromServices] RemoveTripParticipantHandler removeTripParticipantHandler)
        {
            await removeTripParticipantHandler.RemoveTripParticipantAsync(tripId, userId);
            return NoContent();
        }
    }
}
