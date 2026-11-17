
using Microsoft.EntityFrameworkCore;
using TravelApp.BusinessLogic.DTOs.Activities;
using TravelApp.BusinessLogic.Interfaces;
using TravelApp.BusinessLogic.Models.Destinations;
using TravelApp.DataAccess.SQLRepositories.EntityFramework;

namespace TravelApp.DataAccess.SQLRepositories
{
    /// <summary>Implements database operations for activities using Entity Framework Core.</summary>
    public class SqlActivitiesRepository : IActivitiesRepository
    {
        private readonly TravelAppContext _context;
        private Activity? _addedActivity;

        /// <summary>Initializes a new instance of the <see cref="SqlActivitiesRepository"/> class.</summary>
        /// <param name="context">The Entity Framework Core context connected to SQL Server.</param>
        public SqlActivitiesRepository(TravelAppContext context)
        {
            _context = context;
        }

        #region Create - Activities

        /// <inheritdoc />
        public async Task<ActivityDto?> CreateActivityAsync(CreateActivityDto activityToCreate)
        {
            _addedActivity = new Activity
            {
                Name = activityToCreate.Name,
                Description = activityToCreate.Description
            };

            await _context.ActivityEntities.AddAsync(_addedActivity);
            _context.Entry(_addedActivity).Property("CityId").CurrentValue = activityToCreate.CityId;

            // cowboy, skal bruge cecilias måde at Create på
            int cityId = _context.Entry(_addedActivity).Property<int>("CityId").CurrentValue;

            if (_addedActivity.Id <= 0)
            {
                return null;
            }

            ActivityDto createdActivity = new ActivityDto
            {
                Id = _addedActivity.Id,
                CityId = cityId,
                Name = _addedActivity.Name,
                Description = _addedActivity.Description
            };

            return createdActivity;
        }

        /// <inheritdoc />
        public ActivityDto GetCreatedActivity()
        {
            if (_addedActivity is null)
            {
                throw new InvalidOperationException("CreateActivityAsync must be called before GetCreatedActivity.");
            }

            int cityId = _context.Entry(_addedActivity).Property<int>("CityId").CurrentValue;

            ActivityDto createdActivity = new ActivityDto
            {
                Id = _addedActivity.Id,
                CityId = cityId,
                Name = _addedActivity.Name,
                Description = _addedActivity.Description
            };

            return createdActivity;
        }

        #endregion

        #region Read - All Activities

        /// <inheritdoc />
        public async Task<List<ActivityDto>> GetActivitiesAsync(int? cityId)
        {
            IQueryable<Activity> activities = _context.ActivityEntities.AsNoTracking();

            if (cityId.HasValue)
            {
                int filterCityId = cityId.Value;
                activities = activities.Where(activity => EF.Property<int>(activity, "CityId") == filterCityId);
            }

            List<ActivityDto> result = await activities
                .OrderBy(activity => activity.Name)
                .ThenBy(activity => activity.Id)
                .Select(activity => new ActivityDto
                {
                    Id = activity.Id,
                    CityId = EF.Property<int>(activity, "CityId"),
                    Name = activity.Name,
                    Description = activity.Description
                })
                .ToListAsync();

            return result;
        }

        #endregion

        #region Read - By-ID Activities

        /// <inheritdoc />
        public async Task<ActivityDto?> GetActivityByIdAsync(int activityId)
        {
            ActivityDto? activityData = await _context.ActivityEntities
                .AsNoTracking()
                .Where(activity => activity.Id == activityId)
                .Select(activity => new ActivityDto
                {
                    Id = activity.Id,
                    CityId = EF.Property<int>(activity, "CityId"),
                    Name = activity.Name,
                    Description = activity.Description
                })
                .FirstOrDefaultAsync();

            return activityData;
        }

        #endregion

        #region Update - Activities

        /// <inheritdoc />
        public async Task<ActivityDto?> UpdateActivityAsync(UpdateActivityDto activityToUpdate)
        {
            Activity? activityInDb = await _context.ActivityEntities.FirstOrDefaultAsync(activity => activity.Id == activityToUpdate.Id);

            if (activityInDb is null)
            {
                return null;
            }

            activityInDb.Name = activityToUpdate.Name;
            activityInDb.Description = activityToUpdate.Description;
            _context.Entry(activityInDb).Property("CityId").CurrentValue = activityToUpdate.CityId;

            ActivityDto updatedActivity = new ActivityDto
            {
                Id = activityInDb.Id,
                CityId = activityToUpdate.CityId,
                Name = activityInDb.Name,
                Description = activityInDb.Description
            };

            return updatedActivity;
        }

        #endregion

        #region Delete - Activities

        /// <inheritdoc />
        public async Task DeleteAsync(int activityId)
        {
            Activity? activity = await _context.ActivityEntities.FirstOrDefaultAsync(item => item.Id == activityId);

            if (activity is not null)
            {
                _context.ActivityEntities.Remove(activity);
            }
        }

        #endregion
    }
}