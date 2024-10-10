using eshop_orderapi.Domain.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace eshop_orderapi.Interfaces.Services
{
    public interface IOrderItemService : IBaseService<OrderItem>
    {
        Task<List<OrderItem>> GetAllAsync();

        Task<OrderItem> GetAsync(int id);

        Task<bool> AddAsync(OrderItem model);

        Task<bool> UpdateAsync(OrderItem model);

        Task<bool> DeleteAsync(int id);
    }
}