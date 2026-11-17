
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TravelApp.BusinessLogic.Handlers.Locations.GetCountries;
using TravelApp.BusinessLogic.Handlers.Users.GetUserProfile;
using TravelApp.BusinessLogic.Handlers.Users.GetUserProfiles;
using TravelApp.BusinessLogic.Interfaces;
using TravelApp.BusinessLogic.Models.Users;
using TravelApp.DataAccess.Auth;
using TravelApp.DataAccess.SQLRepositories.EntityFramework;

namespace TravelApp.DataAccess.SQLRepositories
{
    /// <summary>Implements database operations for Users using Entity Framework Core.</summary>
    internal class SqlUserRepository : IUserRepository
    {
        private readonly TravelAppContext _context;
        private readonly IPasswordHasher<UserAccount> _passwordHasher;

        /// <summary>Initializes a new instance of the <see cref="SqlUserRepository"/> class.</summary>
        /// <param name="context">The Entity Framework Core context connected to SQL Server.</param>
        /// <param name="passwordHasher">The hasher used for local login accounts.</param>
        public SqlUserRepository(TravelAppContext context, IPasswordHasher<UserAccount> passwordHasher)
        {
            _context = context;
            _passwordHasher = passwordHasher;
        }

        #region Create - Users

        /// <inheritdoc />
        public async Task AddAsync(TravelUser user)
        {
            await _context.TravelUserEntities.AddAsync(user);
        }

        /// <inheritdoc />
        public async Task AddWithAccountAsync(TravelUser user, string userName, string password)
        {
            var account = new UserAccount
            {
                Profile = user,
                UserName = userName,
                NormalizedUserName = userName.ToUpperInvariant(),
                Role = "User"
            };

            account.PasswordHash = _passwordHasher.HashPassword(account, password);

            await AddAsync(user);
            _context.Set<UserAccount>().Add(account);
        }

        #endregion

        #region Read - All Profiles & VisitedCountries

        /// <inheritdoc />
        public async Task<List<GetUserProfilesResponse>> GetProfilesAsync(GetUserProfilesRequest request, int viewerUserId, bool includePrivate)
        {
            IQueryable<GetUserProfileResponse> profiles = _context.UserProfiles.AsNoTracking();

            if (includePrivate is false)
            {
                profiles = profiles.Where(profile => profile.IsPrivate == false || profile.UserId == viewerUserId);
            }

            if (string.IsNullOrWhiteSpace(request.Search) is false)
            {
                string search = request.Search.Trim();
                profiles = profiles.Where(profile => profile.FullName.Contains(search));
            }

            if (request.Descending)
            {
                profiles = profiles.OrderByDescending(profile => profile.UserId);
            }
            else
            {
                profiles = profiles.OrderBy(profile => profile.UserId);
            }

            IQueryable<GetUserProfilesResponse> userProfiles = profiles.Select(profile => new GetUserProfilesResponse
            {
                UserId = profile.UserId,
                FullName = profile.FullName,
                Age = profile.Age,
                HomeCity = profile.HomeCity,
                HomeCountry = profile.HomeCountry,
                IsPrivate = profile.IsPrivate,
                MemberSince = profile.MemberSince,
                FriendCount = profile.FriendCount,
                CompletedTrips = profile.CompletedTrips,
                PlannedTrips = profile.PlannedTrips,
                ReviewCount = profile.ReviewCount,
                AchievementCount = profile.AchievementCount
            });

            List<GetUserProfilesResponse> result = await userProfiles.ToListAsync();

            return result;
        }

        /// <inheritdoc />
        public async Task<List<GetCountriesResponse>> GetVisitedCountriesAsync(int userId)
        {
            IQueryable<GetCountriesResponse> visitedCountries =
                from participant in _context.TripParticipantEntities.AsNoTracking()
                join stop in _context.TripStopEntities.AsNoTracking()
                    on participant.Trip!.Id equals stop.Trip!.Id
                where participant.User!.Id == userId && participant.Trip!.IsCompleted
                select new GetCountriesResponse
                {
                    Id = stop.City!.Country!.Id,
                    ContinentId = stop.City.Country.Continent!.Id,
                    Name = stop.City.Country.Name
                };

            List<GetCountriesResponse> result = await visitedCountries
                .Distinct()
                .OrderBy(country => country.Name)
                .ToListAsync();

            return result;
        }

        #endregion

        #region Read - By ID

        /// <inheritdoc />
        public async Task<TravelUser?> GetByIdAsync(int userId)
        {
            TravelUser? entity = await _context.TravelUserEntities
                .FirstOrDefaultAsync(user => user.Id == userId);

            return entity;
        }

        /// <inheritdoc />
        public async Task<GetUserProfileResponse?> GetProfileAsync(int userId)
        {
            GetUserProfileResponse? matchingProfile = await _context.UserProfiles
                .AsNoTracking()
                .FirstOrDefaultAsync(profile => profile.UserId == userId);

            return matchingProfile;
        }

        #endregion

        #region Update - Users

        /// <inheritdoc />
        public Task UpdateAsync(TravelUser user)
        {
            // The handler updated an entity already tracked by this Unit of Work.
            return Task.CompletedTask;
        }

        #endregion

        #region Delete - Users

        /// <inheritdoc />
        public async Task<bool> DeleteAsync(int userId)
        {
            if (_context.Database.CurrentTransaction is null)
            {
                await _context.Database.BeginTransactionAsync();
            }

            int deletedRows = await _context.TravelUserEntities
                .Where(user => user.Id == userId)
                .ExecuteDeleteAsync();

            bool wasDeleted = deletedRows > 0;

            return wasDeleted;
        }

        #endregion
    }
}
