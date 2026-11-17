using TravelApp.BusinessLogic.Exceptions.SpecificCustomException;
using TravelApp.BusinessLogic.Interfaces;
using TravelApp.BusinessLogic.Models.Destinations;

namespace TravelApp.BusinessLogic.Handlers.Activities.UpdateActivity
{
    /// <summary>Handles the update activity use case.</summary>
    public class UpdateActivityHandler
    {
        private readonly IUnitOfWorkFactory _unitOfWorkFactory;

        /// <summary>Initializes a new UpdateActivityHandler instance.</summary>
        public UpdateActivityHandler(IUnitOfWorkFactory unitOfWorkFactory)
        {
            _unitOfWorkFactory = unitOfWorkFactory;
        }

        /// <summary>Updates activity.</summary>
        public async Task UpdateActivityAsync(int activityId, UpdateActivityRequest request)
        {
            await using IUnitOfWork unitOfWork = _unitOfWorkFactory.Create();

            Activity? activityromDbToUpdate = await unitOfWork.Activities.GetActivityByIdAsync(activityId);

            if (activityromDbToUpdate is null)
            {
                throw new TravelAppNotFoundException($"Activity with ID {activityId} was not found.");
            }

            if (string.IsNullOrWhiteSpace(request.Name))
            {
                throw new TravelAppValidationException("Activity name is required.");
            }

            City? cityFromDb = await unitOfWork.Locations.GetCityByIdAsync(request.CityId);

            if (cityFromDb is null)
            {
                throw new TravelAppValidationException("The parent destination does not exist.");
            }

            activityromDbToUpdate.Name = request.Name.Trim();
            activityromDbToUpdate.City = cityFromDb;
            activityromDbToUpdate.Description = request.Description;

            await unitOfWork.Activities.UpdateActivityAsync(activityId, request);

            await unitOfWork.CommitAsync();
        }
    }
}
