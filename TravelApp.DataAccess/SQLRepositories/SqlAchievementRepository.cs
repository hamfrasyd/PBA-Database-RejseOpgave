using Microsoft.EntityFrameworkCore;
using TravelApp.BusinessLogic.DTOs.Achievements;
using TravelApp.BusinessLogic.Exceptions.SpecificCustomException;
using TravelApp.BusinessLogic.Interfaces;
using TravelApp.BusinessLogic.Models.Achievements;
using TravelApp.DataAccess.SQLRepositories.EntityFramework;

namespace TravelApp.DataAccess.SQLRepositories
{
    /// <summary>Implements database operations for achievements using Entity Framework Core.</summary>
    public class SqlAchievementRepository : IAchievementRepository
    {
        private readonly TravelAppContext _context;

        /// <summary>Initializes a new instance of the <see cref="SqlAchievementRepository"/> class.</summary>
        /// <param name="context">The Entity Framework Core context connected to SQL Server.</param>
        public SqlAchievementRepository(TravelAppContext context)
        {
            _context = context;
        }

        /// <inheritdoc />
        public async Task<List<AchievementDto>> GetDefinitionsAsync()
        {
            List<Achievement> achievements = await _context.AchievementEntities.AsNoTracking().OrderBy(achievement => achievement.Name).ToListAsync();
            List<AchievementDto> result = achievements.Select(achievement => new AchievementDto
            {
                Id = achievement.Id,
                Name = achievement.Name,
                Description = achievement.Description
            }).ToList();

            return result;
        }

        /// <inheritdoc />
        public async Task<List<EarnedAchievementDto>> GetEarnedByUserAsync(int userId)
        {
            List<EarnedAchievement> earnedAchievementList = await _context.EarnedAchievementEntities.AsNoTracking()
                .Include(earnedAchievement => earnedAchievement.Achievement)
                .Where(earnedAchievement => earnedAchievement.User != null && earnedAchievement.User.Id == userId)
                .OrderByDescending(earnedAchievement => earnedAchievement.EarnedAt)
                .ToListAsync();

            List<EarnedAchievementDto> responseList = [];

            foreach (EarnedAchievement earnedAchiev in earnedAchievementList)
            {
                Achievement? achiev = earnedAchiev.Achievement;
                if (achiev is null)
                {
                    throw new TravelAppNotFoundException($"An earned achievement for user {userId} references a missing achievement definition.");
                }

                EarnedAchievementDto response = new EarnedAchievementDto
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
