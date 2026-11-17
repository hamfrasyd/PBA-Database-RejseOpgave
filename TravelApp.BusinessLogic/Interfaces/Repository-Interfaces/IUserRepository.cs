using TravelApp.BusinessLogic.Handlers.Locations.GetCountries;
using TravelApp.BusinessLogic.Handlers.Users.GetUserProfile;
using TravelApp.BusinessLogic.Handlers.Users.GetUserProfiles;
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

        /// <summary>Retrieves user profiles with optional filtering and sorting.</summary>
        /// <param name="request">The filter and sort options.</param>
        /// <param name="viewerUserId">The identifier of the user viewing the profiles.</param>
        /// <param name="includePrivate">A value indicating whether private profiles are included.</param>
        /// <returns>The matching user profiles.</returns>
        Task<List<GetUserProfilesResponse>> GetProfilesAsync(GetUserProfilesRequest request, int viewerUserId, bool includePrivate);

        /// <summary>Retrieves countries visited by a user on completed trips.</summary>
        /// <param name="userId">The user identifier.</param>
        /// <returns>The visited countries.</returns>
        Task<List<GetCountriesResponse>> GetVisitedCountriesAsync(int userId);

        #endregion

        #region Read - By ID

        /// <summary>Retrieves a user by ID.</summary>
        /// <param name="userId">The user identifier.</param>
        /// <returns>The user, or <see langword="null"/> when it does not exist.</returns>
        Task<TravelUser?> GetByIdAsync(int userId);

        /// <summary>Retrieves a user profile by ID.</summary>
        /// <param name="userId">The user identifier.</param>
        /// <returns>The profile, or <see langword="null"/> when the user does not exist.</returns>
        Task<GetUserProfileResponse?> GetProfileAsync(int userId);

        #endregion

        #region Update - Users

        /// <summary>Updates a user in the database.</summary>
        /// <param name="user">The user to update.</param>
        /// <returns>A task that completes when the user is updated.</returns>
        Task UpdateAsync(TravelUser user);

        #endregion

        #region Delete - Users

        /// <summary>Deletes a user by ID.</summary>
        /// <param name="userId">The user identifier.</param>
        /// <returns><see langword="true"/> when the user was deleted; otherwise, <see langword="false"/>.</returns>
        Task<bool> DeleteAsync(int userId);

        #endregion
    }
}
