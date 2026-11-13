using Microsoft.AspNetCore.Mvc;
using TravelApp.BusinessLogic.Handlers.DatabaseSelection;

namespace TravelApp.API.Controllers
{
    [ApiController]
    [Route("api/database-providers")]
    public class DatabaseProvidersController : ControllerBase
    {
        /// <summary>
        /// Returns the available database provider choices.
        /// </summary>
        // GET api/database-providers
        [HttpGet]
        [ProducesResponseType(typeof(GetDatabaseProvidersResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<GetDatabaseProvidersResponse> Get([FromServices] GetDatabaseProvidersHandler getDatabaseProvidersHandler)
        {
            GetDatabaseProvidersResponse result = getDatabaseProvidersHandler.GetDatabaseProviders();
            return Ok(result);
        }
    }
}
