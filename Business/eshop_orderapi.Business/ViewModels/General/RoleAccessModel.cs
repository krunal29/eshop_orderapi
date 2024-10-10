using eshop_orderapi.Business.Enums.General;

namespace eshop_orderapi.Business.ViewModels.General
{
    public class RoleAccessModel
    {
        public RoleAccessModel(ModuleEnum module, AccessTypeEnum accessType)
        {
            Module = module;
            AccessType = accessType;
        }

        public ModuleEnum Module { get; set; }

        public AccessTypeEnum AccessType { get; set; }
    }
}