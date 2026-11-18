using System;
using System.Collections.Generic;
using System.Text;
using TravelApp.BusinessLogic.DTOs.Groups;
using TravelApp.BusinessLogic.Exceptions.SpecificCustomException;
using TravelApp.BusinessLogic.Interfaces;
using TravelApp.BusinessLogic.Models.Groups;

namespace TravelApp.BusinessLogic.Handlers.Groups.UpdateGroup
{
    /// <summary>Handles the update group operation.</summary>
    public class UpdateGroupHandler
    {
        private readonly IUnitOfWorkFactory _unitOfWorkFactory;

        /// <summary>Initializes a new UpdatePostHandler instance.</summary>
        public UpdateGroupHandler(IUnitOfWorkFactory unitOfWorkFactory)
        {
            _unitOfWorkFactory = unitOfWorkFactory;
        }

        /// <summary>Updates a group</summary>
        public async Task UpdateGroupAsync(int groupId, UpdateGroupRequest request)
        {
            await using IUnitOfWork unitOfWork = _unitOfWorkFactory.Create();

            await unitOfWork.Groups.UpdateGroupAsync(new UpdateTravelGroupDTO
            {
                Id = groupId,
                Name = string.IsNullOrWhiteSpace(request.Name) ? "" : request.Name.Trim(),
                Description = string.IsNullOrWhiteSpace(request.Description) ? "" : request.Description.Trim()
            });

            await unitOfWork.CommitAsync();
        }
    }
}
