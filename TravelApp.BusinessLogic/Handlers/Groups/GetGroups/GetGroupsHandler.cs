using System;
using System.Collections.Generic;
using System.Text;
using TravelApp.BusinessLogic.Interfaces;

namespace TravelApp.BusinessLogic.Handlers.Groups.GetGroups
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
            List<Group> groups = await unitOfWork.Groups.GetAllGroupsAsync();
            return groups
                .Select(group => new GetGroupsResponse
                {
                    // Map properties from Group to GetGroupsResponse as needed
                })
                .ToList();
        }
    }
}
