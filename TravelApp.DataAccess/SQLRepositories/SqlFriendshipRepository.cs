using Microsoft.EntityFrameworkCore;
using TravelApp.BusinessLogic.DTOs.Friendships;
using TravelApp.BusinessLogic.Interfaces;
using TravelApp.BusinessLogic.Models.Friendships;
using TravelApp.DataAccess.SQLRepositories.EntityFramework;

namespace TravelApp.DataAccess.SQLRepositories
{
    /// <summary>Implements SQL Server database operations for friendships.</summary>
    public class SqlFriendshipRepository : IFriendshipRepository
    {
        private readonly TravelAppContext _context;

        /// <summary>Initializes the friendship repository.</summary>
        /// <param name="context">The shared database context.</param>
        public SqlFriendshipRepository(TravelAppContext context)
        {
            _context = context;
        }

        #region Create - Friendships

        /// <inheritdoc/>
        public async Task AddAsync(Friendship friendship, int firstUserId, int secondUserId)
        {
            await _context.FriendshipEntities.AddAsync(friendship);

            _context.Entry(friendship).Property("UserAId").CurrentValue = firstUserId;
            _context.Entry(friendship).Property("UserBId").CurrentValue = secondUserId;
        }

        #endregion

        #region Read - Friendships

        /// <inheritdoc/>
        public async Task<Friendship?> GetByIdAsync(int friendshipId)
        {
            Friendship? friendshipFromDb = await _context.FriendshipEntities
                .Include(friendship => friendship.UserA)
                .Include(friendship => friendship.UserB)
                .FirstOrDefaultAsync(friendship => friendship.Id == friendshipId);

            return friendshipFromDb;
        }

        /// <inheritdoc/>
        public async Task<List<FriendDTO>> GetFriendsAsync(int userId, int viewerId, bool canSeePrivateFriends)
        {
            IQueryable<Friendship> friendshipsQuery = _context.FriendshipEntities
                .AsNoTracking()
                .Where(friendship => friendship.UserA!.Id == userId || friendship.UserB!.Id == userId);

            if (canSeePrivateFriends is false)
            {
                // Do not expose private users in another user's public friend list.
                friendshipsQuery = friendshipsQuery.Where(friendship =>
                    friendship.UserA!.Id == userId
                        ? (friendship.UserB!.IsPrivate == false || friendship.UserB.Id == viewerId)
                        : (friendship.UserA.IsPrivate == false || friendship.UserA.Id == viewerId));
            }

            IQueryable<FriendDTO> friendsQuery = friendshipsQuery.Select(friendship => new FriendDTO
                {
                    FriendshipId = friendship.Id,
                    UserId = friendship.UserA!.Id == userId ? friendship.UserB!.Id : friendship.UserA.Id,
                    FirstName = friendship.UserA.Id == userId ? friendship.UserB!.FirstName : friendship.UserA.FirstName,
                    LastName = friendship.UserA.Id == userId ? friendship.UserB!.LastName : friendship.UserA.LastName,
                    StartDate = friendship.StartDate
                });

            List<FriendDTO> friendsFromDb = await friendsQuery
                .OrderBy(friend => friend.FirstName)
                .ThenBy(friend => friend.LastName)
                .ThenBy(friend => friend.UserId)
                .ToListAsync();

            return friendsFromDb;
        }

        /// <inheritdoc/>
        public async Task<bool> FriendshipExistsAsync(int firstUserId, int secondUserId)
        {
            bool friendshipExists = await _context.FriendshipEntities.AsNoTracking().AnyAsync(friendship =>
                (friendship.UserA!.Id == firstUserId && friendship.UserB!.Id == secondUserId) ||
                (friendship.UserA!.Id == secondUserId && friendship.UserB!.Id == firstUserId));

            return friendshipExists;
        }

        #endregion
    }
}
