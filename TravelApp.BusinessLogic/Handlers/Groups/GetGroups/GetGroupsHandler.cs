using TravelApp.BusinessLogic.DTOs.Groups;
using TravelApp.BusinessLogic.Interfaces;

namespace TravelApp.BusinessLogic.Handlers.Groups
{
    ///<summary>Handles the retrieval of groups.</summary>
    public class GetGroupsHandler
    {
        private readonly IUnitOfWorkFactory _unitOfWorkFactory;
        /// <summary>Initializes a new GetGroupsHandler instance.</summary>
        public GetGroupsHandler(IUnitOfWorkFactory unitOfWorkFactory)
        {
            _unitOfWorkFactory = unitOfWorkFactory;
        }

        /// <summary>Retrieves all groups.</summary>
        public async Task<List<GetGroupsResponse>> GetGroupsAsync()
        {
            await using IUnitOfWork unitOfWork = _unitOfWorkFactory.Create();
            List<TravelGroupDTO> groups = await unitOfWork.Groups.GetAllGroupsAsync();
            return groups
                .Select(group => new GetGroupsResponse
                {
                    Id = group.Id,
                    Name = group.Name,
                    Description = group.Description,
                    Members = group.Members
                        .Select(member => new GetGroupsMember
                        {
                            UserId = member.UserId,
                            Name = member.Name
                        })
                        .ToList(),
                    Trips = group.Trips
                        .Select(trip => new GetGroupsTrip
                        {
                            TripId = trip.TripId,
                            Name = trip.Name
                        })
                        .ToList()
                })
                .ToList();
        }
    }
}
