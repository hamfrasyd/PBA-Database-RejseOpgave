
using Microsoft.EntityFrameworkCore;
using TravelApp.BusinessLogic.DTOs.Activities;
using TravelApp.BusinessLogic.DTOs.Pagination;
using TravelApp.BusinessLogic.DTOs.Trips;
using TravelApp.BusinessLogic.Interfaces;
using TravelApp.BusinessLogic.Models.Activities;
using TravelApp.BusinessLogic.Models.Trips;
using TravelApp.DataAccess.SQLRepositories.EntityFramework;

namespace TravelApp.DataAccess.SQLRepositories
{
    /// <summary>Implements database operations for activities using Entity Framework Core.</summary>
    public class SqlActivitiesRepository : IActivitiesRepository
    {
        private readonly TravelAppContext _context;

        /// <summary>Initializes a new instance of the <see cref="SqlActivitiesRepository"/> class.</summary>
        /// <param name="context">The Entity Framework Core context connected to SQL Server.</param>
        public SqlActivitiesRepository(TravelAppContext context)
        {
            _context = context;
        }

        #region Create - Activities

        /// <inheritdoc />
        public async Task CreateActivityAsync(Activity activity, int cityId)
        {
            await _context.ActivityEntities.AddAsync(activity);
            _context.Entry(activity).Property("CityId").CurrentValue = cityId;
        }

        #endregion

        #region Read - All Activities

        /// <inheritdoc />
        public async Task<PagedResultDTO<ActivityDTO>> GetActivitiesAsync(int? cityId, int pageNumber, int pageSize)
        {
            IQueryable<Activity> activities = _context.ActivityEntities.AsNoTracking();

            if (cityId.HasValue)
            {
                int filterCityId = cityId.Value;
                activities = activities.Where(activity => EF.Property<int>(activity, "CityId") == filterCityId);
            }
            int totalCount = await activities.CountAsync();

            List<ActivityDTO> items = await activities
                .OrderByDescending(activity => activity.Name)
                .ThenByDescending(activity => activity.Id)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .Select(activity => new ActivityDTO
                {
                    Id = activity.Id,
                    CityId = EF.Property<int>(activity, "CityId"),
                    Name = activity.Name,
                    Description = activity.Description
                })
                .ToListAsync();

            return new PagedResultDTO<ActivityDTO>
            {
                Items = items,
                TotalCount = totalCount,
                PageNumber = pageNumber,
                PageSize = pageSize
            }; ;
        }

        #endregion

        #region Read - By-ID Activities

        /// <inheritdoc />
        public async Task<ActivityDTO?> GetActivityByIdAsync(int activityId)
        {
            ActivityDTO? activityData = await _context.ActivityEntities
                .AsNoTracking()
                .Where(activity => activity.Id == activityId)
                .Select(activity => new ActivityDTO
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
        public async Task<bool> UpdateActivityAsync(ActivityDTO activityToUpdate)
        {
            Activity? activityInDb = await _context.ActivityEntities.FirstOrDefaultAsync(activity => activity.Id == activityToUpdate.Id);

            if (activityInDb is null)
            {
                return false;
            }

            activityInDb.Name = activityToUpdate.Name;
            activityInDb.Description = activityToUpdate.Description;
            _context.Entry(activityInDb).Property("CityId").CurrentValue = activityToUpdate.CityId;

            return true;
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