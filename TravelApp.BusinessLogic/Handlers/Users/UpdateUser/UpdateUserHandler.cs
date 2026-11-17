using TravelApp.BusinessLogic.Exceptions.SpecificCustomException;
using TravelApp.BusinessLogic.Interfaces;
using TravelApp.BusinessLogic.Auth;
using TravelApp.BusinessLogic.Models.Destinations;
using TravelApp.BusinessLogic.Models.Users;

namespace TravelApp.BusinessLogic.Handlers.Users.UpdateUser
{
    /// <summary>Handles the update user use case.</summary>
    public class UpdateUserHandler
    {
        private readonly IUnitOfWorkFactory _unitOfWorkFactory;
        private readonly ICurrentUser _currentUser;

        /// <summary>Initializes a new UpdateUserHandler instance.</summary>
        /// <param name="unitOfWorkFactory">The unit of work factory.</param>
        /// <param name="currentUser">The current logged-in user.</param>
        public UpdateUserHandler(IUnitOfWorkFactory unitOfWorkFactory, ICurrentUser currentUser)
        {
            _unitOfWorkFactory = unitOfWorkFactory;
            _currentUser = currentUser;
        }

        /// <summary>Updates user.</summary>
        /// <param name="request">The updated user details.</param>
        /// <exception cref="TravelAppForbiddenException">Thrown when the current user cannot update the user.</exception>
        /// <exception cref="TravelAppNotFoundException">Thrown when the user does not exist.</exception>
        /// <exception cref="TravelAppValidationException">Thrown when the home city does not exist.</exception>
        public async Task UpdateUserAsync(UpdateUserRequest request)
        {
            AccessRules.RequireOwnerOrAdmin(_currentUser, request.UserId);
            await using IUnitOfWork unitOfWork = _unitOfWorkFactory.Create();

            TravelUser? user = await unitOfWork.Users.GetByIdAsync(request.UserId);
            if (user is null)
            {
                throw new TravelAppNotFoundException($"User {request.UserId} was not found.");
            }

            City? city = await unitOfWork.Locations.GetCityByIdAsync(request.CityId);
            if (city is null)
            {
                throw new TravelAppValidationException($"City {request.CityId} does not exist.");
            }
            user.FirstName = request.FirstName.Trim();
            user.LastName = request.LastName.Trim();
            user.City = city;
            user.IsPrivate = request.IsPrivate;
            await unitOfWork.Users.UpdateAsync(user);
            await unitOfWork.CommitAsync();
        }
    }
}
