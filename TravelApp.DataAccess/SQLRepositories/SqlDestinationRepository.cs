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

        public async Task<List<Country>> GetCountriesAsync(int? continentId)
        {
            IQueryable<Country> countries = _context.CountryEntities.AsNoTracking().Include(country => country.Continent);

            if (continentId.HasValue)
            {
                countries = countries.Where(country => EF.Property<int>(country, "ContinentId") == continentId.Value);
            }

            var result = await countries.OrderBy(item => item.Name).ToListAsync();
            return result;
        }
    }
}
