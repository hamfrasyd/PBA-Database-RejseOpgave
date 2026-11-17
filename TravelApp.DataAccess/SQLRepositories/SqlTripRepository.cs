using Microsoft.EntityFrameworkCore;
using TravelApp.BusinessLogic.DTOs.Trips;
using TravelApp.BusinessLogic.Interfaces;
using TravelApp.BusinessLogic.Models.Activities;
using TravelApp.BusinessLogic.Models.Locations;
using TravelApp.BusinessLogic.Models.Trips;
using TravelApp.BusinessLogic.Models.Users;
using TravelApp.DataAccess.SQLRepositories.EntityFramework;

namespace TravelApp.DataAccess.SQLRepositories
{
    /// <summary>SQL repository for managing trip data.</summary>
    public class SqlTripRepository : ITripRepository
    {
        private readonly TravelAppContext _context;

        /// <summary>Initializes a new instance of the <see cref="SqlTripRepository"/> class.</summary>
        public SqlTripRepository(TravelAppContext context)
        {
            _context = context;
        }

        #region Create
        /// <summary>Creates a new trip for a user.</summary>
        public async Task CreateTripForUserAsync(Trip trip, int userId)
        {
            TravelUser? user = await _context.TravelUserEntities.FindAsync(userId);
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
        #endregion

        #region Read All
        /// <summary>Retrieves trip summaries for a user with optional filtering by country and continent.</summary>
        public async Task<List<TripSummaryDTO>> GetSummariesByUserAsync(int userId, int? countryId, int? continentId)
        {
            IQueryable<Trip> trips = _context.TripEntities.AsNoTracking().Where(trip => trip.TripParticipants.Any(participant => participant.User!.Id == userId));

            if (countryId.HasValue)
            {
                trips = trips.Where(trip => trip.TripStops.Any(stop => stop.City!.Country!.Id == countryId.Value));
            }

            if (continentId.HasValue)
            {
                trips = trips.Where(trip => trip.TripStops.Any(stop => stop.City!.Country!.Continent!.Id == continentId.Value));
            }

            List<TripSummaryDTO> result = await trips.OrderByDescending(trip => trip.StartDate).Select(trip => new TripSummaryDTO
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

        #region Read By Id
        /// <summary>Retrieves trip relations for a trip.</summary>
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

            List<TripDestinationDTO> cities = trip.TripStops.Select(stop => new TripDestinationDTO
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
        #endregion

        #region Update
        /// <summary>Updates the details of a trip.</summary>
        public async Task UpdateAsync(int tripId, UpdateTripDTO update)
        {
            Trip? trip = await _context.TripEntities.FirstOrDefaultAsync(trip => trip.Id == tripId);
            if (trip is null)
            {
                throw new InvalidOperationException($"Trip {tripId} was not found.");
            }

            trip.StartDate = update.StartDate;
            trip.EndDate = update.EndDate;
            trip.IsCompleted = update.IsCompleted;
            trip.PlanNotes = update.PlanNotes;
            trip.ExperienceNotes = update.ExperienceNotes;
        }
        #endregion

        #region Delete
        /// <summary>Deletes a trip.</summary>
        public async Task DeleteAsync(int tripId)
        {
            Trip? trip = await _context.TripEntities.FirstOrDefaultAsync(trip => trip.Id == tripId);

            if (trip is null)
            {
                throw new InvalidOperationException($"Trip {tripId} was not found.");
            }
            _context.TripEntities.Remove(trip);
        }
        #endregion

        #region Stops in trip
        /// <summary>Adds a city to a trip.</summary>
        public async Task AddCityToTripAsync(int tripId, int cityId)
        {
            Trip trip = await _context.TripEntities.FindAsync(tripId) ?? throw new InvalidOperationException("Trip was not found.");

            City city = await _context.CityEntities.FindAsync(cityId) ?? throw new InvalidOperationException("City was not found.");

            _context.TripStopEntities.Add(new TripStop
            {
                Trip = trip,
                City = city
            });
        }

        /// <summary>Removes a city from a trip.</summary>
        public async Task RemoveCityFromTripAsync(int tripId, int cityId)
        {
            TripStop? stop = await _context.TripStopEntities.FirstOrDefaultAsync(stop =>
                    stop.Trip!.Id == tripId && stop.City!.Id == cityId);

            if (stop is null)
            {
                throw new InvalidOperationException("Trip stop was not found.");
            }
            _context.TripStopEntities.Remove(stop);
        }
        #endregion

        #region Activities in trip
        /// <summary>Retrieves the activities associated with a specific trip.</summary>
        public async Task<List<TripActivityDTO>> GetActivitiesnTripAsync(int tripId)
        {
            List<TripActivityDTO> activities = await _context.TripActivityEntities
                .AsNoTracking().Where(item => item.Trip!.Id == tripId)
                .Select(item => new TripActivityDTO
                {
                    Id = item.Activity!.Id,
                    CityId = item.Activity.City!.Id,
                    Name = item.Activity.Name,
                    Description = item.Activity.Description
                })
                .ToListAsync();

            return activities;
        }

        /// <summary>Adds an activity to a trip.</summary>
        public async Task AddActivityToTripAsync(int tripId, int activityId)
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

        /// <summary>Removes an activity from a trip.</summary>
        public async Task RemoveActivityFromTripAsync(int tripId, int activityId)
        {
            TripActivity? activity = await _context.TripActivityEntities
                .FirstOrDefaultAsync(activity => activity.Trip!.Id == tripId && activity.Activity!.Id == activityId);

            if (activity is null)
            {
                throw new InvalidOperationException("Trip activity was not found.");
            }

            _context.TripActivityEntities.Remove(activity);
        }
        #endregion

        #region Participants in trip
        /// <summary>Retrieves participants for a trip.</summary>
        public async Task<List<TripParticipantDTO>> GetParticipantsInTripAsync(int tripId)
        {
            List<TripParticipantDTO> participants = await _context.TripParticipantEntities
                    .AsNoTracking()
                    .Where(participant => participant.Trip!.Id == tripId)
                    .Select(participant => new TripParticipantDTO
                    {
                        Id = participant.User!.Id,
                        FirstName = participant.User.FirstName,
                        LastName = participant.User.LastName
                    })
                    .ToListAsync();

            return participants;
        }

        /// <summary>Adds participants in a trip.</summary>
        public async Task AddParticipantToTripAsync(int tripId, int userId)
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

        /// <summary>Removes participants from a trip.</summary>
        public async Task RemoveParticipantFromTripAsync(int tripId, int userId)
        {
            TripParticipant? participant = await _context.TripParticipantEntities
                .FirstOrDefaultAsync(participant => participant.Trip!.Id == tripId && participant.User!.Id == userId);

            if (participant is null)
            {
                throw new InvalidOperationException("Trip participant was not found.");
            }
            _context.TripParticipantEntities.Remove(participant);
        }
        #endregion

        #region Access and relations
        /// <summary>Retrieves access information for a trip and user.</summary>
        public async Task<TripAccessInfoDTO?> GetAccessInfoAsync(int tripId, int userId)
        {
            IQueryable<Trip> trips = _context.TripEntities.AsNoTracking().Where(trip => trip.Id == tripId);

            IQueryable<TripAccessInfoDTO> accessInfo = trips.Select(trip => new TripAccessInfoDTO
            {
                IsParticipant = trip.TripParticipants.Any(participant => participant.User!.Id == userId)
            });

            TripAccessInfoDTO? result = await accessInfo.FirstOrDefaultAsync();
            return result;
        }

        /// <summary>Retrieves the relations of a trip, including cities, activities, and participants.</summary>
        public async Task<TripRelationsDTO?> GetRelationsAsync(int tripId)
        {
            TripRelationsDTO? relations = await _context.TripEntities.AsNoTracking().Where(trip => trip.Id == tripId)
                .Select(trip => new TripRelationsDTO
                {
                    CityIds = trip.TripStops.Select(stop => stop.City!.Id).ToList(),
                    Activities = trip.TripActivities.Select(activity => new TripActivityRelationDTO
                    {
                        Id = activity.Activity!.Id,
                        CityId = activity.Activity.City!.Id
                    }).ToList(),
                    ParticipantIds = trip.TripParticipants.Select(participant => participant.User!.Id).ToList()
                })
                .AsSplitQuery()
                .FirstOrDefaultAsync();

            return relations;
        }
        #endregion
    }
}