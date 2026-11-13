using TravelApp.BusinessLogic.Exceptions.SpecificCustomException;
using TravelApp.BusinessLogic.Interfaces;
using TravelApp.BusinessLogic.Models;

namespace TravelApp.BusinessLogic.Handlers.Accounts
{
    /// <summary>Finds an existing user for local demo sign-in; does not authenticate identity.</summary>
    public class LoginHandler
    {
        private readonly IUnitOfWorkFactory _unitOfWorkFactory;

        /// <summary>Initializes a new LoginHandler instance.</summary>
        public LoginHandler(IUnitOfWorkFactory unitOfWorkFactory)
        {
            _unitOfWorkFactory = unitOfWorkFactory;
        }

        /// <summary>Signs in the operation.</summary>
        public async Task<LoginResponse> LoginAsync(LoginRequest request)
        {
            if (request.UserId <= 0)
            {
                throw new TravelAppValidationException("A valid user ID is required.");
            }

            await using IUnitOfWork unitOfWork = _unitOfWorkFactory.Create();

            TravelUser? user = await unitOfWork.Users.GetByIdAsync(request.UserId);

            if (user is null)
            {
                throw new TravelAppNotFoundException($"User {request.UserId} was not found.");
            }

            LoginResponse result = new LoginResponse
            {
                UserId = user.Id,
                FullName = $"{user.FirstName} {user.LastName}"
            };

            return result;
        }
    }
}
