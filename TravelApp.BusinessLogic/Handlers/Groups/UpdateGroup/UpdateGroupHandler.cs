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
        /// <exception cref="TravelAppValidationException">Thrown when no field is provided or the name or description is empty.</exception>
        public async Task UpdateGroupAsync(int groupId, UpdateGroupRequest request)
        {
            if (request.Name is null && request.Description is null)
            {
                throw new TravelAppValidationException("At least one field must be provided.");
            }

            if (request.Name is not null && string.IsNullOrWhiteSpace(request.Name))
            {
                throw new TravelAppValidationException("Group name cannot be empty.");
            }

            if (request.Description is not null && string.IsNullOrWhiteSpace(request.Description))
            {
                throw new TravelAppValidationException("Group description cannot be empty.");
            }

            await using IUnitOfWork unitOfWork = _unitOfWorkFactory.Create();

            await unitOfWork.Groups.UpdateGroupAsync(new UpdateTravelGroupDTO
            {
                Id = groupId,
                Name = request.Name?.Trim(),
                Description = request.Description?.Trim()
            });

            await unitOfWork.CommitAsync();
        }
    }
}
