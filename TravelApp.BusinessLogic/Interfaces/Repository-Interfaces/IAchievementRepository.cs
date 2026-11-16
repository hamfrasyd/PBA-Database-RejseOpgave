using TravelApp.BusinessLogic.Handlers.Achievements.GetEarnedAchievements;
using TravelApp.BusinessLogic.Models.Achievements;

namespace TravelApp.BusinessLogic.Interfaces
{
    /// <summary>Defines database operations for achievements.</summary>
    public interface IAchievementRepository
    {
        #region Read - Achievements
        /// <summary>Retrieves definitions.</summary>
        public Task<List<Achievement>> GetDefinitionsAsync();
        /// <summary>Retrieves earned by user.</summary>
        public Task<List<GetEarnedAchievementsResponse>> GetEarnedByUserAsync(int userId);

        #endregion
    }
}
