using TravelApp.BusinessLogic.DTOs;
using TravelApp.BusinessLogic.DTOs.Pagination;
using TravelApp.BusinessLogic.DTOs.Trips;
using TravelApp.BusinessLogic.Exceptions.SpecificCustomException;
using TravelApp.BusinessLogic.Handlers.Trips.TripCity;
using TravelApp.BusinessLogic.Interfaces;

namespace TravelApp.BusinessLogic.Handlers.Trips.GetTrip
{
    /// <summary>Returns trip summaries across all users.</summary>
    public class GetAllTripsHandler
    {
        private readonly IUnitOfWorkFactory _unitOfWorkFactory;

        /// <summary>Initializes a new instance of the <see cref="GetAllTripsHandler"/> class.</summary>
        public GetAllTripsHandler(IUnitOfWorkFactory unitOfWorkFactory)
        {
            _unitOfWorkFactory = unitOfWorkFactory;
        }

        /// <summary>Returns a paginated list of all trips.</summary>
        public async Task<PagedResultDTO<GetTripsResponse>> GetAllTripsAsync(int? countryId = null, int? continentId = null, bool? isCompleted = null, int pageNumber = 1)
        {
            if (pageNumber < 1)
            {
                throw new TravelAppValidationException("Page number must be at least 1.");
            }

            const int pageSize = 10;

            await using IUnitOfWork unitOfWork = _unitOfWorkFactory.Create();

            PagedResultDTO<TripSummaryDTO> trips = await unitOfWork.Trips.GetAllSummariesAsync(countryId, continentId, isCompleted, pageNumber, pageSize);

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
