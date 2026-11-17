using TravelApp.BusinessLogic.DTOs;

namespace TravelApp.BusinessLogic.Interfaces
{
    /// <summary>Defines database operations for achievements.</summary>
    public interface IAchievementRepository
    {
        #region Read - Achievements
        /// <summary>Retrieves definitions.</summary>
        public Task<List<AchievementDto>> GetDefinitionsAsync();

        /// <summary>Retrieves earned by user.</summary>
        public Task<List<EarnedAchievementDto>> GetEarnedByUserAsync(int userId);
        #endregion
    }
}
