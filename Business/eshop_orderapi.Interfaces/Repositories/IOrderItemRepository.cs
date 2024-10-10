using eshop_orderapi.Domain.Models;
using eshop_orderapi.Interfaces.Repository;

namespace eshop_orderapi.Interfaces.Repositories
{
    public interface IOrderItemRepository : IBaseRepository<OrderItem>
    {
    }
}