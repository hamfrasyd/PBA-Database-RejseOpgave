using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using TravelApp.BusinessLogic.Interfaces;
using TravelApp.BusinessLogic.Models.Groups;
using TravelApp.DataAccess.SQLRepositories.EntityFramework;

namespace TravelApp.DataAccess.SQLRepositories
{
    public class SqlGroupRepository : IGroupRepository
    {
        private readonly TravelAppContext _context;

        /// <summary>Initializes a new instance of the SqlGroupRepository class.</summary>
        public SqlGroupRepository(TravelAppContext context)
        {
            _context = context;
        }

        /// <summary>Retrieves all groups from the database.</summary>
        public async Task<List<TravelGroup>> GetAllGroupsAsync()
        {
            IQueryable<TravelGroup> groups = _context.TravelGroupEntities
                .Include(g => g.GroupMemberships).ThenInclude(gm => gm.User)
                .Include(g => g.GroupTrips).ThenInclude(gt => gt.Trip)
                .AsSplitQuery()
                .AsNoTracking();

            return await groups.ToListAsync();
        }
    }
}
