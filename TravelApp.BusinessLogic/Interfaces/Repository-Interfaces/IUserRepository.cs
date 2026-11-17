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
        public Task AddAsync(TravelUser user);

        #endregion

        #region Read - Users

        /// <summary>Retrieves a user by its ID.</summary>
        public Task<TravelUser?> GetByIdAsync(int userId);
        /// <summary>Retrieves profile.</summary>
        public Task<GetUserProfileResponse?> GetProfileAsync(int userId);
        /// <summary>Retrieves visited countries.</summary>
        public Task<List<GetCountriesResponse>> GetVisitedCountriesAsync(int userId);
        /// <summary>Retrieves profiles.</summary>
        public Task<List<GetUserProfilesResponse>> GetProfilesAsync(GetUserProfilesRequest request, int viewerUserId, bool includePrivate);

        #endregion

        #region Update - Users
        /// <summary>Updates a user in the database.</summary>
        public Task UpdateAsync(TravelUser user);

        #endregion
        
        #region Delete - Users
        /// <summary>Deletes a user by its ID.</summary>
        public Task<bool> DeleteAsync(int userId);
        #endregion
    }
}
