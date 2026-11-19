using TravelApp.BusinessLogic.DTOs.Friendships;
using TravelApp.BusinessLogic.Models.Friendships;

namespace TravelApp.BusinessLogic.Interfaces
{
    /// <summary>Defines database operations for friendships.</summary>
    public interface IFriendshipRepository
    {
        #region Create - Friendships

        /// <summary>Stages a new friendship between two users.</summary>
        /// <param name="friendship">The friendship to add.</param>
        /// <param name="firstUserId">The first user's identifier.</param>
        /// <param name="secondUserId">The second user's identifier.</param>
        /// <returns>A task that completes when the friendship is staged.</returns>
        Task AddAsync(Friendship friendship, int firstUserId, int secondUserId);

        #endregion

        #region Read - Friendships

        /// <summary>Retrieves a friendship by its identifier.</summary>
        /// <param name="friendshipId">The friendship identifier.</param>
        /// <returns>The friendship, or null if it does not exist.</returns>
        Task<Friendship?> GetByIdAsync(int friendshipId);

        /// <summary>Retrieves the friends of a user.</summary>
        /// <param name="userId">The user identifier.</param>
        /// <param name="viewerId">The current user's identifier.</param>
        /// <param name="canSeePrivateFriends">Whether the viewer may see all private friends.</param>
        /// <returns>The user's friendships that the viewer may see.</returns>
        Task<List<FriendDTO>> GetFriendsAsync(int userId, int viewerId, bool canSeePrivateFriends);

        /// <summary>Checks whether two users are already friends in either direction.</summary>
        /// <param name="firstUserId">The first user's identifier.</param>
        /// <param name="secondUserId">The second user's identifier.</param>
        /// <returns>true if the friendship exists; otherwise, false.</returns>
        Task<bool> FriendshipExistsAsync(int firstUserId, int secondUserId);

        #endregion

        #region Delete - Friendships

        /// <summary>Stages deletion of a friendship by its identifier.</summary>
        /// <param name="friendshipFromDb">The friendship to delete.</param>
        Task DeleteAsync(Friendship friendshipFromDb);

        #endregion
    }
}
