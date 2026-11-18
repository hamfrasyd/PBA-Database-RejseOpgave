using System;
using System.Collections.Generic;
using System.Text;
using TravelApp.BusinessLogic.DTOs.Groups;
using TravelApp.BusinessLogic.Exceptions.SpecificCustomException;
using TravelApp.BusinessLogic.Handlers.Groups.CreateGroup;
using TravelApp.BusinessLogic.Interfaces;
using TravelApp.BusinessLogic.Models.Groups;

namespace TravelApp.BusinessLogic.Handlers.Groups.AddGroupMembers
{

    /// <summary>Handles adding members to an existing travel group.</summary>
    public class AddGroupMembersHandler
    {
        private readonly IUnitOfWorkFactory _unitOfWorkFactory;

        /// <summary>Initializes a new AddGroupMembersHandler instance.</summary>
        public AddGroupMembersHandler(IUnitOfWorkFactory unitOfWorkFactory)
        {
            _unitOfWorkFactory = unitOfWorkFactory;
        }

        /// <summary>Adds the given users as members of a group.</summary>
        public async Task AddGroupMembersAsync(int groupId, AddGroupMembersRequest request)
        {
            if (groupId <= 0 || request.ToBeAddedMembers.Count <= 0)
            {
                throw new TravelAppValidationException("Group id or members are empty");
            }

            await using IUnitOfWork unitOfWork = _unitOfWorkFactory.Create();

            await unitOfWork.Groups.AddGroupMembersAsync(groupId, request.ToBeAddedMembers);

            await unitOfWork.CommitAsync();
        }
    }
}