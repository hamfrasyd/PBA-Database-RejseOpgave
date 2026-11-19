using TravelApp.BusinessLogic.Auth;
using TravelApp.BusinessLogic.DTOs.Users;
using TravelApp.BusinessLogic.Exceptions.SpecificCustomException;
using TravelApp.BusinessLogic.Interfaces;

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

        /// <summary>Updates a user and returns the persisted result.</summary>
        /// <param name="userId">The user identifier.</param>
        /// <param name="request">The updated user details.</param>
        /// <returns>The updated user.</returns>
        /// <exception cref="TravelAppForbiddenException">Thrown when the current user cannot update the user.</exception>
        /// <exception cref="TravelAppNotFoundException">Thrown when the user does not exist.</exception>
        /// <exception cref="TravelAppValidationException">Thrown when the home city does not exist.</exception>
        public async Task<UpdateUserResponse> UpdateUserAsync(int userId, UpdateUserRequest request)
        {
            await using IUnitOfWork unitOfWork = _unitOfWorkFactory.Create();

            bool userWasFound = await unitOfWork.Users.UserExistsAsync(userId);
            if (userWasFound is false)
            {
                throw new TravelAppNotFoundException($"User with ID {userId} was not found.");
            }

            AccessRules.RequireCurrentUserIsOwnerOrAdmin(_currentUser, userId);

            bool cityExists = await unitOfWork.Locations.CityExistsAsync(request.CityId);
            if (cityExists is false)
            {
                throw new TravelAppValidationException($"City with ID {request.CityId} does not exist.");
            }

            UserBasicInfoDto userToUpdate = new UserBasicInfoDto
            {
                Id = userId,
                FirstName = request.FirstName.Trim(),
                LastName = request.LastName.Trim(),
                CityId = request.CityId,
                IsPrivate = request.IsPrivate
            };

            bool userWasUpdated = await unitOfWork.Users.UpdateUserAsync(userToUpdate);
            if (userWasUpdated is false)
            {
                throw new TravelAppNotFoundException($"User with ID {userId} was not found.");
            }

            await unitOfWork.CommitAsync();

            UpdateUserResponse response = new UpdateUserResponse
            {
                Id = userToUpdate.Id,
                FirstName = userToUpdate.FirstName,
                LastName = userToUpdate.LastName,
                CityId = userToUpdate.CityId,
                IsPrivate = userToUpdate.IsPrivate
            };

            return response;
        }
    }
}
