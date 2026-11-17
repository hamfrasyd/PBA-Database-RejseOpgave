using TravelApp.BusinessLogic.DTOs.Activities;
using TravelApp.BusinessLogic.DTOs.Locations;
using TravelApp.BusinessLogic.Exceptions.SpecificCustomException;
using TravelApp.BusinessLogic.Interfaces;

namespace TravelApp.BusinessLogic.Handlers.Activities.UpdateActivity
{
    /// <summary>Handles the update activity use case.</summary>
    public class UpdateActivityHandler
    {
        private readonly IUnitOfWorkFactory _unitOfWorkFactory;

        /// <summary>Initializes a new UpdateActivityHandler instance.</summary>
        /// <param name="unitOfWorkFactory">The unit of work factory.</param>
        public UpdateActivityHandler(IUnitOfWorkFactory unitOfWorkFactory)
        {
            _unitOfWorkFactory = unitOfWorkFactory;
        }

        /// <summary>Updates an activity and returns the persisted result.</summary>
        /// <param name="activityId">The activity identifier.</param>
        /// <param name="request">The updated activity details.</param>
        /// <returns>The updated activity.</returns>
        /// <exception cref="TravelAppNotFoundException">Thrown when the activity does not exist.</exception>
        /// <exception cref="TravelAppValidationException">Thrown when the name is missing or the city does not exist.</exception>
        public async Task<UpdateActivityResponse> UpdateActivityAsync(int activityId, UpdateActivityRequest request)
        {
            await using IUnitOfWork unitOfWork = _unitOfWorkFactory.Create();

            CityDTO? existingCity = await unitOfWork.Locations.GetCityAsync(request.CityId);
            if (existingCity is null)
            {
                throw new TravelAppValidationException("The City for the activity does not exist.");
            }

            ActivityDTO activityToUpdate = new ActivityDTO
            {
                Id = activityId,
                CityId = request.CityId,
                Name = request.Name.Trim(),
                Description = request.Description
            };

            ActivityDTO? updatedActivity = await unitOfWork.Activities.UpdateActivityAsync(activityToUpdate);
            if (updatedActivity is null)
            {
                throw new TravelAppNotFoundException($"Activity with ID {activityId} was not found.");
            }

            await unitOfWork.CommitAsync();

            UpdateActivityResponse response = new UpdateActivityResponse
            {
                Id = updatedActivity.Id,
                CityId = updatedActivity.CityId,
                Name = updatedActivity.Name,
                Description = updatedActivity.Description
            };

            return response;
        }
    }
}