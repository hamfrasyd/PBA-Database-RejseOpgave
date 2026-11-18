using TravelApp.BusinessLogic.DTOs.Groups;
using TravelApp.BusinessLogic.Exceptions.SpecificCustomException;
using TravelApp.BusinessLogic.Interfaces;

namespace TravelApp.BusinessLogic.Handlers.Group
{
    /// <summary>Handles the retrieval of a specific group.</summary>
    public class GetGroupHandler
    {
        private readonly IUnitOfWorkFactory _unitOfWorkFactory;

        /// <summary>Initializes a new GetGroupHandler instance.</summary>
        public GetGroupHandler(IUnitOfWorkFactory unitOfWorkFactory)
        {
            _unitOfWorkFactory = unitOfWorkFactory;
        }

        /// <summary>Retrieves a specific group.</summary>
        public async Task<GetGroupResponse> GetGroupByIdAsync(int groupId)
        {
            await using IUnitOfWork unitOfWork = _unitOfWorkFactory.Create();

            TravelGroupDTO? group = await unitOfWork.Groups.GetGroupByIdAsync(groupId);

            if (group == null)
            {
                throw new TravelAppNotFoundException($"Group {groupId} was not found.");
            }

            return new GetGroupResponse
            {
                Id = group.Id,
                Name = group.Name,
                Description = group.Description,
                Members = group.Members
                    .Select(member => new GetGroupMember
                    {
                        UserId = member.UserId,
                        Name = member.Name
                    })
                    .ToList(),
                Trips = group.Trips
                    .Select(trip => new GetGroupTrip
                    {
                        TripId = trip.TripId,
                        Name = trip.Name
                    })
                    .ToList()
            };
        }
    }
}
