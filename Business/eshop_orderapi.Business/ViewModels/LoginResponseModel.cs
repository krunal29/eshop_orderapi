using eshop_orderapi.Business.ViewModels.Organization;
using eshop_orderapi.Domain.Models;
using System.Collections.Generic;

namespace eshop_orderapi.Business.ViewModels
{
    public class LoginResponseModel
    {
        public ApplicationUser ApplicationUser { get; set; }

        public string Token { get; set; }

        public List<RoleModuleModel> roleModuleModel { get; set; }
    }
}