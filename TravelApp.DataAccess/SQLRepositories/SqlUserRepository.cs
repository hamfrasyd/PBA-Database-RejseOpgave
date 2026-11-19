using Microsoft.EntityFrameworkCore;
using TravelApp.BusinessLogic.DTOs.Locations;
using TravelApp.BusinessLogic.DTOs.Users;
using TravelApp.BusinessLogic.Exceptions.SpecificCustomException;
using TravelApp.BusinessLogic.Interfaces;
using TravelApp.BusinessLogic.Models.Users;
using TravelApp.DataAccess.SQLRepositories.EntityFramework;

namespace TravelApp.DataAccess.SQLRepositories
{
    /// <summary>Implements database operations for Users using Entity Framework Core.</summary>
    public class SqlUserRepository : IUserRepository
    {
        private readonly TravelAppContext _context;

        /// <summary>Initializes a new instance of the <see cref="SqlUserRepository"/> class.</summary>
        /// <param name="context">The Entity Framework Core context connected to SQL Server.</param>
        public SqlUserRepository(TravelAppContext context)
        {
            _context = context;
        }

        #region Create - Users

        /// <inheritdoc />
        public async Task AddAsync(TravelUser user)
        {
            await _context.TravelUserEntities.AddAsync(user);
        }

        #endregion

        #region Read - All Profiles & VisitedCountries

        /// <inheritdoc />
        public async Task<List<UserProfileView>> GetProfilesAsync(UserProfileSearchDTO profileSearch)
        {
            IQueryable<UserProfileView> profiles = _context.UserProfileViews.AsNoTracking();

            if (profileSearch.IncludePrivateProfiles is false)
            {
                profiles = profiles.Where(profile => profile.IsPrivate == false || profile.UserId == profileSearch.CurrentUserId);
            }

            if (string.IsNullOrWhiteSpace(profileSearch.Search) is false)
            {
                string search = profileSearch.Search.Trim();

                profiles = profiles.Where(profile => profile.FullName.Contains(search));
            }

            if (profileSearch.Descending)
            {
                profiles = profiles.OrderByDescending(profile => profile.UserId);
            }
            else
            {
                profiles = profiles.OrderBy(profile => profile.UserId);
            }

            List<UserProfileView> result = await profiles.ToListAsync();

            return result;
        }

        /// <inheritdoc />
        public async Task<List<VisitedCountryDTO>> GetVisitedCountriesAsync(int userId)
        {
            // The ! only reassures the compiler. These links are always stored, and a missing one is skipped instead of crashing.
            IQueryable<VisitedCountryDTO> visitedCountries = _context.TripParticipantEntities.AsNoTracking()
                .Where(participant => participant.User!.Id == userId && participant.Trip!.IsCompleted)
                .SelectMany(participant => participant.Trip!.TripStops)
                .Select(stop => new VisitedCountryDTO
                {
                    Id = stop.City!.Country!.Id,
                    ContinentId = stop.City.Country.Continent!.Id,
                    Name = stop.City.Country.Name
                });

            List<VisitedCountryDTO> result = await visitedCountries.Distinct().OrderBy(country => country.Name).ToListAsync();

            return result;
        }

        #endregion

        #region Read - By ID

        /// <inheritdoc />
        public async Task<bool> UserExistsAsync(int userId)
        {
            bool userExists = await _context.TravelUserEntities.AsNoTracking().AnyAsync(user => user.Id == userId);

            return userExists;
        }

        /// <inheritdoc />
        public async Task<TravelUser?> GetByIdAsync(int userId)
        {
            TravelUser? entity = await _context.TravelUserEntities.FirstOrDefaultAsync(user => user.Id == userId);

            return entity;
        }

        /// <inheritdoc />
        public async Task<UserProfileView?> GetProfileAsync(int userId)
        {
            UserProfileView? matchingProfile = await _context.UserProfileViews.AsNoTracking().FirstOrDefaultAsync(profile => profile.UserId == userId);

            return matchingProfile;
        }

        #endregion

        #region Update - Users

        /// <inheritdoc />
        public async Task<bool> UpdateUserAsync(UserBasicInfoDto userToUpdate)
        {
            TravelUser? userInDb = await _context.TravelUserEntities.FirstOrDefaultAsync(user => user.Id == userToUpdate.Id);

            if (userInDb is null)
            {
                return false;
            }

            if (string.IsNullOrEmpty(userToUpdate.FirstName) is false)
            {
                userInDb.FirstName = userToUpdate.FirstName.Trim();
            }

            if (string.IsNullOrEmpty(userToUpdate.LastName) is false)
            {
                userInDb.LastName = userToUpdate.LastName.Trim();
            }

            if (userToUpdate.IsPrivate is not null)
            {
                userInDb.IsPrivate = userToUpdate.IsPrivate.Value;
            }

            _context.Entry(userInDb).Property("CityId").CurrentValue = userToUpdate.CityId;

            return true;
        }

        #endregion

        #region Delete - Users

        /// <inheritdoc />
        public async Task DeleteAsync(int userId)
        {
            TravelUser? user = await _context.TravelUserEntities.FirstOrDefaultAsync(user => user.Id == userId);

            if (user is null)
            {
                throw new TravelAppNotFoundException($"User with ID {userId} was not found.");
            }

            _context.TravelUserEntities.Remove(user);
        }

        #endregion
    }
}
