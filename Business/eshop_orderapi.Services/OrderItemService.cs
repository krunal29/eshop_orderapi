using AutoMapper;
using eshop_orderapi.Domain.Models;
using eshop_orderapi.Interfaces.Services;
using eshop_orderapi.UoW;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace eshop_orderapi.Services
{
    public class OrderItemService : ServiceBase, IOrderItemService
    {
        public OrderItemService(IUnitOfWork unitOfWork, IMapper _mapper) : base(unitOfWork, _mapper)
        {
        }

        public async Task<List<OrderItem>> GetAllAsync()
        {
            var result = mapper.Map<List<OrderItem>>(await unitOfWork.OrderItemRepository.GetAllAsync());
            return result.ToList();
        }

        public async Task<OrderItem> GetAsync(int id)
        {
            return mapper.Map<OrderItem>(await unitOfWork.OrderItemRepository.GetAsync(id));
        }

        public async Task<bool> AddAsync(OrderItem model)
        {
            await unitOfWork.OrderItemRepository.AddAsync(model);
            return await Task.FromResult(true);
        }

        public async Task<bool> UpdateAsync(OrderItem model)
        {
            var data = await unitOfWork.OrderItemRepository.GetAsync(model.Id);
            if (data != null)
            {
                data.Id = model.Id;
                //MAP other fields
                await unitOfWork.OrderItemRepository.UpdateAsync(data);
                return await Task.FromResult(true);
            }
            return await Task.FromResult(false);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var data = unitOfWork.OrderItemRepository.GetAsync(id).Result;
            if (data != null)
            {
                await unitOfWork.OrderItemRepository.DeleteAsync(data);
                return await Task.FromResult(true);
            }
            return await Task.FromResult(false);
        }
    }
}