using eshop_orderapi.Domain;
using eshop_orderapi.Interfaces.Repositories;
using System.Linq;

namespace eshop_orderapi.Repositories
{
    public class PersonRepository : BaseRepository<Person>, IPersonRepository
    {
        public PersonRepository(eshop_orderapiContext context) : base(context)
        {
        }

        public int GetRoleIdBaseonUserid(string id)
        {
            return Context.Person.FirstOrDefault(x => x.AspNetUserId == id).RoleId;
        }
    }
}