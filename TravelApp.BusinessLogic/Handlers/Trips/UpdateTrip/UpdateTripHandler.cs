using TravelApp.BusinessLogic.Auth;
using TravelApp.BusinessLogic.Exceptions.SpecificCustomException;
using TravelApp.BusinessLogic.Interfaces;
using TravelApp.BusinessLogic.Models.Trips;

namespace TravelApp.BusinessLogic.Handlers.Trips
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
        /// <exception cref="TravelAppValidationException">Thrown when the start date is after the end date.</exception>
        /// <exception cref="TravelAppNotFoundException">Thrown when the trip does not exist.</exception>
        /// <exception cref="TravelAppForbiddenException">Thrown when the current user cannot update the trip.</exception>
        public async Task UpdateTripAsync(UpdateTripRequest request)
        {
            if (request.StartDate > request.EndDate)
            {
                throw new TravelAppValidationException("Start date cannot be after end date.");
            }

            await using IUnitOfWork unitOfWork = _unitOfWorkFactory.Create();

            Trip? trip = await unitOfWork.Trips.GetByIdAsync(request.TripId);

            if (trip is null)
            {
                throw new TravelAppNotFoundException($"Trip {request.TripId} was not found.");
            }

            bool isParticipant = trip.TripParticipants.Any(p => p.User?.Id == _currentUser.UserId);

            if (!isParticipant && !_currentUser.IsAdmin)
            {
                throw new TravelAppForbiddenException("You cannot update this trip.");
            }

            trip.StartDate = request.StartDate;
            trip.EndDate = request.EndDate;
            trip.IsCompleted = request.IsCompleted;
            trip.PlanNotes = request.PlanNotes;
            trip.ExperienceNotes = request.ExperienceNotes;

            unitOfWork.Trips.Update(trip);
            await unitOfWork.CommitAsync();
        }
    }
}