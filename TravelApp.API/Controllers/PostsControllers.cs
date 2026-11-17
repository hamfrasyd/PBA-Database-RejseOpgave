using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TravelApp.BusinessLogic.Exceptions.Filters;
using TravelApp.BusinessLogic.Handlers;
using TravelApp.BusinessLogic.Handlers.Posts;

namespace TravelApp.API.Controllers
{
    /// <summary>Exposes HTTP endpoints for posts.</summary>
    [ApiController]
    [Route("api/posts")]
    public class PostsControllers : ControllerBase
    {
        /// <summary>Lists posts by user ID.</summary>
        // GET api/posts/users/{userId}
        [HttpGet("users/{userId:int}")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(List<GetPostsResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<List<GetPostsResponse>>> GetPosts([FromRoute] int userId, [FromServices] GetPostsHandler getPostsHandler)
        {
            List<GetPostsResponse> result = await getPostsHandler.GetPostsByUserIdAsync(userId);
            if (result.Count == 0)
            {
                return NotFound();
            }
            return Ok(result);
        }

        /// <summary>Get specific post by id</summary>
        // GET api/post/{id]
        [HttpGet("{id:int}")]
        [ProducesResponseType(typeof(List<GetPostResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<GetPostResponse>> GetPost([FromRoute] int id, [FromServices] GetPostHandler getPostHandler)
        {
            GetPostResponse result = await getPostHandler.GetPostAsync(id);
            if (result is null)
            {
                return NotFound();
            }
            return Ok(result);
        }

        /// <summary>Creates a post and returns its ID.</summary>
        // POST api/posts
        [HttpPost]
        [ProducesResponseType(typeof(int), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)] // May be thrown from CreatePostHandler.
        [ProducesResponseType(typeof(UnauthorizedResult), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status409Conflict)] // May be thrown from CreatePostHandler.
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<int>> CreatePost([FromBody] CreatePostRequest request, [FromServices] CreatePostHandler createPostHandler)
        {
            CreatePostResponse response = await createPostHandler.CreatePostAsync(request);
            return Created($"/api/posts/{response.Id}", response);
        }

        /// <summary>Deletes a post and returns its ID.</summary>
        // DELETE api/posts/{userId}
        [HttpDelete("{postId:int}")]
        [ProducesResponseType(typeof(int), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)] // Thrown from DeletePostHandler for missing resources.
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status409Conflict)] // May be thrown from DeletePostHandler.
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<int>> DeletePost([FromRoute] int postId, [FromServices] DeletePostHandler deletePostHandler)
        {
            await deletePostHandler.DeletePostAsync(postId);
            return Ok(postId);
        }

        /// <summary>Updates a post and returns its ID.</summary>
        // PUT api/posts/{cityId}
        [HttpPut("{postId:int}")]
        [ProducesResponseType(typeof(int), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)] // May be thrown from UpdatePostHandler.
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)] // Thrown from UpdatePostHandler for missing resources.
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<UpdatePostRequest>> UpdatePost(
            [FromRoute] int postId,
            [FromBody] UpdatePostRequest request,
            [FromServices] UpdatePostHandler updatePostHandler)
        {
            request.Id = postId;
            await updatePostHandler.UpdatePostAsync(request);
            return Ok(request);
        }
    }
}
