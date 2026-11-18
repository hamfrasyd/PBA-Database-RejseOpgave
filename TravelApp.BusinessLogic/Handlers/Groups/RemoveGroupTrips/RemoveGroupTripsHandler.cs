using System;
using System.Collections.Generic;
using System.Text;
using TravelApp.BusinessLogic.Exceptions.SpecificCustomException;
using TravelApp.BusinessLogic.Interfaces;

namespace TravelApp.BusinessLogic.Handlers.Groups.RemoveGroupTrips
{
    /// <summary>Handles removing trips from an existing travel group.</summary>
    public class RemoveGroupTripsHandler
    {
        private readonly IUnitOfWorkFactory _unitOfWorkFactory;

        /// <summary>Initializes a new RemoveGroupTripsHandler instance.</summary>
        public RemoveGroupTripsHandler(IUnitOfWorkFactory unitOfWorkFactory)
        {
            _unitOfWorkFactory = unitOfWorkFactory;
        }

        /// <summary>Removes trips from group.</summary>
        public async Task RemoveGroupTripsAsync(int groupId, RemoveGroupTripsRequest request)
        {
            if (groupId <= 0 || request.ToBeRemovedTrips.Count <= 0)
            {
                throw new TravelAppValidationException("Group id or trips are empty.");
            }

            await using IUnitOfWork unitOfWork = _unitOfWorkFactory.Create();

            await unitOfWork.Groups.RemoveGroupTripsAsync(groupId, request.ToBeRemovedTrips);

            await unitOfWork.CommitAsync();
        }
    }
}
