using TravelApp.BusinessLogic.DTOs.Users;
using TravelApp.BusinessLogic.Handlers.Locations.Data;
using TravelApp.BusinessLogic.Models.Users;

namespace TravelApp.BusinessLogic.Interfaces
{
    /// <summary>Defines database operations for users.</summary>
    public interface IUserRepository
    {
        #region Create - Users

        /// <summary>Adds a user to the database.</summary>
        /// <param name="user">The user to add.</param>
        /// <returns>A task that completes when the user is added.</returns>
        Task AddAsync(TravelUser user);

        #endregion

        #region Read - All Profiles & VisitedCountries

        /// <summary>Retrieves detailed user profiles that match a search.</summary>
        /// <param name="profileSearch">The name search, sort direction, and privacy scope.</param>
        /// <returns>The matching detailed user profiles.</returns>
        Task<List<UserProfileDetailsDTO>> GetProfilesAsync(UserProfileSearchDTO profileSearch);

        /// <summary>Retrieves countries visited by a user on completed trips.</summary>
        /// <param name="userId">The user identifier.</param>
        /// <returns>The visited countries.</returns>
        Task<List<VisitedCountryDto>> GetVisitedCountriesAsync(int userId);

        #endregion

        #region Read - By ID

        /// <summary>Checks whether a user exists.</summary>
        /// <param name="userId">The user identifier.</param>
        /// <returns><see langword="true"/> when the user exists; otherwise, <see langword="false"/>.</returns>
        Task<bool> UserExistsAsync(int userId);

        /// <summary>Retrieves a user by ID.</summary>
        /// <param name="userId">The user identifier.</param>
        /// <returns>The user, or <see langword="null"/> when it does not exist.</returns>
        Task<TravelUser?> GetByIdAsync(int userId);

        /// <summary>Retrieves one detailed user profile by ID.</summary>
        /// <param name="userId">The user identifier.</param>
        /// <returns>The detailed profile, or <see langword="null"/> when the user does not exist.</returns>
        Task<UserProfileDetailsDTO?> GetProfileAsync(int userId);

        #endregion

        #region Update - Users

        /// <summary>Updates a user's basic info: first name, last name, home city, and privacy.</summary>
        /// <param name="userToUpdate">The basic user fields to persist.</param>
        /// <returns>The saved basic user, or <see langword="null"/> when the user does not exist.</returns>
        Task<UserBasicInfoDto?> UpdateUserAsync(UserBasicInfoDto userToUpdate);

        #endregion

        #region Delete - Users

        /// <summary>Deletes a user.</summary>
        /// <param name="userId">The user identifier.</param>
        /// <returns>A task that completes when the user is marked for deletion.</returns>
        Task DeleteAsync(int userId);

        #endregion
    }
}
