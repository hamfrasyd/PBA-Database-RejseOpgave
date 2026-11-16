
using Microsoft.EntityFrameworkCore;
using TravelApp.BusinessLogic.DTOs.Trips;
using TravelApp.BusinessLogic.Interfaces;
using TravelApp.BusinessLogic.Models.Destinations;
using TravelApp.BusinessLogic.Models.Trips;
using TravelApp.BusinessLogic.Models.Users;
using TravelApp.DataAccess.SQLRepositories.EntityFramework;

namespace TravelApp.DataAccess.SQLRepositories
{
    /// <summary>Implements database operations for trips using Entity Framework Core.</summary>
    public class SqlTripRepository : ITripRepository
    {
        private readonly TravelAppContext _context;

        /// <summary>Initializes a new instance of the <see cref="SqlTripRepository"/> class.</summary>
        /// <param name="context">The Entity Framework Core context connected to SQL Server.</param>
        public SqlTripRepository(TravelAppContext context)
        {
            _context = context;
        }

        #region Create - Trips

        /// <inheritdoc />
        public async Task AddForUserAsync(Trip trip, int userId)
        {
            TravelUser? user = await _context.TravelUserEntities
                .FindAsync(userId);

            if (user is null)
            {
                throw new InvalidOperationException("The user must exist before creating a trip.");
            }

            trip.TripParticipants.Add(new TripParticipant
            {
                User = user
            });

            await _context.TripEntities.AddAsync(trip);
        }

        /// <inheritdoc />
        public void AddStop(TripStop stop)
        {
            _context.TripStopEntities.Add(stop);
        }

        /// <inheritdoc />
        public void AddTripActivity(TripActivity activity)
        {
            _context.TripActivityEntities.Add(activity);
        }

        /// <inheritdoc />
        public void AddParticipant(TripParticipant participant)
        {
            _context.TripParticipantEntities.Add(participant);
        }

        /// <inheritdoc />
        public async Task AddCityAsync(int tripId, int cityId)
        {
            Trip trip = await _context.TripEntities.FindAsync(tripId)
                ?? throw new InvalidOperationException("Trip was not found.");

            City city = await _context.CityEntities.FindAsync(cityId)
                ?? throw new InvalidOperationException("City was not found.");

            _context.TripStopEntities.Add(new TripStop
            {
                Trip = trip,
                City = city
            });
        }

        /// <inheritdoc />
        public async Task AddActivityAsync(int tripId, int activityId)
        {
            Trip trip = await _context.TripEntities.FindAsync(tripId)
                ?? throw new InvalidOperationException("Trip was not found.");

            Activity activity = await _context.ActivityEntities.FindAsync(activityId)
                ?? throw new InvalidOperationException("Activity was not found.");

            _context.TripActivityEntities.Add(new TripActivity
            {
                Trip = trip,
                Activity = activity
            });
        }

        /// <inheritdoc />
        public async Task AddParticipantAsync(int tripId, int userId)
        {
            Trip trip = await _context.TripEntities.FindAsync(tripId)
                ?? throw new InvalidOperationException("Trip was not found.");

            TravelUser user = await _context.TravelUserEntities.FindAsync(userId)
                ?? throw new InvalidOperationException("User was not found.");

            _context.TripParticipantEntities.Add(new TripParticipant
            {
                Trip = trip,
                User = user
            });
        }

        #endregion

        #region Read - All

        /// <inheritdoc />
        public async Task<List<Trip>> GetByUserAsync(int userId)
        {
            return await _context.TripEntities
                .AsNoTracking()
                .Where(trip => trip.TripParticipants.Any(p => p.User!.Id == userId))
                .OrderByDescending(trip => trip.StartDate)
                .ToListAsync();
        }

