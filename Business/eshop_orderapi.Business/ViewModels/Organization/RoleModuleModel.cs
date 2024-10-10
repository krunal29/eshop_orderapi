using System.Collections.Generic;

namespace eshop_orderapi.Business.ViewModels.Organization
{
    public class RoleModuleModel
    {
        public RoleModuleModel()
        {
            RoleModuleDetails = new List<RoleModuleDetailsModel>();
        }

        public int RoleId { get; set; }

        public List<RoleModuleDetailsModel> RoleModuleDetails { get; set; }
    }

    public class RoleModuleDetailsModel
    {
        public int Id { get; set; }

        public int RoleId { get; set; }

        public int ModuleId { get; set; }

        public bool Overview { get; set; }

        public bool View { get; set; }

        public bool Add { get; set; }

        public bool Edit { get; set; }

        public bool Delete { get; set; }
    }
}