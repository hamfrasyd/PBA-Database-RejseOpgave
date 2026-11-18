using TravelApp.BusinessLogic.DTOs.Groups;
using TravelApp.BusinessLogic.Models.Groups;

namespace TravelApp.BusinessLogic.Interfaces
{
    /// <summary>Repository interface for managing travel groups.</summary>
    public interface IGroupRepository
    {
        ///<summary>Retrieves all travel groups.</summary>
        public Task<List<TravelGroupDTO>> GetAllGroupsAsync();

        ///<summary>Retrieves a specific travel group by its unique identifier.</summary>
        public Task<TravelGroupDTO?> GetGroupByIdAsync(int id);

        ///<summary>Creates a new travel group with no members and trips.</summary>
        public Task<TravelGroup> CreateGroupAsync(TravelGroupDTO group);

        ///<summary>Adds members to an existing group</summary>
        public Task AddGroupMembersAsync(int groupId, List<int> groupMembers);

        ///<summary>Adds trips to an existing group</summary>
        public Task AddGroupTripsAsync(int tripId, List<int> groupTrips);

        ///<summary>Delete a specific group</summary>
        public Task DeleteGroupAsync(int groupId);

        ///<summary>Updates a groups title or description</summary>
        public Task UpdateGroupAsync(UpdateTravelGroupDTO group);

        ///<summary>Removes members in a group</summary>
        public Task RemoveGroupMembersAsync(int groupId, List<int> userIds);

        ///<summary>Removes trips in a group</summary>
        public Task RemoveGroupTripsAsync(int groupId, List<int> tripIds);
    }
}
