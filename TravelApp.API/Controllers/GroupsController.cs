using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TravelApp.API.ExceptionFilters;
using TravelApp.BusinessLogic.Handlers.Posts;

namespace TravelApp.API.Controllers
{
    /// <summary>Exposes HTTP endpoints for groups and their related resources.</summary>
    [ApiController]
    [Authorize]
    [Route("api/groups")]
    public class GroupsController : ControllerBase
    {
        /// <summary>Lists all groups.</summary>
        // GET api/groups
        [HttpGet]
        [AllowAnonymous]
        [ProducesResponseType(typeof(List<GetGroupsResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<List<GetGroupsResponse>>> GetGroups([FromServices] GetGroupsHandler getGroupsHandler)
        {
            List<GetGroupsResponse> result = await getGroupsHandler.GetGroupsAsync();
            if (result.Count == 0)
            {
                return NotFound();
            }
            return Ok(result);
        }
    }
}
