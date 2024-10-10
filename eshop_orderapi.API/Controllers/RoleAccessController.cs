using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Localization;
using eshop_orderapi.Business.Enums.General;
using eshop_orderapi.Business.ViewModels.Organization;
using eshop_orderapi.Interfaces.Services;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace eshop_orderapi.API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    public class RoleAccessController : BaseApiController
    {
        private readonly IHtmlLocalizer<PersonController> _localizer;
        private readonly IRoleModuleService _roleModuleService;

        public RoleAccessController(IHtmlLocalizer<PersonController> localizer, IRoleModuleService roleModuleService)
        {
            _localizer = localizer;
            _roleModuleService = roleModuleService;
        }

        #region POST/PUT

        [HttpPost]
        [AllowAnonymous]
        public async Task<object> Post([FromBody] RoleModuleDetailsModel roleModuleModel)
        {
            return await GetDataWithMessage(async () =>
            {
                if (ModelState.IsValid && roleModuleModel != null)
                {
                    return roleModuleModel.Id > 0 ? await UpdateAsync(roleModuleModel) : await AddAsync(roleModuleModel);
                }
                var errors = ModelState.Values.SelectMany(v => v.Errors).Select(v => v.ErrorMessage);
                return Response(roleModuleModel, string.Join(",", errors), DropMessageType.Error);
            });
        }

        private async Task<Tuple<RoleModuleDetailsModel, string, DropMessageType>> AddAsync(RoleModuleDetailsModel roleModuleModel)
        {
            var flag = await _roleModuleService.AddAsync(roleModuleModel);
            if (flag)
            {
                return Response(roleModuleModel, _localizer["RecordAddSuccess"].Value.ToString());
            }
            return Response(roleModuleModel, _localizer["RecordNotAdded"].Value.ToString(), DropMessageType.Error);
        }

        private async Task<Tuple<RoleModuleDetailsModel, string, DropMessageType>> UpdateAsync(RoleModuleDetailsModel roleModuleModel)
        {
            var flag = await _roleModuleService.UpdateAsync(roleModuleModel);
            if (flag)
                return Response(roleModuleModel, _localizer["RecordUpdeteSuccess"].Value.ToString());
            return Response(roleModuleModel, _localizer["RecordNotUpdate"].Value.ToString(), DropMessageType.Error);
        }

        #endregion POST/PUT
    }
}