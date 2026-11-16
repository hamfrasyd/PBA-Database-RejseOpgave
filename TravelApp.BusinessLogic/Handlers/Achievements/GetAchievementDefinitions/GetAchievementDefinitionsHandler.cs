using TravelApp.BusinessLogic.DTOs.Achievements;
using TravelApp.BusinessLogic.Interfaces;

namespace TravelApp.BusinessLogic.Handlers.Achievements.GetAchievementDefinitions
{
    /// <summary>Handles the get achievement definitions use case.</summary>
    public class GetAchievementDefinitionsHandler
    {
        private readonly IUnitOfWorkFactory _unitOfWorkFactory;

        /// <summary>Initializes a new GetAchievementDefinitionsHandler instance.</summary>
        /// <param name="unitOfWorkFactory">The unit of work factory.</param>
        public GetAchievementDefinitionsHandler(IUnitOfWorkFactory unitOfWorkFactory)
        {
            _unitOfWorkFactory = unitOfWorkFactory;
        }

        /// <summary>Retrieves achievement definitions.</summary>
        /// <returns>The achievement definitions.</returns>
        public async Task<List<GetAchievementDefinitionsResponse>> GetAchievementDefinitionsAsync()
        {
            await using IUnitOfWork unitOfWork = _unitOfWorkFactory.Create();

            List<AchievementDto> achievements = await unitOfWork.Achievements.GetDefinitionsAsync();
            List<GetAchievementDefinitionsResponse> response = [];

            foreach (AchievementDto achiev in achievements)
            {
                GetAchievementDefinitionsResponse definition = new GetAchievementDefinitionsResponse
                {
                    Id = achiev.Id,
                    Name = achiev.Name,
                    Description = achiev.Description
                };

                response.Add(definition);
            }

            return response;
        }
    }
}