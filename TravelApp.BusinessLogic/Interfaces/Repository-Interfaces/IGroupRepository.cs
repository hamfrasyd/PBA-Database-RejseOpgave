using TravelApp.BusinessLogic.Models.Groups;

namespace TravelApp.BusinessLogic.Interfaces
{
    public interface IGroupRepository
    {
        public Task<List<TravelGroup>> GetAllGroupsAsync();
    }
}
