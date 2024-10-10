using eshop_orderapi.Domain;
using eshop_orderapi.Domain.Models;
using eshop_orderapi.Interfaces.Repositories;

namespace eshop_orderapi.Repositories
{
    public class RoleModuleRepository : BaseRepository<RoleModule>, IRoleModuleRepository
    {
        public RoleModuleRepository(eshop_orderapiContext context) : base(context)
        {
        }
    }
}