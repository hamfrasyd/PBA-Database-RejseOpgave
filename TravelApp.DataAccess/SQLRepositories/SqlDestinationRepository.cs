using Microsoft.EntityFrameworkCore;
using TravelApp.BusinessLogic.Interfaces;
using TravelApp.BusinessLogic.Models;
using TravelApp.DataAccess.SQLRepositories.EntityFramework;

namespace TravelApp.DataAccess.SQLRepositories
{
    internal class SqlDestinationRepository : IDestinationRepository
    {
        private readonly TravelAppContext _context;

        public SqlDestinationRepository(TravelAppContext context)
        {
            _context = context;
        }

        public async Task<List<Continent>> GetContinentsAsync()
        {
            var result = await _context.ContinentEntities.AsNoTracking().OrderBy(item => item.Name).ToListAsync();
            return result;
        }
    }
}
