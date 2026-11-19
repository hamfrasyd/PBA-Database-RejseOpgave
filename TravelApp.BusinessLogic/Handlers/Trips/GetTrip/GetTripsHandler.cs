using TravelApp.BusinessLogic.Auth;
using TravelApp.BusinessLogic.DTOs.Pagination;
using TravelApp.BusinessLogic.DTOs.Trips;
using TravelApp.BusinessLogic.Handlers.Trips.TripCity;
using TravelApp.BusinessLogic.Interfaces;

namespace TravelApp.BusinessLogic.Handlers.Trips.GetTrip
{
    /// <summary>Returns trip summaries for the current user.</summary>
    public class GetTripsHandler
    {
        private readonly IUnitOfWorkFactory _unitOfWorkFactory;
        private readonly ICurrentUser _currentUser;

        /// <summary>Initializes a new instance of the <see cref="GetTripsHandler"/> class.</summary>
        /// <param name="unitOfWorkFactory">The unit of work factory.</param>
        /// <param name="currentUser">The current logged-in user.</param>
        public GetTripsHandler(IUnitOfWorkFactory unitOfWorkFactory, ICurrentUser currentUser)
        {
            _unitOfWorkFactory = unitOfWorkFactory;
            _currentUser = currentUser;
        }

        /// <summary>Returns the current user's trips, optionally filtered by country or continent.</summary>
        /// <param name="countryId">The country identifier to filter by, or <see langword="null"/> for all countries.</param>
        /// <param name="continentId">The continent identifier to filter by, or <see langword="null"/> for all continents.</param>
        /// <param name="pageNumber">The page number for pagination, starting from 1.</param>
        /// <returns>The matching trip summaries.</returns>
        public async Task<PagedResultDTO<GetTripsResponse>> GetTripsAsync(int? countryId = null, int? continentId = null, int pageNumber = 1)
        {
            if (pageNumber < 1)
            {
                throw new ArgumentOutOfRangeException(nameof(pageNumber), "Page number must be greater than or equal to 1.");
            }
            const int pageSize = 10; // Fixed page size for simplicity; can be made configurable if needed.

            await using IUnitOfWork unitOfWork = _unitOfWorkFactory.Create();

            PagedResultDTO<TripSummaryDTO> trips = await unitOfWork.Trips.GetSummariesByUserAsync(_currentUser.UserId, countryId, continentId, pageNumber, pageSize);

            return new PagedResultDTO<GetTripsResponse>
            {
                Items = trips.Items.Select(trip => new GetTripsResponse
                {
                    Id = trip.Id,
                    StartDate = trip.StartDate,
                    EndDate = trip.EndDate,
                    IsCompleted = trip.IsCompleted,
                    PlanNotes = trip.PlanNotes,
                    ExperienceNotes = trip.ExperienceNotes,

                    Cities = trip.Cities.Select(city => new GetTripsCityResponse
                    {
                        Id = city.Id,
                        Name = city.Name
                    }).ToList(),

                    ActivityCount = trip.ActivityCount,
                    ParticipantCount = trip.ParticipantCount,
                    ReviewCount = trip.ReviewCount,
                    AverageStars = trip.AverageStars
                }).ToList(),

                TotalCount = trips.TotalCount,
                PageNumber = trips.PageNumber,
                PageSize = trips.PageSize
            };
        }
    }
}
    