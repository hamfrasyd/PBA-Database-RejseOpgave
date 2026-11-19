using TravelApp.BusinessLogic.Auth;
using TravelApp.BusinessLogic.Exceptions.SpecificCustomException;
using TravelApp.BusinessLogic.Interfaces;
using TravelApp.BusinessLogic.Models.Trips;
using TravelApp.BusinessLogic.Models.Users;

namespace TravelApp.BusinessLogic.Handlers.Trips.CreateTrip
{
    /// <summary>Creates a trip for the current user.</summary>
    public class CreateTripHandler
    {
        private readonly IUnitOfWorkFactory _unitOfWorkFactory;
        private readonly ICurrentUser _currentUser;

        /// <summary>Initializes a new instance of the <see cref="CreateTripHandler"/> class.</summary>
        /// <param name="unitOfWorkFactory">The unit of work factory.</param>
        /// <param name="currentUser">The current logged-in user.</param>
        public CreateTripHandler(IUnitOfWorkFactory unitOfWorkFactory, ICurrentUser currentUser)
        {
            _unitOfWorkFactory = unitOfWorkFactory;
            _currentUser = currentUser;
        }

        /// <summary>Creates a trip from the request.</summary>
        /// <param name="request">The trip details to create.</param>
        /// <returns>The created trip.</returns>
        /// <exception cref="TravelAppValidationException">Thrown when the start date is after the end date.</exception>
        /// <exception cref="TravelAppNotFoundException">Thrown when the current user does not exist.</exception>
        public async Task<CreateTripResponse> CreateTripAsync(CreateTripRequest request)
        {
            if (request.StartDate > request.EndDate)
            {
                throw new TravelAppValidationException("Start date cannot be after end date.");
            }

            await using IUnitOfWork unitOfWork = _unitOfWorkFactory.Create();

            TravelUser? user = await unitOfWork.Users.GetByIdAsync(_currentUser.UserId);
            if (user is null)
            {
                throw new TravelAppNotFoundException("The current user was not found.");
            }

            Trip trip = new Trip
            {
                StartDate = request.StartDate,
                EndDate = request.EndDate,
                PlanNotes = request.PlanNotes,
                IsCompleted = false
            };

            await unitOfWork.Trips.CreateTripForUserAsync(trip, user.Id);

            await unitOfWork.CommitAsync();

            return new CreateTripResponse
            {
                Id = trip.Id,
                StartDate = trip.StartDate,
                EndDate = trip.EndDate,
                PlanNotes = trip.PlanNotes,
                IsCompleted = trip.IsCompleted
            };
        }
    }
}