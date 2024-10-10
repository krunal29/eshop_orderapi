using eshop_orderapi.Domain;
using eshop_orderapi.Domain.Models;
using eshop_orderapi.Interfaces.Repositories;

namespace eshop_orderapi.Repositories
{
    public class AccessModuleRepository : BaseRepository<AccessModule>, IAccessModuleRepository
    {
        public AccessModuleRepository(eshop_orderapiContext context) : base(context)
        {
        }
    }
}