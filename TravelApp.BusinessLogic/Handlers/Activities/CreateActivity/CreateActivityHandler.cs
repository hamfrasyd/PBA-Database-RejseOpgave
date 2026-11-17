using TravelApp.BusinessLogic.DTOs.Activities;
using TravelApp.BusinessLogic.Exceptions.SpecificCustomException;
using TravelApp.BusinessLogic.Handlers.Locations.Data;
using TravelApp.BusinessLogic.Interfaces;

namespace TravelApp.BusinessLogic.Handlers.Activities.CreateActivity
{
    /// <summary>Handles the create activity use case.</summary>
    public class CreateActivityHandler
    {
        private readonly IUnitOfWorkFactory _unitOfWorkFactory;

        /// <summary>Initializes a new CreateActivityHandler instance.</summary>
        /// <param name="unitOfWorkFactory">The unit of work factory.</param>
        public CreateActivityHandler(IUnitOfWorkFactory unitOfWorkFactory)
        {
            _unitOfWorkFactory = unitOfWorkFactory;
        }

        /// <summary>Creates an activity and returns the persisted result.</summary>
        /// <param name="request">The activity to create.</param>
        /// <returns>The created activity.</returns>
        /// <exception cref="TravelAppValidationException">Thrown when the name is missing or the city does not exist.</exception>
        public async Task<CreateActivityResponse> CreateActivityAsync(CreateActivityRequest request)
        {
            await using IUnitOfWork unitOfWork = _unitOfWorkFactory.Create();

            CityDto? existingCity = await unitOfWork.Locations.GetCityAsync(request.CityId);
            if (existingCity is null)
            {
                throw new TravelAppValidationException("The City for the activity does not exist.");
            }

            CreateActivityDto activityToCreate = new CreateActivityDto
            {
                CityId = request.CityId,
                Name = request.Name.Trim(),
                Description = request.Description
            };

            ActivityDto? createdActivity = await unitOfWork.Activities.CreateActivityAsync(activityToCreate);

            await unitOfWork.CommitAsync();

            // cowboy, skal bruge cecilias måde at Create på
            if (createdActivity is null)
            {
                createdActivity = unitOfWork.Activities.GetCreatedActivity();
            }

            CreateActivityResponse response = new CreateActivityResponse
            {
                Id = createdActivity.Id,
                CityId = createdActivity.CityId,
                Name = createdActivity.Name,
                Description = createdActivity.Description
            };

            return response;
        }
    }
}