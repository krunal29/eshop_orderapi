using AAT.API.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Localization;
using eshop_orderapi.API.Filters;
using eshop_orderapi.Business.Enums.General;
using eshop_orderapi.Business.Helpers;
using eshop_orderapi.Business.ViewModels;
using eshop_orderapi.Business.ViewModels.General;
using eshop_orderapi.Business.ViewModels.Organization;
using eshop_orderapi.Domain;
using eshop_orderapi.Domain.Models;
using eshop_orderapi.Interfaces.Background;
using eshop_orderapi.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Transactions;

namespace eshop_orderapi.API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    public class PersonController : BaseApiController
    {
        private readonly IHtmlLocalizer<PersonController> _localizer;
        private readonly IPersonService _personService;
        private readonly IFileHelper _fileHelper;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IBackgroundService _backgroundService;
        private readonly IRoleModuleService _roleModuleService;

        public PersonController(IPersonService personService, IFileHelper fileHelper, IBackgroundService backgroundService, IHtmlLocalizer<PersonController> localizer, SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager, IRoleModuleService roleModuleService)
        {
            _userManager = userManager;
            _fileHelper = fileHelper;
            _signInManager = signInManager;
            _personService = personService;
            _backgroundService = backgroundService;
            _localizer = localizer;
            _roleModuleService = roleModuleService;
        }

        [HttpGet]
        [AllowAnonymous]
        [Route("GetLocalizationDemoString")]
        public async Task<object> GetLocalizationDemoString()
        {
            return await GetDataWithMessage(async () =>
            {
                var text = _localizer["ControllerString"].Value.ToString();
                return Response(text, string.Empty);
            });
        }

        [HttpGet]
        [Route("GetAll")]
        public async Task<object> GetAll()
        {
            return await GetDataWithMessage(async () =>
            {
                var roleModule = MemoryCacheHelper.memoryCache.Get<List<RoleModuleModel>>(Constant.Memory_UserRoles);
                var result = (await _personService.GetAllAsync());
                return Response(result, string.Empty);
            });
        }

        [HttpGet]
        [Route("TestRoleAccess")]
        [RoleAccessAuthorizationFilter(ModuleEnum.Dashboard, AccessTypeEnum.View)]
        public async Task<object> TestRoleAccess()
        {
            return await GetDataWithMessage(async () =>
            {
                var result = (await _personService.GetAllAsync());
                return Response(result, string.Empty);
            });
        }

        [HttpPost]
        [Route("Login")]
        [AllowAnonymous]
        public async Task<object> Login([FromBody] PersonModel personModel)
        {
            return await GetDataWithMessage(async () =>
            {
                if (personModel != null)
                {
                    var result = await _signInManager.PasswordSignInAsync(personModel.Name, personModel.Password, false, false);
                    if (result.Succeeded)
                    {
                        var user = new LoginResponseModel();
                        user.ApplicationUser = await _userManager.FindByNameAsync(personModel.Name);

                        int roleId = _personService.GetRoleIdBaseonUserid(user.ApplicationUser.Id);
                        var token = ApiTokenHelper.GenerateJSONWebToken(user.ApplicationUser, roleId == 0 ? 1 : roleId);
                        user.Token = token;

                        ///Set user role values in memoryCacheHelper like this..
                        var userRoleModule = await _roleModuleService.GetAllAsync();
                        user.roleModuleModel = userRoleModule;
                        MemoryCacheHelper.memoryCache.Set(Constant.Memory_UserRoles, userRoleModule);

                        return Response(user, string.Empty);
                    }
                }
                return Response(new LoginResponseModel(), _localizer["UserNotFound"].Value.ToString(), DropMessageType.Error);
            });
        }

        [HttpGet]
        public async Task<object> Get(int id)
        {
            return await GetDataWithMessage(async () =>
            {
                var result = await _personService.GetAsync(id);
                return Response(result, string.Empty);
            });
        }

        [HttpPost]
        public async Task<object> Post([FromBody] Person model)
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

        private async Task<Tuple<Person, string, DropMessageType>> AddAsync(Person model)
        {
            var flag = await _personService.AddAsync(model);
            if (flag)
            {
                //_backgroundService.EnqueueJob<IBackgroundMailerJobs>(m => m.SendWelcomeEmail());
                return Response(model, _localizer["RecordAddSuccess"].Value.ToString());
            }
            return Response(model, _localizer["RecordNotAdded"].Value.ToString(), DropMessageType.Error);
        }

        private async Task<Tuple<Person, string, DropMessageType>> UpdateAsync(Person model)
        {
            var flag = await _personService.UpdateAsync(model);
            if (flag)
                return Response(model, _localizer["RecordUpdeteSuccess"].Value.ToString());
            return Response(model, _localizer["RecordNotUpdate"].Value.ToString(), DropMessageType.Error);
        }

        [HttpDelete]
        public async Task<object> Delete(int id)
        {
            return await GetDataWithMessage(async () =>
            {
                var flag = await _personService.DeleteAsync(id);
                if (flag)
                    return Response(new BooleanResponseModel { Value = flag }, _localizer["RecordDeleteSuccess"].Value.ToString());
                return Response(new BooleanResponseModel { Value = flag }, _localizer["ReordNotDeleteSucess"].Value.ToString(), DropMessageType.Error);
            });
        }

        #region Reset password

        [HttpPost]
        [Route("ResetPassword")]
        public async Task<object> ResetPassword([FromBody] ChangePassword model)
        {
            var flag = new object();
            return await GetDataWithMessage(async () =>
            {
                using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
                {
                    if (model != null)
                    {
                        flag = await _personService.ResetPassword(model);
                        scope.Complete();
                        if (flag != null)
                        {
                            return Response(true, _localizer["Password change successfully"].Value.ToString(), DropMessageType.Success);
                        }
                    }
                }
                return Response(false, _localizer["UserNotFound"].Value.ToString(), DropMessageType.Error);
            });
        }

        #endregion Reset password

        #region Forgot Password

        [HttpGet]
        [Route("GetForgotPasswordLink")]
        [AllowAnonymous]
        public async Task<object> GetForgotPasswordLink(string emailId)
        {
            var applicationUser = await _userManager.FindByEmailAsync(emailId);
            if (applicationUser == null)
            {
                return Response(applicationUser, "Email Address not found", DropMessageType.Error);
            }
            var resetId = Guid.NewGuid();
            await _personService.ResetForgotPassword(applicationUser.Id, resetId);
            _backgroundService.EnqueueJob<IBackgroundMailerJobs>(m => m.ForgotPassword(applicationUser.Email, resetId.ToString()));

            return Response(applicationUser, "Email sent...", DropMessageType.Success);
        }

        [HttpGet]
        [Route("CheckForgotPasswordLinkExist")]
        [AllowAnonymous]
        public async Task<object> CheckForgotPasswordLinkExist(Guid resetId)
        {
            return await GetDataWithMessage(async () =>
            {
                var result = _personService.IsResetIdExist(resetId);

                if (!result)
                {
                    return Response(false, "This Link Does Not Exist", DropMessageType.Error);
                }
                return Response(false, "", DropMessageType.Error);
            });
        }

        [HttpPost]
        [Route("ForgotPassword")]
        [AllowAnonymous]
        public async Task<object> ForgotPassword([FromBody] ResetPasswordViewModel model)
        {
            var flag = new object();
            return await GetDataWithMessage(async () =>
            {
                using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
                {
                    if (model != null)
                    {
                        flag = await _personService.ForgotPassword(model);
                        scope.Complete();
                        if (flag != null)
                        {
                            return Response(true, _localizer["Password change successfully"].Value.ToString(), DropMessageType.Success);
                        }
                    }
                }
                return Response(false, "", DropMessageType.Error);
            });
        }

        #endregion Forgot Password

    }
}