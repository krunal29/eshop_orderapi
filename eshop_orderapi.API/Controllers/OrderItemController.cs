using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Localization;
using eshop_orderapi.Business.Enums.General;
using eshop_orderapi.Business.ViewModels.General;
using eshop_orderapi.Domain.Models;
using eshop_orderapi.Interfaces.Services;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace eshop_orderapi.API.Controllers
{
    //[Authorize]
    [Route("api/[controller]")]
    public class OrderItemController : BaseApiController
    {
        private readonly IHtmlLocalizer<OrderItemController> _localizer;
        private readonly IOrderItemService _OrderItemService;

        public OrderItemController(IOrderItemService OrderItemService, IHtmlLocalizer<OrderItemController> localizer)
        {
            _OrderItemService = OrderItemService;
            _localizer = localizer;
        }

        [HttpGet]
        [Route("GetAll")]
        public async Task<object> GetAll()
        {
            return await GetDataWithMessage(async () =>
            {
                var result = (await _OrderItemService.GetAllAsync());
                return Response(result, string.Empty);
            });
        }

        [HttpGet]
        public async Task<object> Get(int id)
        {
            return await GetDataWithMessage(async () =>
            {
                var result = await _OrderItemService.GetAsync(id);
                return Response(result, string.Empty);
            });
        }

        [HttpPost]
        public async Task<object> Post([FromBody] OrderItem model)
        {
            return await GetDataWithMessage(async () =>
            {
                if (ModelState.IsValid && model != null)
                {
                    return model.Id <= 0 ? await AddAsync(model) : await UpdateAsync(model);
                }
                var errors = ModelState.Values.SelectMany(v => v.Errors).Select(v => v.ErrorMessage);
                return Response(model, string.Join(",", errors), DropMessageType.Error);
            });
        }

        private async Task<Tuple<OrderItem, string, DropMessageType>> AddAsync(OrderItem model)
        {
            var flag = await _OrderItemService.AddAsync(model);
            if (flag)
            {
                return Response(model, _localizer["RecordAddSuccess"].Value.ToString());
            }
            return Response(model, _localizer["RecordNotAdded"].Value.ToString(), DropMessageType.Error);
        }

        private async Task<Tuple<OrderItem, string, DropMessageType>> UpdateAsync(OrderItem model)
        {
            var flag = await _OrderItemService.UpdateAsync(model);
            if (flag)
                return Response(model, _localizer["RecordUpdeteSuccess"].Value.ToString());
            return Response(model, _localizer["RecordNotUpdate"].Value.ToString(), DropMessageType.Error);
        }

        [HttpDelete]
        public async Task<object> Delete(int id)
        {
            return await GetDataWithMessage(async () =>
            {
                var flag = await _OrderItemService.DeleteAsync(id);
                if (flag)
                    return Response(new BooleanResponseModel { Value = flag }, _localizer["RecordDeleteSuccess"].Value.ToString());
                return Response(new BooleanResponseModel { Value = flag }, _localizer["ReordNotDeleteSucess"].Value.ToString(), DropMessageType.Error);
            });
        }
    }
}