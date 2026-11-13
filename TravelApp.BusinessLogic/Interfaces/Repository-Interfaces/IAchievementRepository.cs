using TravelApp.BusinessLogic.Handlers.Achievements;
using TravelApp.BusinessLogic.Models;

namespace TravelApp.BusinessLogic.Interfaces
{
    /// <summary>Defines database operations for achievements.</summary>
    public interface IAchievementRepository
    {
        /// <summary>Retrieves definitions.</summary>
        public Task<List<Achievement>> GetDefinitionsAsync();
        /// <summary>Retrieves earned by user.</summary>
        public Task<List<GetEarnedAchievementsResponse>> GetEarnedByUserAsync(int userId);
    }
}