        /// <inheritdoc />
        public async Task<List<TripSummaryDTO>> GetSummariesByUserAsync(int userId, int? countryId, int? continentId)
        {
            IQueryable<Trip> trips = _context.TripEntities
                .AsNoTracking()
                .Where(trip => trip.TripParticipants.Any(participant => participant.User!.Id == userId));

            if (countryId.HasValue)
            {
                trips = trips.Where(trip => trip.TripStops.Any(stop => stop.City!.Country!.Id == countryId.Value));
            }

            if (continentId.HasValue)
            {
                trips = trips.Where(trip => trip.TripStops.Any(stop => stop.City!.Country!.Continent!.Id == continentId.Value));
            }

            List<TripSummaryDTO> result = await trips
                .OrderByDescending(trip => trip.StartDate)
                .Select(trip => new TripSummaryDTO
                {
                    Id = trip.Id,
                    StartDate = trip.StartDate,
                    EndDate = trip.EndDate,
                    IsCompleted = trip.IsCompleted,
                    PlanNotes = trip.PlanNotes,
                    ExperienceNotes = trip.ExperienceNotes,

                    Cities = trip.TripStops
                        .Select(stop => new TripCitySummaryDTO
                        {
                            Id = stop.City!.Id,
                            Name = stop.City.Name
                        })
                        .ToList(),

                    ActivityCount = trip.TripActivities.Count(),
                    ParticipantCount = trip.TripParticipants.Count()
                })
                .AsSplitQuery()
                .ToListAsync();

            return result;
        }

        #endregion

        #region Read - By ID

        /// <inheritdoc />
        public async Task<Trip?> GetByIdAsync(int tripId)
        {
            return await _context.TripEntities
                .Include(trip => trip.TripStops).ThenInclude(stop => stop.City).ThenInclude(city => city!.Country).ThenInclude(country => country!.Continent)
                .Include(trip => trip.TripActivities).ThenInclude(activity => activity.Activity).ThenInclude(activity => activity!.City)
                .Include(trip => trip.TripParticipants).ThenInclude(participant => participant.User)
                .AsSplitQuery()
                .FirstOrDefaultAsync(trip => trip.Id == tripId);
        }

        /// <inheritdoc />
        public async Task<TripDetailsDTO?> GetDetailsAsync(int tripId)
        {
            Trip? trip = await _context.TripEntities
                .AsNoTracking()
                .Include(trip => trip.TripStops).ThenInclude(stop => stop.City).ThenInclude(city => city!.Country).ThenInclude(country => country!.Continent)
                .AsSplitQuery()
                .FirstOrDefaultAsync(trip => trip.Id == tripId);

            if (trip is null)
            {
                return null;
            }

            List<TripDestinationDTO> cities = trip.TripStops
                .Select(stop => new TripDestinationDTO
                {
                    Id = stop.City!.Id,
                    City = stop.City.Name,
                    Country = stop.City.Country!.Name,
                    Continent = stop.City.Country.Continent!.Name
                })
                .ToList();

            TripDetailsDTO result = new TripDetailsDTO
            {
                Id = trip.Id,
                StartDate = trip.StartDate,
                EndDate = trip.EndDate,
                IsCompleted = trip.IsCompleted,
                PlanNotes = trip.PlanNotes,
                ExperienceNotes = trip.ExperienceNotes,
                Cities = cities
            };

            return result;
        }

        /// <inheritdoc />
        public async Task<TripRelationsDTO?> GetRelationsAsync(int tripId)
        {
            TripRelationsDTO? relations = await _context.TripEntities
                .AsNoTracking()
                .Where(trip => trip.Id == tripId)
                .Select(trip => new TripRelationsDTO
                {
                    CityIds = trip.TripStops
                        .Select(stop => stop.City!.Id)
                        .ToList(),

                    Activities = trip.TripActivities
                        .Select(activity => new TripActivityRelationDTO
                        {
                            Id = activity.Activity!.Id,
                            CityId = activity.Activity.City!.Id
                        })
                        .ToList(),

                    ParticipantIds = trip.TripParticipants
                        .Select(participant => participant.User!.Id)
                        .ToList()
                })
                .AsSplitQuery()
                .FirstOrDefaultAsync();

            return relations;
        }

