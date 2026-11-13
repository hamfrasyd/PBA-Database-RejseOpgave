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

        /// <summary>Initializes a new SqlAchievementRepository instance.</summary>
        public SqlAchievementRepository(TravelAppContext context)
        {
            _context = context;
        }

        /// <summary>Retrieves definitions.</summary>
        public async Task<List<Achievement>> GetDefinitionsAsync()
        {
            List<Achievement> result = await _context.AchievementEntities.AsNoTracking().OrderBy(item => item.Name).ToListAsync();
            return result;
        }
    }
}
