using Microsoft.EntityFrameworkCore;
using TravelApp.BusinessLogic.Handlers.Achievements;
using TravelApp.BusinessLogic.Interfaces;
using TravelApp.BusinessLogic.Models;
using TravelApp.DataAccess.SQLRepositories.EntityFramework;

namespace TravelApp.DataAccess.SQLRepositories
{
    /// <inheritdoc />
    public class SqlAchievementRepository : IAchievementRepository
    {
        private readonly TravelAppContext _context;

        /// <summary>Initializes a new instance of the <see cref="SqlAchievementRepository"/> class.</summary>
        /// <param name="context">The database context.</param>
        public SqlAchievementRepository(TravelAppContext context)
        {
            _context = context;
        }

        /// <inheritdoc />
        public async Task<List<Achievement>> GetDefinitionsAsync()
        {
            List<Achievement> result = await _context.AchievementEntities.AsNoTracking().OrderBy(achievement => achievement.Name).ToListAsync();
            return result;
        }

        /// <inheritdoc />
        public async Task<List<GetEarnedAchievementsResponse>> GetEarnedByUserAsync(int userId)
        {
            List<EarnedAchievement> earnedAchievementList = await _context.EarnedAchievementEntities.AsNoTracking()
                .Include(earnedAchievement => earnedAchievement.Achievement)
                .Where(earnedAchievement => earnedAchievement.User != null && earnedAchievement.User.Id == userId)
                .OrderByDescending(earnedAchievement => earnedAchievement.EarnedAt)
                .ToListAsync();

            List<GetEarnedAchievementsResponse> responseList = [];

            foreach (EarnedAchievement earnedAchiev in earnedAchievementList)
            {
                Achievement? achiev = earnedAchiev.Achievement;
                if (achiev is null)
                {
                    throw new InvalidOperationException($"An earned achievement for user {userId} references a missing achievement definition.");
                }

                GetEarnedAchievementsResponse response = new GetEarnedAchievementsResponse
                {
                    AchievementId = achiev.Id,
                    Name = achiev.Name,
                    Description = achiev.Description,
                    EarnedAt = earnedAchiev.EarnedAt
                };

                responseList.Add(response);
            }

            return responseList;
        }
    }
}
