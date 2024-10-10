using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using eshop_orderapi.Business.Attributes;
using eshop_orderapi.Business.Enums.General;
using eshop_orderapi.Business.Helpers;
using eshop_orderapi.Business.ViewModels;
using eshop_orderapi.Helpers;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace eshop_orderapi.Controllers
{
    public class PersonController : BaseController
    {
        [Authorize]
        [AuthorizeUser(ModuleEnum.Dashboard, AccessTypeEnum.Overview)]
        public async Task<IActionResult> Index()
        {
            //Logger.Info("LogPrint");
            var result = await DoActionForGet<List<PersonModel>>(null, "Person/GetAll");
            return View(result.Data);
        }

        [HttpGet]
        public async Task<IActionResult> Add(int id = 0)
        {
            return View(new PersonModel());
        }

        [HttpPost]
        public async Task<PostJsonResult> Add(PersonModel person)
        {
            var users = SessionHelper.GetLoginUserInfo(HttpContext);
            if (users != null)
            {
                person.AspNetUserId = users.ApplicationUser.Id;
                person.RoleId = users.roleModuleModel.FirstOrDefault().RoleId;
                person.IsActive = true;
            }

            var result = await DoActionForPost<PersonModel>(person, "Person");
            return PostCompleteView(this, "Detail", result.Data);
        }
    }
}