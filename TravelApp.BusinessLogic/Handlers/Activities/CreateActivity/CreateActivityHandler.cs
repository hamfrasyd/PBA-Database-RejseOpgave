using TravelApp.BusinessLogic.DTOs.Locations;
using TravelApp.BusinessLogic.Exceptions.SpecificCustomException;
using TravelApp.BusinessLogic.Interfaces;
using TravelApp.BusinessLogic.Models.Activities;

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
        /// <exception cref="TravelAppValidationException">Thrown when the city does not exist.</exception>
        public async Task<CreateActivityResponse> CreateActivityAsync(CreateActivityRequest request)
        {
            await using IUnitOfWork unitOfWork = _unitOfWorkFactory.Create();

            CityDTO? existingCity = await unitOfWork.Locations.GetCityAsync(request.CityId);
            if (existingCity is null)
            {
                throw new TravelAppValidationException("The City for the activity does not exist.");
            }

            Activity activity = new Activity
            {
                Name = request.Name.Trim(),
                Description = request.Description
            };

            await unitOfWork.Activities.CreateActivityAsync(activity, request.CityId);

            await unitOfWork.CommitAsync();

            CreateActivityResponse response = new CreateActivityResponse
            {
                Id = activity.Id,
                CityId = request.CityId,
                Name = activity.Name,
                Description = activity.Description
            };

            return response;
        }
    }
}