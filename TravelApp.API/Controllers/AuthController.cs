using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TravelApp.API.ExceptionFilters;
using TravelApp.BusinessLogic.Exceptions.SpecificCustomException;
using TravelApp.BusinessLogic.Auth;
using TravelApp.BusinessLogic.Handlers.Accounts.Login;
using TravelApp.BusinessLogic.Handlers.Users.GetUserProfile;

namespace TravelApp.API.Controllers
{
    /// <summary>Local demonstration sessions using users already stored in dbo.User.</summary>
    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        private readonly ICurrentUser _currentUser;

        /// <summary>Initializes a new AuthController instance.</summary>
        /// <param name="currentUser">The current logged-in user.</param>
        public AuthController(ICurrentUser currentUser)
        {
            _currentUser = currentUser;
        }

        #region POST - Login
        /// <summary>Selects an existing user for a local demonstration session.</summary>
        /// <param name="request">The login request.</param>
        /// <param name="loginHandler">The handler that finds the user.</param>
        /// <returns>The signed-in user.</returns>
        /// <exception cref="TravelAppValidationException">Thrown when the user identifier is missing.</exception>
        /// <exception cref="TravelAppNotFoundException">Thrown when the user does not exist.</exception>
        // POST api/auth/login
        [HttpPost("login")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(LoginResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)] // Thrown from LoginHandler for an invalid ID.
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)] // Thrown from LoginHandler when the user does not exist.
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<LoginResponse>> Login([FromBody] LoginRequest request, [FromServices] LoginHandler loginHandler)
        {
            LoginResponse result = await loginHandler.LoginAsync(request);

            List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, result.UserId.ToString()),
                new Claim(ClaimTypes.Name, result.FullName)
            };

            ClaimsIdentity identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            ClaimsPrincipal principal = new ClaimsPrincipal(identity);

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

            return Ok(result);
        }

        #endregion

        #region POST - Logout
        /// <summary>Signs out of the current demonstration session.</summary>
        /// <returns>A confirmation that the session was signed out.</returns>
        // POST api/auth/logout
        [HttpPost("logout")]
        [Authorize]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(UnauthorizedResult), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<string>> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            string result = "Logged out.";
            return Ok(result);
        }

        #endregion

        #region GET - Current Logged-in User Profile

        /// <summary>Returns the profile of the currently selected user.</summary>
        /// <param name="getUserProfileHandler">The handler that loads the profile.</param>
        /// <returns>The current user's profile.</returns>
        /// <exception cref="UnauthorizedAccessException">Thrown when no user is signed in.</exception>
        /// <exception cref="TravelAppNotFoundException">Thrown when the current user does not exist.</exception>
        // GET api/auth/me
        [HttpGet("me")]
        [Authorize]
        [ProducesResponseType(typeof(GetUserProfileResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(UnauthorizedResult), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)] // Thrown from GetUserProfileHandler.
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<GetUserProfileResponse>> Me([FromServices] GetUserProfileHandler getUserProfileHandler)
        {
            GetUserProfileResponse result = await getUserProfileHandler.GetUserProfileAsync(_currentUser.UserId);

            return Ok(result);
        }

        #endregion
    }
}
