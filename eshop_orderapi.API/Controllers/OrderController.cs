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
    public class OrderController : BaseApiController
    {
        private readonly IHtmlLocalizer<OrderController> _localizer;
        private readonly IOrderService _OrderService;

        public OrderController(IOrderService OrderService, IHtmlLocalizer<OrderController> localizer)
        {
            _OrderService = OrderService;
            _localizer = localizer;
        }

        [HttpGet]
        [Route("GetAll")]
        public async Task<object> GetAll()
        {
            return await GetDataWithMessage(async () =>
            {
                var result = (await _OrderService.GetAllAsync());
                return Response(result, string.Empty);
            });
        }

        [HttpGet]
        public async Task<object> Get(int id)
        {
            return await GetDataWithMessage(async () =>
            {
                var result = await _OrderService.GetAsync(id);
                return Response(result, string.Empty);
            });
        }

        [HttpPost]
        public async Task<object> Post([FromBody] Order model)
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

        private async Task<Tuple<Order, string, DropMessageType>> AddAsync(Order model)
        {
            var flag = await _OrderService.AddAsync(model);
            if (flag)
            {
                return Response(model, _localizer["RecordAddSuccess"].Value.ToString());
            }
            return Response(model, _localizer["RecordNotAdded"].Value.ToString(), DropMessageType.Error);
        }

        private async Task<Tuple<Order, string, DropMessageType>> UpdateAsync(Order model)
        {
            var flag = await _OrderService.UpdateAsync(model);
            if (flag)
                return Response(model, _localizer["RecordUpdeteSuccess"].Value.ToString());
            return Response(model, _localizer["RecordNotUpdate"].Value.ToString(), DropMessageType.Error);
        }

        [HttpDelete]
        public async Task<object> Delete(int id)
        {
            return await GetDataWithMessage(async () =>
            {
                var flag = await _OrderService.DeleteAsync(id);
                if (flag)
                    return Response(new BooleanResponseModel { Value = flag }, _localizer["RecordDeleteSuccess"].Value.ToString());
                return Response(new BooleanResponseModel { Value = flag }, _localizer["ReordNotDeleteSucess"].Value.ToString(), DropMessageType.Error);
            });

            return await GetDataWithMessage(async () =>
            {
                var flag = await _OrderService.DeleteAsync(id);
                if (flag)
                    return Response(new BooleanResponseModel { Value = flag }, _localizer["RecordDeleteSuccess"].Value.ToString());
                return Response(new BooleanResponseModel { Value = flag }, _localizer["ReordNotDeleteSucess"].Value.ToString(), DropMessageType.Error);
            });
            return await GetDataWithMessage(async () =>
            {

                var flag = await _OrderService.DeleteAsync(id);
                if (flag)
                    return Response(new BooleanResponseModel { Value = flag }, _localizer["RecordDeleteSuccess"].Value.ToString());
                return Response(new BooleanResponseModel { Value = flag }, _localizer["ReordNotDeleteSucess"].Value.ToString(), DropMessageType.Error);
            });
            return await GetDataWithMessage(async () =>
            {
                var flag = await _OrderService.DeleteAsync(id);
                if (flag)
                    return Response(new BooleanResponseModel { Value = flag }, _localizer["RecordDeleteSuccess"].Value.ToString());
                return Response(new BooleanResponseModel { Value = flag }, _localizer["ReordNotDeleteSucess"].Value.ToString(), DropMessageType.Error);
            });
        }


        [HttpDelete]
        public async Task TestApi()
        {


            string Abc= "";


        }
        [HttpDelete]
        public async Task TestApai()
        {


            string Abc = "";


        }

        [HttpDelete]
        public async Task TestApai21()
        {


            string Abc = "";


        }
    }
}