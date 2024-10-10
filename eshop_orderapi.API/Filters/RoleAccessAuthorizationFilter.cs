using Microsoft.AspNetCore.Mvc.Filters;
using eshop_orderapi.Business.Enums.General;
using eshop_orderapi.Business.Helpers;
using eshop_orderapi.Business.ViewModels;
using eshop_orderapi.Business.ViewModels.General;
using eshop_orderapi.Business.ViewModels.Organization;
using System;
using System.Collections.Generic;
using System.Linq;

namespace eshop_orderapi.API.Filters
{
    public class RoleAccessAuthorizationFilter : Attribute, IActionFilter
    {
        public static PersonModel ApplicationUserApiRequest { get; set; }

        public List<RoleAccessModel> _roleAccessList = new List<RoleAccessModel>();

        public RoleAccessAuthorizationFilter()
        {
        }

        public RoleAccessAuthorizationFilter(ModuleEnum module, AccessTypeEnum accessType)
        {
            _roleAccessList.Add(new RoleAccessModel(module, accessType));
        }

        public RoleAccessAuthorizationFilter(ModuleEnum[] modules, AccessTypeEnum[] accessTypes)
        {
            for (int i = 0; i < modules.Length; i++)
            {
                _roleAccessList.Add(new RoleAccessModel(modules[i], accessTypes[i]));
            }
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            //throw new NotImplementedException();
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            var roleId = JwtAuthenticationFilter.ApplicationUserApiRequest.RoleId;
            try
            {
                dynamic valuesCntlr = context.Controller as dynamic;
                if (valuesCntlr != null)
                {
                    if (roleId == Constant.AdminId)
                    {
                        return;
                    }
                }

                ///Added database or memory cache logic for get role module details every time
                var isAuthorized = false;

                //Get role access from Memory Cache based on current user RoleId
                var _roleModules = MemoryCacheHelper.memoryCache.Get<List<RoleModuleModel>>(Constant.Memory_UserRoles);
                if (_roleModules != null && _roleModules.Count > 0)
                {
                    var userRoleModules = _roleModules.FirstOrDefault(x => x.RoleId == roleId);
                    foreach (var item in _roleAccessList)
                    {
                        if (AccessHelper.HasRoleAccess(item.Module, item.AccessType, userRoleModules.RoleModuleDetails))
                        {
                            isAuthorized = true;
                            break;
                        }
                    }
                    if (!isAuthorized)
                    {
                        throw new UnauthorizedAccessException();
                    }
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}