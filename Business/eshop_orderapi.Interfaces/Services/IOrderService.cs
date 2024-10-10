using eshop_orderapi.Domain.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace eshop_orderapi.Interfaces.Services
{
    public interface IOrderService : IBaseService<Order>
    {
        Task<List<Order>> GetAllAsync();

        Task<Order> GetAsync(int id);

        Task<bool> AddAsync(Order model);

        Task<bool> UpdateAsync(Order model);

        Task<bool> DeleteAsync(int id);
    }
}