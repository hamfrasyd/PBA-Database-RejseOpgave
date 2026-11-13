using Microsoft.EntityFrameworkCore;
using TravelApp.BusinessLogic.Handlers.Achievements;
using TravelApp.BusinessLogic.Interfaces;
using TravelApp.BusinessLogic.Models;
using TravelApp.DataAccess.SQLRepositories.EntityFramework;

namespace TravelApp.DataAccess.SQLRepositories
{
    public class SqlAchievementRepository : IAchievementRepository
    {
        private readonly TravelAppContext _context;

        public SqlAchievementRepository(TravelAppContext context)
        {
            _context = context;
        }

        public async Task<List<Achievement>> GetDefinitionsAsync()
        {
            List<Achievement> result = await _context.AchievementEntities.AsNoTracking().OrderBy(item => item.Name).ToListAsync();
            return result;
        }

        public async Task<List<GetEarnedAchievementsResponse>> GetEarnedByUserAsync(int userId)
        {
            IQueryable<GetEarnedAchievementsResponse> earnedAchievements = _context.EarnedAchievementEntities
                .AsNoTracking()
                .Where(earned => earned.User!.Id == userId)
                .OrderByDescending(earned => earned.EarnedAt)
                .Select(earned => new GetEarnedAchievementsResponse
                {
                    AchievementId = earned.Achievement!.Id,
                    Name = earned.Achievement.Name,
                    Description = earned.Achievement.Description,
                    EarnedAt = earned.EarnedAt
                });

            List<GetEarnedAchievementsResponse> result = await earnedAchievements.ToListAsync();
            return result;
        }
    }
}
