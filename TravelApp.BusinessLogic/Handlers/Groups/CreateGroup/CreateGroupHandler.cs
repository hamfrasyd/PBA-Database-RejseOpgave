using System;
using System.Collections.Generic;
using System.Text;
using TravelApp.BusinessLogic.DTOs.Groups;
using TravelApp.BusinessLogic.Exceptions.SpecificCustomException;
using TravelApp.BusinessLogic.Interfaces;
using TravelApp.BusinessLogic.Models.Groups;

namespace TravelApp.BusinessLogic.Handlers.Groups.CreateGroup
{
    /// <summary>Handles the creation of a new group without members or trips.</summary>
    public class CreateGroupHandler
    {
        private readonly IUnitOfWorkFactory _unitOfWorkFactory;

        /// <summary>Initializes a new CreateGroupHandler instance.</summary>
        public CreateGroupHandler(IUnitOfWorkFactory unitOfWorkFactory)
        {
            _unitOfWorkFactory = unitOfWorkFactory;
        }

        /// <summary>Creates group.</summary>
        /// <exception cref="TravelAppValidationException">Thrown when the description is empty.</exception>
        public async Task<CreateGroupResponse> CreateGroupAsync(CreateGroupRequest request)
        {
            if (request.Description is not null && string.IsNullOrWhiteSpace(request.Description))
            {
                throw new TravelAppValidationException("Group description cannot be empty.");
            }

            await using IUnitOfWork unitOfWork = _unitOfWorkFactory.Create();

            TravelGroup group = await unitOfWork.Groups.CreateGroupAsync(new TravelGroupDTO
            {
                Name = request.Name.Trim(),
                Description = request.Description?.Trim()
            });

            await unitOfWork.CommitAsync();

            return new CreateGroupResponse { Id = group.Id, Name = group.Name, Description = group.Description };
        }
    }
}
