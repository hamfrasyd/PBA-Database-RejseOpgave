using TravelApp.BusinessLogic.Handlers.Destinations;
using TravelApp.BusinessLogic.Handlers.Users;
using TravelApp.BusinessLogic.Models;

namespace TravelApp.BusinessLogic.Interfaces
{
    /// <summary>Defines database operations for users.</summary>
    public interface IUserRepository
    {
        /// <summary>Retrieves a user by its ID.</summary>
        public Task<TravelUser?> GetByIdAsync(int userId);
        /// <summary>Retrieves profile.</summary>
        public Task<GetUserProfileResponse?> GetProfileAsync(int userId);
        /// <summary>Retrieves visited countries.</summary>
        public Task<List<GetCountriesResponse>> GetVisitedCountriesAsync(int userId);
        /// <summary>Retrieves profiles.</summary>
        public Task<List<GetUserProfilesResponse>> GetProfilesAsync(GetUserProfilesRequest request, int viewerUserId, bool includePrivate);
        /// <summary>Adds a user to the database.</summary>
        public Task AddAsync(TravelUser user);
        /// <summary>Updates a user in the database.</summary>
        public Task UpdateAsync(TravelUser user);
        /// <summary>Deletes a user by its ID.</summary>
        public Task<bool> DeleteAsync(int userId);
    }
}
