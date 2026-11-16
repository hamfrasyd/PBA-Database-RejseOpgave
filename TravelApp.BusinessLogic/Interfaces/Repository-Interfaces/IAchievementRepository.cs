using TravelApp.BusinessLogic.DTOs.Achievements;

namespace TravelApp.BusinessLogic.Interfaces
{
    /// <summary>Defines database operations for achievements.</summary>
    public interface IAchievementRepository
    {
        #region Read - All Achievements
        
        /// <summary>Retrieves achievement definitions.</summary>
        /// <returns>The achievement definitions.</returns>
        public Task<List<AchievementDto>> GetDefinitionsAsync();

        #endregion

        #region Read - By-ID Earned Achievements for a User

        /// <summary>Retrieves achievements earned by a user.</summary>
        /// <param name="userId">The user identifier.</param>
        /// <returns>The achievements earned by the user.</returns>
        public Task<List<EarnedAchievementDto>> GetEarnedByUserAsync(int userId);

        #endregion
    }
}
