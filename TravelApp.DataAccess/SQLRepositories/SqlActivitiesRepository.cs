
using Microsoft.EntityFrameworkCore;
using TravelApp.BusinessLogic.Handlers.Activities.UpdateActivity;
using TravelApp.BusinessLogic.Interfaces;
using TravelApp.BusinessLogic.Models.Destinations;
using TravelApp.DataAccess.SQLRepositories.EntityFramework;

namespace TravelApp.DataAccess.SQLRepositories
{
    public class SqlActivitiesRepository : IActivitiesRepository
    {
        private readonly TravelAppContext _context;

        public SqlActivitiesRepository(TravelAppContext context)
        {
            _context = context;
        }

        #region Create - Activities

        public async Task AddActivityAsync(Activity activity)
        {
            await _context.ActivityEntities.AddAsync(activity);
        }

        #endregion

        #region Read - Activities

        public async Task<List<Activity>> GetActivitiesAsync(int? cityId)
        {
            IQueryable<Activity> activities = _context.ActivityEntities
                .AsNoTracking()
                .Include(activity => activity.City);

            if (cityId.HasValue)
            {
                activities = activities.Where(activity => activity.City!.Id == cityId.Value);
            }

            List<Activity> result = await activities
                .OrderBy(activity => activity.Name)
                .ThenBy(activity => activity.Id)
                .ToListAsync();

            return result;
        }

        public async Task<Activity?> GetActivityByIdAsync(int activityId)
        {
            Activity? matchingActivity = await _context.ActivityEntities
                .Include(activity => activity.City)
                .FirstOrDefaultAsync(activity => activity.Id == activityId);

            return matchingActivity;
        }

        #endregion

        #region Update - Activities

        public Task UpdateActivityAsync(int activityId, UpdateActivityRequest request)
        {
            // The handler updated an entity already tracked by this Unit of Work.
            return Task.CompletedTask;
        }

        #endregion

        #region Delete - Activities

        public async Task<bool> DeleteActivityAsync(int activityId)
        {
            if (_context.Database.CurrentTransaction is null)
            {
                await _context.Database.BeginTransactionAsync();
            }

            int deletedRows = await _context.ActivityEntities
                .Where(activity => activity.Id == activityId)
                .ExecuteDeleteAsync();

            bool wasDeleted = deletedRows > 0;

            return wasDeleted;
        }

        #endregion

        #region Validation - Activities

        public async Task<bool> ActivityBelongsToCityAsync(int activityId, int cityId)
        {
            bool activityBelongsToCity = await _context.ActivityEntities
                .AnyAsync(activity => activity.Id == activityId && activity.City!.Id == cityId);

            return activityBelongsToCity;
        }

        #endregion
    }
}
