using Microsoft.EntityFrameworkCore;
using TravelApp.BusinessLogic.Interfaces;
using TravelApp.BusinessLogic.Models;
using TravelApp.DataAccess.SQLRepositories.EntityFramework;

namespace TravelApp.DataAccess.SQLRepositories
{
    public class SqlTripRepository : ITripRepository
    {
        private readonly TravelAppContext _context;

        public SqlTripRepository(TravelAppContext context)
        {
            _context = context;
        }

        public async Task<Trip?> GetByIdAsync(int tripId)
        {
            return await _context.TripEntities
                .Include(t => t.TripStops).ThenInclude(s => s.City).ThenInclude(c => c!.Country).ThenInclude(c => c!.Continent)
                .Include(t => t.TripActivities).ThenInclude(a => a.Activity).ThenInclude(a => a!.City)
                .Include(t => t.TripParticipants).ThenInclude(p => p.User)
                .AsSplitQuery()
                .FirstOrDefaultAsync(t => t.Id == tripId);
        }

        public async Task<List<Trip>> GetByUserAsync(int userId)
        {
            return await _context.TripEntities
                .AsNoTracking()
                .Where(t => t.TripParticipants.Any(p => p.User!.Id == userId))
                .OrderByDescending(t => t.StartDate)
                .ToListAsync();
        }

        public async Task AddAsync(Trip trip)
        {
            await _context.TripEntities.AddAsync(trip);
        }

        public void Delete(Trip trip)
        {
            _context.TripEntities.Remove(trip);
        }

        public async Task<bool> CityExistsAsync(int cityId)
        {
            return await _context.CityEntities.AnyAsync(c => c.Id == cityId);
        }

        public async Task<TripStop?> GetStopAsync(int tripId, int cityId)
        {
            return await _context.TripStopEntities
                .FirstOrDefaultAsync(s => s.Trip!.Id == tripId && s.City!.Id == cityId);
        }

        public async Task<TripActivity?> GetTripActivityAsync(int tripId, int activityId)
        {
            return await _context.TripActivityEntities
                .FirstOrDefaultAsync(a =>a.Trip!.Id == tripId && a.Activity!.Id == activityId);
        }

        public async Task<TripParticipant?> GetParticipantAsync(int tripId, int userId)
        {
            return await _context.TripParticipantEntities
                .FirstOrDefaultAsync(p => p.Trip!.Id == tripId && p.User!.Id == userId);
        }

        public void AddStop(TripStop stop)
        {
            _context.TripStopEntities.Add(stop);
        }

        public void AddTripActivity(TripActivity activity)
        {
            _context.TripActivityEntities.Add(activity);
        }

        public void AddParticipant(TripParticipant participant)
        {
            _context.TripParticipantEntities.Add(participant);
        }

        public void RemoveStop(TripStop stop)
        {
            _context.TripStopEntities.Remove(stop);
        }

        public void RemoveTripActivity(TripActivity activity)
        {
            _context.TripActivityEntities.Remove(activity);
        }

        public void RemoveParticipant(TripParticipant participant)
        {
            _context.TripParticipantEntities.Remove(participant);
        }
    }
}