        /// <inheritdoc />
        public async Task<TripStop?> GetStopAsync(int tripId, int cityId)
        {
            return await _context.TripStopEntities
                .FirstOrDefaultAsync(tripStop => tripStop.Trip!.Id == tripId && tripStop.City!.Id == cityId);
        }

        /// <inheritdoc />
        public async Task<TripActivity?> GetTripActivityAsync(int tripId, int activityId)
        {
            return await _context.TripActivityEntities
                .FirstOrDefaultAsync(activity => activity.Trip!.Id == tripId && activity.Activity!.Id == activityId);
        }

        /// <inheritdoc />
        public async Task<TripParticipant?> GetParticipantAsync(int tripId, int userId)
        {
            return await _context.TripParticipantEntities
                .FirstOrDefaultAsync(participant => participant.Trip!.Id == tripId && participant.User!.Id == userId);
        }

        #endregion

        #region Update - Trips

        /// <inheritdoc />
        public void Update(Trip trip)
        {
            _context.TripEntities.Update(trip);
        }

        #endregion

        #region Delete - Trips

        /// <inheritdoc />
        public async Task DeleteAsync(int tripId)
        {
            Trip? trip = await _context.TripEntities
                .FirstOrDefaultAsync(trip => trip.Id == tripId);

            if (trip is null)
            {
                throw new InvalidOperationException($"Trip {tripId} was not found.");
            }

            _context.TripEntities.Remove(trip);
        }

        /// <inheritdoc />
        public void RemoveStop(TripStop stop)
        {
            _context.TripStopEntities.Remove(stop);
        }

        /// <inheritdoc />
        public void RemoveTripActivity(TripActivity activity)
        {
            _context.TripActivityEntities.Remove(activity);
        }

        /// <inheritdoc />
        public void RemoveParticipant(TripParticipant participant)
        {
            _context.TripParticipantEntities.Remove(participant);
        }

        /// <inheritdoc />
        public async Task RemoveCityAsync(int tripId, int cityId)
        {
            TripStop? stop = await _context.TripStopEntities
                .FirstOrDefaultAsync(stop =>
                    stop.Trip!.Id == tripId &&
                    stop.City!.Id == cityId);

            if (stop is null)
            {
                throw new InvalidOperationException("Trip stop was not found.");
            }

            _context.TripStopEntities.Remove(stop);
        }

        /// <inheritdoc />
        public async Task RemoveActivityAsync(int tripId, int activityId)
        {
            TripActivity? activity = await _context.TripActivityEntities
                .FirstOrDefaultAsync(a =>
                    a.Trip!.Id == tripId &&
                    a.Activity!.Id == activityId);

            if (activity is null)
            {
                throw new InvalidOperationException("Trip activity was not found.");
            }

            _context.TripActivityEntities.Remove(activity);
        }

        /// <inheritdoc />
        public async Task RemoveParticipantAsync(int tripId, int userId)
        {
            TripParticipant? participant = await _context.TripParticipantEntities
                .FirstOrDefaultAsync(p =>
                    p.Trip!.Id == tripId &&
                    p.User!.Id == userId);

            if (participant is null)
            {
                throw new InvalidOperationException("Trip participant was not found.");
            }

            _context.TripParticipantEntities.Remove(participant);
        }

        #endregion

        #region Validation - Trip Access & Cities

        /// <inheritdoc />
        public async Task<TripAccessInfoDTO?> GetAccessInfoAsync(int tripId, int userId)
        {
            IQueryable<Trip> trips = _context.TripEntities
                .AsNoTracking()
                .Where(trip => trip.Id == tripId);

            IQueryable<TripAccessInfoDTO> accessInfo = trips
                .Select(trip => new TripAccessInfoDTO
                {
                    IsParticipant = trip.TripParticipants.Any(participant => participant.User!.Id == userId)
                });

            TripAccessInfoDTO? result = await accessInfo.FirstOrDefaultAsync();

            return result;
        }

        /// <inheritdoc />
        public async Task<bool> CityExistsAsync(int cityId)
        {
            return await _context.CityEntities
                .AnyAsync(city => city.Id == cityId);
        }

        #endregion
    }
}
