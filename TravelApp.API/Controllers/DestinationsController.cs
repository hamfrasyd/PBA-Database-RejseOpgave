using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TravelApp.BusinessLogic.Handlers.Destinations;

namespace TravelApp.API.Controllers
{
    [ApiController]
    [Route("api/destinations")]
    public class DestinationsController : ControllerBase
    {
        private readonly GetContinentsHandler _getContinentsHandler;

        public DestinationsController(GetContinentsHandler getContinentsHandler)
        {
            _getContinentsHandler = getContinentsHandler;
        }

        /// <summary>
        /// Lists continents.
        /// </summary>
        // GET api/destinations/continents
        [HttpGet("continents")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(List<GetContinentsResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<List<GetContinentsResponse>>> GetContinents()
        {
            List<GetContinentsResponse> result = await _getContinentsHandler.GetContinentsAsync();
            if (result.Count == 0)
            {
                return NoContent();
            }
            return Ok(result);
        }
    }
}
