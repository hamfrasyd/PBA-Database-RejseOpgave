using TravelApp.BusinessLogic.Auth;
using TravelApp.BusinessLogic.DTOs.Trips;
using TravelApp.BusinessLogic.Exceptions.SpecificCustomException;
using TravelApp.BusinessLogic.Handlers.Trips.ManageTripRelations;
using TravelApp.BusinessLogic.Interfaces;

namespace TravelApp.BusinessLogic.Handlers.Trips.UpdateTrip
{
    /// <summary>Updates the details of a trip.</summary>
    public class UpdateTripHandler
    {
        private readonly IUnitOfWorkFactory _unitOfWorkFactory;
        private readonly ICurrentUser _currentUser;

        /// <summary>Initializes a new instance of the <see cref="UpdateTripHandler"/> class.</summary>
        /// <param name="unitOfWorkFactory">The unit of work factory.</param>
        /// <param name="currentUser">The current logged-in user.</param>
        public UpdateTripHandler(IUnitOfWorkFactory unitOfWorkFactory, ICurrentUser currentUser)
        {
            _unitOfWorkFactory = unitOfWorkFactory;
            _currentUser = currentUser;
        }

        /// <summary>Updates a trip from the request.</summary>
        /// <param name="request">The updated trip details.</param>
        /// <exception cref="TravelAppValidationException">Thrown when no field is provided, the start date is after the end date, or the notes are empty.</exception>
        /// <exception cref="TravelAppNotFoundException">Thrown when the trip does not exist.</exception>
        /// <exception cref="TravelAppForbiddenException">Thrown when the current user cannot update the trip.</exception>
        public async Task UpdateTripAsync(int tripId, UpdateTripRequest request)
        {
            if (request.StartDate is null
                && request.EndDate is null
                && request.IsCompleted is null
                && request.PlanNotes is null
                && request.ExperienceNotes is null)
            {
                throw new TravelAppValidationException("At least one field must be provided.");
            }

            if (request.StartDate > request.EndDate)
            {
                throw new TravelAppValidationException("Start date cannot be after end date.");
            }

            if (request.PlanNotes is not null && string.IsNullOrWhiteSpace(request.PlanNotes))
            {
                throw new TravelAppValidationException("Trip plan notes cannot be empty.");
            }

            if (request.ExperienceNotes is not null && string.IsNullOrWhiteSpace(request.ExperienceNotes))
            {
                throw new TravelAppValidationException("Trip experience notes cannot be empty.");
            }

            await using IUnitOfWork unitOfWork = _unitOfWorkFactory.Create();

            await TripAccessHelper.EnsureCanModifyAsync(unitOfWork, tripId, _currentUser);

            UpdateTripDTO update = new UpdateTripDTO
            {
                StartDate = request.StartDate,
                EndDate = request.EndDate,
                IsCompleted = request.IsCompleted,
                PlanNotes = request.PlanNotes?.Trim(),
                ExperienceNotes = request.ExperienceNotes?.Trim()
            };

            await unitOfWork.Trips.UpdateAsync(tripId, update);

            await unitOfWork.CommitAsync();
        }
    }
}