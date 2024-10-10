using eshop_orderapi.Domain;
using eshop_orderapi.Interfaces.Repository;

namespace eshop_orderapi.Interfaces.Repositories
{
    public interface IPersonRepository : IBaseRepository<Person>
    {
        int GetRoleIdBaseonUserid(string id);
    }
}