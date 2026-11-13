using System.Security.Claims;
using TravelApp.BusinessLogic.Auth;

namespace TravelApp.API.Auth
{
    /// <summary>Exposes the authenticated user to Core without passing HTTP types into handlers.</summary>
    public class CurrentUser : ICurrentUser
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        /// <summary>Initializes a new CurrentUser instance.</summary>
        public CurrentUser(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        /// <summary>Gets or sets the user id.</summary>
        public int UserId
        {
            get
            {
                string? userId = _httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (int.TryParse(userId, out int parsedUserId) is false)
                {
                    throw new UnauthorizedAccessException("Authentication is required.");
                }
                return parsedUserId;
            }
        }

        /// <summary>Gets or sets whether is admin.</summary>
        public bool IsAdmin
        {
            get
            {
                return _httpContextAccessor.HttpContext?.User.IsInRole("Admin") ?? false;
            }
        }
    }
}
