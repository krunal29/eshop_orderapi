using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using eshop_orderapi.Domain;

namespace eshop_orderapi.UoW
{
    public class UnitOfWorkHelper
    {
        private static IConfiguration _configuration;

        public static void Initialize(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public UnitOfWork GetUnitOfWork()
        {
            var optionBuilder = new DbContextOptionsBuilder<eshop_orderapiContext>();
            optionBuilder.UseSqlServer(_configuration.GetConnectionString("DefaultConnection"));
            var _context = new eshop_orderapiContext(optionBuilder.Options);
            return new UnitOfWork(_context);
        }
    }
}
