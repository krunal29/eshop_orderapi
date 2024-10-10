using eshop_orderapi.Domain;
using eshop_orderapi.Domain.Models;
using eshop_orderapi.Interfaces.Repositories;

namespace eshop_orderapi.Repositories
{
    public class RoleRepository : BaseRepository<Role>, IRoleRepository
    {
        public RoleRepository(eshop_orderapiContext context) : base(context)
        {
        }
    }
}