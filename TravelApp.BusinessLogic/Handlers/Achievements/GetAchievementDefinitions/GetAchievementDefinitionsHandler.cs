using TravelApp.BusinessLogic.Models;
using TravelApp.BusinessLogic.Interfaces;

namespace TravelApp.BusinessLogic.Handlers.Achievements
{
    /// <summary>Handles the get achievement definitions use case.</summary>
    public class GetAchievementDefinitionsHandler
    {
        private readonly IUnitOfWorkFactory _unitOfWorkFactory;
        /// <summary>Initializes a new GetAchievementDefinitionsHandler instance.</summary>
        public GetAchievementDefinitionsHandler(IUnitOfWorkFactory unitOfWorkFactory)
        {
            _unitOfWorkFactory = unitOfWorkFactory;
        }
        /// <summary>Retrieves achievement definitions.</summary>
        public async Task<List<GetAchievementDefinitionsResponse>> GetAchievementDefinitionsAsync()
        {
            await using IUnitOfWork unitOfWork = _unitOfWorkFactory.Create();

            List<Achievement> items = await unitOfWork.Achievements.GetDefinitionsAsync();
            List<GetAchievementDefinitionsResponse> result = [];

            foreach (Achievement item in items)
            {
                result.Add(new GetAchievementDefinitionsResponse
                {
                    Id = item.Id,
                    Name = item.Name,
                    Description = item.Description
                });
            }

            return result;
        }
    }
}
