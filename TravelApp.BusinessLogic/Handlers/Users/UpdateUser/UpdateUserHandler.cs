using TravelApp.BusinessLogic.Auth;
using TravelApp.BusinessLogic.DTOs.Users;
using TravelApp.BusinessLogic.Exceptions.SpecificCustomException;
using TravelApp.BusinessLogic.Handlers.Locations.Data;
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
        /// <exception cref="TravelAppValidationException">Thrown when a name is missing or the home city does not exist.</exception>
        public async Task<UpdateUserResponse> UpdateUserAsync(int userId, UpdateUserRequest request)
        {
            AccessRules.RequireOwnerOrAdmin(_currentUser, userId);

            bool firstNameIsMissing = string.IsNullOrWhiteSpace(request.FirstName);
            bool lastNameIsMissing = string.IsNullOrWhiteSpace(request.LastName);

            if (firstNameIsMissing || lastNameIsMissing)
            {
                throw new TravelAppValidationException("First name and last name are required.");
            }

            string normalizedFirstName = request.FirstName.Trim();
            string normalizedLastName = request.LastName.Trim();

            await using IUnitOfWork unitOfWork = _unitOfWorkFactory.Create();

            bool userExists = await unitOfWork.Users.UserExistsAsync(userId);
            if (userExists is false)
            {
                throw new TravelAppNotFoundException($"User {userId} was not found.");
            }

            CityDto? existingCity = await unitOfWork.Locations.GetCityAsync(request.CityId);
            bool cityNotFound = existingCity is null;
            if (cityNotFound)
            {
                throw new TravelAppValidationException($"City {request.CityId} does not exist.");
            }

            UserBasicInfoDto userToUpdate = new UserBasicInfoDto
            {
                Id = userId,
                FirstName = normalizedFirstName,
                LastName = normalizedLastName,
                CityId = request.CityId,
                IsPrivate = request.IsPrivate
            };

            UserBasicInfoDto? updatedUser = await unitOfWork.Users.UpdateUserAsync(userToUpdate);
            bool userNotFound = updatedUser is null;
            if (userNotFound)
            {
                throw new TravelAppNotFoundException($"User {userId} was not found.");
            }

            await unitOfWork.CommitAsync();

            UserBasicInfoDto persistedUser = updatedUser ?? throw new TravelAppNotFoundException($"User {userId} was not found.");

            UpdateUserResponse response = new UpdateUserResponse
            {
                Id = persistedUser.Id,
                FirstName = persistedUser.FirstName,
                LastName = persistedUser.LastName,
                CityId = persistedUser.CityId,
                IsPrivate = persistedUser.IsPrivate
            };

            return response;
        }
    }
}
