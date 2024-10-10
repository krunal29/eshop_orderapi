using AutoMapper;
using eshop_orderapi.Domain.Models;
using eshop_orderapi.Interfaces.Services;
using eshop_orderapi.UoW;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace eshop_orderapi.Services
{
    public class OrderService : ServiceBase, IOrderService
    {
        public OrderService(IUnitOfWork unitOfWork, IMapper _mapper) : base(unitOfWork, _mapper)
        {
        }

        public async Task<List<Order>> GetAllAsync()
        {
            var result = mapper.Map<List<Order>>(await unitOfWork.OrderRepository.GetAllAsync());
            return result.ToList();
        }

        public async Task<Order> GetAsync(int id)
        {
            return mapper.Map<Order>(await unitOfWork.OrderRepository.GetAsync(id));
        }

        public async Task<bool> AddAsync(Order model)
        {
            await unitOfWork.OrderRepository.AddAsync(model);
            return await Task.FromResult(true);
        }

        public async Task<bool> UpdateAsync(Order model)
        {
            var data = await unitOfWork.OrderRepository.GetAsync(model.Id);
            if (data != null)
            {
                data.Id = model.Id;
                //MAP other fields
                await unitOfWork.OrderRepository.UpdateAsync(data);
                return await Task.FromResult(true);
            }
            return await Task.FromResult(false);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var data = unitOfWork.OrderRepository.GetAsync(id).Result;
            if (data != null)
            {
                await unitOfWork.OrderRepository.DeleteAsync(data);
                return await Task.FromResult(true);
            }
            return await Task.FromResult(false);
        }
    }
}