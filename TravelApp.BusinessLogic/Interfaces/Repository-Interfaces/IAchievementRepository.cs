using TravelApp.BusinessLogic.DTOs;
using TravelApp.BusinessLogic.Models;

namespace TravelApp.BusinessLogic.Interfaces
{
    /// <summary>Defines database operations for achievements.</summary>
    public interface IAchievementRepository
    {
        /// <summary>Retrieves definitions.</summary>
        public Task<List<AchievementDto>> GetDefinitionsAsync();

        /// <summary>Retrieves earned by user.</summary>
        public Task<List<EarnedAchievementDto>> GetEarnedByUserAsync(int userId);
    }
}
