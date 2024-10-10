using eshop_orderapi.Business.Enums.General;
using eshop_orderapi.Business.ViewModels.Organization;
using System;
using System.Collections.Generic;
using System.Linq;

namespace eshop_orderapi.Business.Helpers
{
    public static class AccessHelper
    {
        public static bool HasAccessToModuleForWeb(ModuleEnum module, AccessTypeEnum accessType)
        {
            var adminId = Constant.AdminId;

            RoleModuleModel roleModule = MemoryCacheHelper.memoryCache.Get<List<RoleModuleModel>>(Constant.Memory_UserRoles).FirstOrDefault(x => x.RoleId == adminId);
            if (roleModule == null) return false;

            var roleId = roleModule.RoleId;

            if (roleId == Convert.ToInt32(adminId)) return true;

            return HasRoleAccess(module, accessType, roleModule.RoleModuleDetails);
        }

        public static bool HasRoleAccess(ModuleEnum module, AccessTypeEnum accessType, List<RoleModuleDetailsModel> roleModule)
        {
            var hasAccess = false;
            var roleAccess = roleModule.FirstOrDefault(x => x.ModuleId == (long)module);
            if (roleAccess != null)
            {
                switch (accessType)
                {
                    case AccessTypeEnum.Overview:
                        hasAccess = roleAccess.Overview;
                        break;

                    case AccessTypeEnum.View:
                        hasAccess = roleAccess.View;
                        break;

                    case AccessTypeEnum.Add:
                        hasAccess = roleAccess.Add;
                        break;

                    case AccessTypeEnum.Edit:
                        hasAccess = roleAccess.Edit;
                        break;

                    case AccessTypeEnum.Delete:
                        hasAccess = roleAccess.Delete;
                        break;
                }
            }

            return hasAccess;
        }
    }
}