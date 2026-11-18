using Microsoft.EntityFrameworkCore;
using TravelApp.BusinessLogic.DTOs.Groups;
using TravelApp.BusinessLogic.Exceptions.SpecificCustomException;
using TravelApp.BusinessLogic.Interfaces;
using TravelApp.BusinessLogic.Models.Groups;
using TravelApp.BusinessLogic.Models.Posts;
using TravelApp.BusinessLogic.Models.Trips;
using TravelApp.BusinessLogic.Models.Users;
using TravelApp.DataAccess.SQLRepositories.EntityFramework;

namespace TravelApp.DataAccess.SQLRepositories
{
    ///<summary>Provides SQL-based data access operations for travel groups.</summary>
    public class SqlGroupRepository : IGroupRepository
    {
        private readonly TravelAppContext _context;

        /// <summary>Initializes a new instance of the SqlGroupRepository class.</summary>
        public SqlGroupRepository(TravelAppContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Retrieves all travel groups together with their members and trips.
        /// </summary>
        /// <returns>
        /// A list of <see cref="TravelGroupDTO"/> objects, one per travel group.
        /// Groups without members or trips are included with empty lists.
        /// </returns>
        public async Task<List<TravelGroupDTO>> GetAllGroupsAsync()
        {
            IQueryable<TravelGroupDTO> groups = _context.TravelGroupEntities
                .AsNoTracking()
                .Select(g => new TravelGroupDTO
                {
                    Id = g.Id,
                    Name = g.Name,
                    Description = g.Description,
                    Members = g.GroupMemberships
                        .Select(gm => new GroupMemberDto
                        {
                            UserId = gm.User.Id,
                            Name = gm.User.FirstName + " " + gm.User.LastName
                        })
                        .ToList(),
                    Trips = g.GroupTrips
                        .Select(gt => new GroupTripDto
                        {
                            TripId = gt.Trip.Id
                            // TODO: Add trip name when it's been added in the database
                            //Name = gt.Trip.Name
                        })
                        .ToList()
                });

            return await groups.ToListAsync();
        }

        ///<summary>Gets group by its id</summary>
        public async Task<TravelGroupDTO?> GetGroupByIdAsync(int id)
        {
            TravelGroupDTO? matchingGroup = await _context.TravelGroupEntities
            .AsNoTracking()
            .Where(g => g.Id == id)
            .Select(g => new TravelGroupDTO
            {
                Id = g.Id,
                Name = g.Name,
                Description = g.Description,
                Members = g.GroupMemberships
                    .Select(gm => new GroupMemberDto
                    {
                        UserId = gm.User.Id,
                        Name = gm.User.FirstName + " " + gm.User.LastName
                    })
                    .ToList(),
                Trips = g.GroupTrips
                    .Select(gt => new GroupTripDto
                    {
                        TripId = gt.Trip.Id
                        // TODO: Add trip name when it's been added in the database
                        //Name = gt.Trip.Name
                    })
                    .ToList()
            })
            .FirstOrDefaultAsync();

            return matchingGroup;
        }

        ///<summary>Creates a new group in the database</summary>
        public async Task<TravelGroup> CreateGroupAsync(TravelGroupDTO group)
        {
            var createdGroup = _context.TravelGroupEntities.Add(new TravelGroup
            {
                Name = group.Name,
                Description = group.Description
            });

            return createdGroup.Entity;
        }

        /// <inheritdoc />
        public async Task AddGroupMembersAsync(int groupId, List<int> userIds)
        {
            TravelGroup group = await _context.TravelGroupEntities.FindAsync(groupId)
                ?? throw new InvalidOperationException("Group was not found.");

            List<int> distinctIds = userIds.Distinct().ToList();

            List<TravelUser> users = await _context.TravelUserEntities
                .Where(u => distinctIds.Contains(u.Id))
                .ToListAsync();

            if (users.Count != distinctIds.Count)
            {
                int missingId = distinctIds.Except(users.Select(u => u.Id)).First();
                throw new InvalidOperationException($"User {missingId} was not found.");
            }

            HashSet<int> existingMemberIds = (await _context.GroupMembershipEntities
                .Where(m => m.Group.Id == groupId && distinctIds.Contains(m.User.Id))
                .Select(m => m.User.Id)
                .ToListAsync())
                .ToHashSet();

            foreach (TravelUser user in users.Where(u => !existingMemberIds.Contains(u.Id)))
            {
                _context.GroupMembershipEntities.Add(new GroupMembership
                {
                    Group = group,
                    User = user
                });
            }
        }

        /// <inheritdoc />
        public async Task AddGroupTripsAsync(int groupId, List<int> tripIds)
        {
            TravelGroup group = await _context.TravelGroupEntities.FindAsync(groupId)
                ?? throw new InvalidOperationException("Group was not found.");

            List<int> distinctIds = tripIds.Distinct().ToList();

            List<Trip> trips = await _context.TripEntities
                .Where(t => distinctIds.Contains(t.Id))
                .ToListAsync();

            if (trips.Count != distinctIds.Count)
            {
                int missingId = distinctIds.Except(trips.Select(t => t.Id)).First();
                throw new InvalidOperationException($"Trip {missingId} was not found.");
            }

            HashSet<int> existingTripIds = (await _context.GroupTripEntities
                .Where(gt => gt.Group.Id == groupId && distinctIds.Contains(gt.Trip.Id))
                .Select(gt => gt.Trip.Id)
                .ToListAsync())
                .ToHashSet();

            foreach (Trip trip in trips.Where(t => !existingTripIds.Contains(t.Id)))
            {
                _context.GroupTripEntities.Add(new GroupTrip
                {
                    Group = group,
                    Trip = trip
                });
            }
        }

        ///<inheritdoc/>
        public async Task DeleteGroupAsync(int groupId)
        {
            TravelGroup? toBeDeletedGroup = await _context.TravelGroupEntities.FirstOrDefaultAsync(group => group.Id == groupId);

            if (toBeDeletedGroup is null)
            {
                throw new TravelAppNotFoundException($"Group {groupId} was not found.");
            }

            _context.TravelGroupEntities.Remove(toBeDeletedGroup);
        }

        /// <inheritdoc />
        public async Task UpdateGroupAsync(UpdateTravelGroupDTO group)
        {
            TravelGroup toBeUpdated = await _context.TravelGroupEntities.FindAsync(group.Id)
                ?? throw new TravelAppNotFoundException($"Group {group.Id} was not found.");

            if (string.IsNullOrEmpty(group.Name) is false)
            {
                toBeUpdated.Name = group.Name.Trim();
            }

            if (string.IsNullOrEmpty(group.Description) is false)
            {
                toBeUpdated.Description = group.Description.Trim();
            }
        }

        /// <inheritdoc />
        public async Task RemoveGroupMembersAsync(int groupId, List<int> userIds)
        {
            bool groupExists = await _context.TravelGroupEntities.AnyAsync(g => g.Id == groupId);

            if (!groupExists)
            {
                throw new TravelAppNotFoundException($"Group {groupId} was not found.");
            }

            List<int> distinctIds = userIds.Distinct().ToList();

            List<GroupMembership> memberships = await _context.GroupMembershipEntities
                .Where(m => m.Group.Id == groupId && distinctIds.Contains(m.User.Id))
                .ToListAsync();

            _context.GroupMembershipEntities.RemoveRange(memberships);
        }

        /// <inheritdoc />
        public async Task RemoveGroupTripsAsync(int groupId, List<int> tripIds)
        {
            bool groupExists = await _context.TravelGroupEntities.AnyAsync(g => g.Id == groupId);

            if (!groupExists)
            {
                throw new TravelAppNotFoundException($"Group {groupId} was not found.");
            }

            List<int> distinctIds = tripIds.Distinct().ToList();

            List<GroupTrip> groupTrips = await _context.GroupTripEntities
                .Where(gt => gt.Group.Id == groupId && distinctIds.Contains(gt.Trip.Id))
                .ToListAsync();

            _context.GroupTripEntities.RemoveRange(groupTrips);
        }
    }
}
