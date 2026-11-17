using TravelApp.BusinessLogic.Auth;
using TravelApp.BusinessLogic.DTOs.Trips;
using TravelApp.BusinessLogic.Exceptions.SpecificCustomException;
using TravelApp.BusinessLogic.Interfaces;
using TravelApp.BusinessLogic.Models.Trips;

namespace TravelApp.BusinessLogic.Handlers.Trips
{
    /// <summary>Handler for updating trip details.</summary>
    public class UpdateTripHandler
    {
        private readonly IUnitOfWorkFactory _unitOfWorkFactory;
        private readonly ICurrentUser _currentUser;

        /// <summary>Initializes a new instance of the <see cref="UpdateTripHandler"/> class.</summary>
        public UpdateTripHandler(IUnitOfWorkFactory unitOfWorkFactory, ICurrentUser currentUser)
        {
            _unitOfWorkFactory = unitOfWorkFactory;
            _currentUser = currentUser;
        }

        /// <summary>Updates the details of a specific trip.</summary>
        public async Task UpdateTripAsync(UpdateTripRequest request)
        {
            if (request.StartDate > request.EndDate)
            {
                throw new TravelAppValidationException("Start date cannot be after end date.");
            }

            await using IUnitOfWork unitOfWork = _unitOfWorkFactory.Create();

            await TripAccessHelper.EnsureCanModifyAsync(unitOfWork, request.TripId, _currentUser);

            UpdateTripDTO update = new UpdateTripDTO
            {
                StartDate = request.StartDate,
                EndDate = request.EndDate,
                IsCompleted = request.IsCompleted,
                PlanNotes = request.PlanNotes,
                ExperienceNotes = request.ExperienceNotes
            };

            await unitOfWork.Trips.UpdateAsync(request.TripId, update);

            await unitOfWork.CommitAsync();
        }
    }
}