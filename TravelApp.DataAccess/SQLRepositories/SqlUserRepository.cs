using Microsoft.EntityFrameworkCore;
using TravelApp.BusinessLogic.DTOs.Users;
using TravelApp.BusinessLogic.Handlers.Locations.Data;
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
        public async Task<List<UserProfileDetailsDTO>> GetProfilesAsync(UserProfileSearchDTO profileSearch)
        {
            IQueryable<UserProfileDetailsDTO> profiles = _context.UserProfiles.AsNoTracking();

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

            List<UserProfileDetailsDTO> result = await profiles.ToListAsync();

            return result;
        }

        /// <inheritdoc />
        public async Task<List<VisitedCountryDto>> GetVisitedCountriesAsync(int userId)
        {
            // The ! only reassures the compiler. These links are always stored, and a missing one is skipped instead of crashing.
            IQueryable<VisitedCountryDto> visitedCountries = _context.TripParticipantEntities.AsNoTracking()
                .Where(participant => participant.User!.Id == userId && participant.Trip!.IsCompleted)
                .SelectMany(participant => participant.Trip!.TripStops)
                .Select(stop => new VisitedCountryDto
                {
                    Id = stop.City!.Country!.Id,
                    ContinentId = stop.City.Country.Continent!.Id,
                    Name = stop.City.Country.Name
                });

            List<VisitedCountryDto> result = await visitedCountries.Distinct().OrderBy(country => country.Name).ToListAsync();

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
        public async Task<UserProfileDetailsDTO?> GetProfileAsync(int userId)
        {
            UserProfileDetailsDTO? matchingProfile = await _context.UserProfiles.AsNoTracking().FirstOrDefaultAsync(profile => profile.UserId == userId);

            return matchingProfile;
        }

        #endregion

        #region Update - Users

        /// <inheritdoc />
        public async Task<UserBasicInfoDto?> UpdateUserAsync(UserBasicInfoDto userToUpdate)
        {
            TravelUser? userInDb = await _context.TravelUserEntities.FirstOrDefaultAsync(user => user.Id == userToUpdate.Id);

            if (userInDb is null)
            {
                return null;
            }

            userInDb.FirstName = userToUpdate.FirstName;
            userInDb.LastName = userToUpdate.LastName;
            userInDb.IsPrivate = userToUpdate.IsPrivate;
            _context.Entry(userInDb).Property("CityId").CurrentValue = userToUpdate.CityId;

            return userToUpdate;
        }

        #endregion

        #region Delete - Users

        /// <inheritdoc />
        public async Task DeleteAsync(int userId)
        {
            TravelUser? user = await _context.TravelUserEntities.FirstOrDefaultAsync(item => item.Id == userId);

            if (user is not null)
            {
                _context.TravelUserEntities.Remove(user);
            }
        }

        #endregion
    }
}
