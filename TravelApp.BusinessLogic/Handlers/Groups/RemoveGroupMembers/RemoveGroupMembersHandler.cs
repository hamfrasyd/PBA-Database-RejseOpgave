using System;
using System.Collections.Generic;
using System.Text;
using TravelApp.BusinessLogic.Exceptions.SpecificCustomException;
using TravelApp.BusinessLogic.Handlers.Groups.UpdateGroupMembers;
using TravelApp.BusinessLogic.Interfaces;

namespace TravelApp.BusinessLogic.Handlers.Groups.UpdateGroupMember
{
    /// <summary>Handles removing members from an existing travel group.</summary>
    public class RemoveGroupMembersHandler
    {
        private readonly IUnitOfWorkFactory _unitOfWorkFactory;

        /// <summary>Initializes a new UpdateGroupMembersHandler instance.</summary>
        /// <param name="unitOfWorkFactory">The factory used to create units of work.</param>
        public RemoveGroupMembersHandler(IUnitOfWorkFactory unitOfWorkFactory)
        {
            _unitOfWorkFactory = unitOfWorkFactory;
        }

        /// <summary>Removes members from group.</summary>
        public async Task RemoveGroupMembersAsync(int groupId, RemoveGroupMembersRequest request)
        {
            if (groupId <= 0)
            {
                throw new TravelAppValidationException("Group id is invalid");
            }

            await using IUnitOfWork unitOfWork = _unitOfWorkFactory.Create();

            await unitOfWork.Groups.RemoveGroupMembersAsync(groupId, request.ToBeRemovedMembers);

            await unitOfWork.CommitAsync();
        }
    }
}
