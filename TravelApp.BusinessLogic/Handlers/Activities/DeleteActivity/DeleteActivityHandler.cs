using TravelApp.BusinessLogic.DTOs.Activities;
using TravelApp.BusinessLogic.Exceptions.SpecificCustomException;
using TravelApp.BusinessLogic.Interfaces;

namespace TravelApp.BusinessLogic.Handlers.Activities.DeleteActivity
{
    /// <summary>Deletes an activity.</summary>
    public class DeleteActivityHandler
    {
        private readonly IUnitOfWorkFactory _unitOfWorkFactory;

        /// <summary>Initializes a new DeleteActivityHandler instance.</summary>
        /// <param name="unitOfWorkFactory">The unit of work factory.</param>
        public DeleteActivityHandler(IUnitOfWorkFactory unitOfWorkFactory)
        {
            _unitOfWorkFactory = unitOfWorkFactory;
        }

        /// <summary>Deletes the activity with the specified identifier.</summary>
        /// <param name="activityId">The activity identifier.</param>
        /// <exception cref="TravelAppNotFoundException">Thrown when the activity does not exist.</exception>
        public async Task DeleteActivityAsync(int activityId)
        {
            await using IUnitOfWork unitOfWork = _unitOfWorkFactory.Create();

            ActivityDto? activity = await unitOfWork.Activities.GetActivityByIdAsync(activityId);

            if (activity is null)
            {
                throw new TravelAppNotFoundException($"Activity {activityId} was not found.");
            }

            await unitOfWork.Activities.DeleteAsync(activityId);
            await unitOfWork.CommitAsync();
        }
    }
}