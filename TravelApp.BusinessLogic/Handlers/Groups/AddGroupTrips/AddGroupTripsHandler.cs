using TravelApp.BusinessLogic.Exceptions.SpecificCustomException;
using TravelApp.BusinessLogic.Interfaces;

namespace TravelApp.BusinessLogic.Handlers.Groups.AddGroupTrips
{
    /// <summary>Handles adding trips to an existing travel group.</summary>
    public class AddGroupTripsHandler
    {
        private readonly IUnitOfWorkFactory _unitOfWorkFactory;

        /// <summary>Initializes a new AddGroupTripsHandler instance.</summary>
        public AddGroupTripsHandler(IUnitOfWorkFactory unitOfWorkFactory)
        {
            _unitOfWorkFactory = unitOfWorkFactory;
        }

        /// <summary>Adds the given trips to a group.</summary>
        public async Task AddGroupTripsAsync(int groupId, AddGroupTripsRequest request)
        {
            if (groupId <= 0 || request.ToBeAddedTrips.Count <= 0)
            {
                throw new TravelAppValidationException("Group id or trips are empty.");
            }

            await using IUnitOfWork unitOfWork = _unitOfWorkFactory.Create();

            await unitOfWork.Groups.AddGroupTripsAsync(groupId, request.ToBeAddedTrips);

            await unitOfWork.CommitAsync();
        }
    }
}