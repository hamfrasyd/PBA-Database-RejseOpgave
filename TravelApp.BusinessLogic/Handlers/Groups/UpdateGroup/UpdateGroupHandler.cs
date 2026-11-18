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
        public async Task UpdateGroupAsync(UpdateGroupRequest request)
        {
            if (request.Id <= 0 || string.IsNullOrWhiteSpace(request.Name))
            {
                throw new TravelAppValidationException("Invalid group data.");
            }

            await using IUnitOfWork unitOfWork = _unitOfWorkFactory.Create();

            await unitOfWork.Groups.UpdateGroupAsync(new TravelGroupDTO
            {
                Id = request.Id,
                Name = request.Name.Trim(),
                Description = string.IsNullOrWhiteSpace(request.Description) ? null : request.Description.Trim()
            });

            await unitOfWork.CommitAsync();
        }
    }
}
