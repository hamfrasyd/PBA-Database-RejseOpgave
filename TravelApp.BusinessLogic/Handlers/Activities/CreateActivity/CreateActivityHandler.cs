using TravelApp.BusinessLogic.Exceptions.SpecificCustomException;
using TravelApp.BusinessLogic.Interfaces;
using TravelApp.BusinessLogic.Models.Destinations;

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

        /// <summary>Creates activity.</summary>
        /// <param name="request">The activity to create.</param>
        /// <returns>The identifier of the created activity.</returns>
        /// <exception cref="TravelAppValidationException">Thrown when the name is missing or the city does not exist.</exception>
        public async Task<int> CreateActivityAsync(CreateActivityRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.Name))
            {
                throw new TravelAppValidationException("Activity name is required.");
            }

            await using IUnitOfWork unitOfWork = _unitOfWorkFactory.Create();

            City? cityFromDb = await unitOfWork.Locations.GetCityByIdAsync(request.CityId);

            if (cityFromDb is null)
            {
                throw new TravelAppValidationException("The parent destination does not exist.");
            }

            Activity createdActivity = new Activity
            {
                Name = request.Name.Trim(),
                City = cityFromDb,
                Description = request.Description
            };

            await unitOfWork.Activities.AddActivityAsync(createdActivity);

            await unitOfWork.CommitAsync();

            return createdActivity.Id;
        }
    }
}
