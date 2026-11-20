using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TravelApp.API.ExceptionFilters;
using TravelApp.BusinessLogic.Handlers.Group;
using TravelApp.BusinessLogic.Handlers.Groups;
using TravelApp.BusinessLogic.Handlers.Groups.AddGroupMembers;
using TravelApp.BusinessLogic.Handlers.Groups.AddGroupTrips;
using TravelApp.BusinessLogic.Handlers.Groups.CreateGroup;
using TravelApp.BusinessLogic.Handlers.Groups.DeleteGroup;
using TravelApp.BusinessLogic.Handlers.Groups.RemoveGroupTrips;
using TravelApp.BusinessLogic.Handlers.Groups.UpdateGroup;
using TravelApp.BusinessLogic.Handlers.Groups.UpdateGroupMember;
using TravelApp.BusinessLogic.Handlers.Groups.UpdateGroupMembers;

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
            return Ok(result);
        }

        ///<summary>Gets a specific group by its id</summary>
        // GET api/groups/{id}
        [HttpGet("{id:int}")]
        [ProducesResponseType(typeof(GetGroupResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<GetGroupResponse>> GetGroup([FromRoute] int id, [FromServices] GetGroupHandler getGroupHandler)
        {
            GetGroupResponse result = await getGroupHandler.GetGroupByIdAsync(id);
            return Ok(result);
        }

        /// <summary>Creates a group and returns its id.</summary>
        // POST api/groups
        [HttpPost]
        [ProducesResponseType(typeof(CreateGroupResponse), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)] // May be thrown from CreateGroupHandler.
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status409Conflict)] // May be thrown from CreateGroupHandler.
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<CreateGroupResponse>> CreateGroup([FromBody] CreateGroupRequest request, [FromServices] CreateGroupHandler createGroupHandler)
        {
            CreateGroupResponse response = await createGroupHandler.CreateGroupAsync(request);
            return Created($"/api/groups/{response.Id}", response);
        }

        /// <summary>Adds members to a group</summary>
        // POST api/groups/{groupId}/members
        [HttpPost("{groupId:int}/members")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)] // May be thrown from AddGroupMembersHandler.
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)] // Group or user not found.
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> AddGroupMembers([FromRoute] int groupId, [FromBody] AddGroupMembersRequest request, [FromServices] AddGroupMembersHandler addGroupMembersHandler)
        {
            await addGroupMembersHandler.AddGroupMembersAsync(groupId, request);
            return NoContent();
        }

        /// <summary>Adds trips to a group</summary>
        // POST api/groups/{groupId}/trips
        [HttpPost("{groupId:int}/trips")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)] // May be thrown from AddGroupTripsHandler.
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)] // Group or trip not found.
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> AddGroupTrips([FromRoute] int groupId, [FromBody] AddGroupTripsRequest request, [FromServices] AddGroupTripsHandler addGroupTripsHandler)
        {
            await addGroupTripsHandler.AddGroupTripsAsync(groupId, request);
            return NoContent();
        }

        /// <summary>Deletes a group.</summary>
        // DELETE api/groups/{groupId}
        [HttpDelete("{groupId:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)] // Thrown from DeleteGroupHandler for missing resources.
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status409Conflict)] // May be thrown from DeleteGroupHandler.
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteGroup([FromRoute] int groupId, [FromServices] DeleteGroupHandler deleteGroupHandler)
        {
            await deleteGroupHandler.DeleteGroupAsync(groupId);
            return NoContent();
        }

        /// <summary>Updates a group and returns the updated group.</summary>
        // PUT api/groups/{groupId}
        [HttpPut("{groupId:int}")]
        [ProducesResponseType(typeof(UpdateGroupRequest), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)] // May be thrown from UpdateGroupHandler.
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)] // Thrown from UpdateGroupHandler for missing resources.
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<UpdateGroupRequest>> UpdateGroup(
            [FromRoute] int groupId,
            [FromBody] UpdateGroupRequest request,
            [FromServices] UpdateGroupHandler updateGroupHandler)
        {
            await updateGroupHandler.UpdateGroupAsync(groupId, request);
            return Ok(request);
        }

        /// <summary>Removes members from a group</summary>
        // DELETE api/groups/{groupId}/members
        [HttpDelete("{groupId:int}/members")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)] // May be thrown from UpdateGroupMembersHandler.
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)] // Thrown from UpdateGroupMembersHandler for missing resources.
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateGroupMembers(
            [FromRoute] int groupId,
            [FromBody] RemoveGroupMembersRequest request,
            [FromServices] RemoveGroupMembersHandler removeGroupMembersHandler)
        {
            await removeGroupMembersHandler.RemoveGroupMembersAsync(groupId, request);
            return NoContent();
        }

        /// <summary>Removes trips from a group</summary>
        // DELETE api/groups/{groupId}/trips
        [HttpDelete("{groupId:int}/trips")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)] // May be thrown from RemoveGroupTripsHandler.
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)] // Thrown from RemoveGroupTripsHandler for missing resources.
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> RemoveGroupTrips(
            [FromRoute] int groupId,
            [FromBody] RemoveGroupTripsRequest request,
            [FromServices] RemoveGroupTripsHandler removeGroupTripsHandler)
        {
            await removeGroupTripsHandler.RemoveGroupTripsAsync(groupId, request);
            return NoContent();
        }
    }
}
